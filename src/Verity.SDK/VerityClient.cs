using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Verity.SDK.Models;

namespace Verity.SDK
{
    /// <summary>
    /// Client for interacting with the Verity API
    /// </summary>
    public class VerityClient : IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _baseUrl;
        private bool _disposed;

        /// <summary>
        /// Creates a new Verity API client
        /// </summary>
        /// <param name="apiKey">Your Verity API key</param>
        /// <param name="baseUrl">Base URL for the API (optional)</param>
        public VerityClient(string apiKey, string baseUrl = "https://verity.backworkai.com/api/v1")
        {
            if (string.IsNullOrWhiteSpace(apiKey))
                throw new ArgumentException("API key is required", nameof(apiKey));

            _apiKey = apiKey;
            _baseUrl = baseUrl.TrimEnd('/');
            _httpClient = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(30)
            };
            _httpClient.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue("Bearer", apiKey);
            _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("verity-dotnet/1.0.0");
        }

        /// <summary>
        /// Check API health status
        /// </summary>
        public async Task<ApiResponse<HealthStatus>> HealthAsync(CancellationToken cancellationToken = default)
        {
            return await GetAsync<HealthStatus>("/health", cancellationToken);
        }

        /// <summary>
        /// Look up a medical code
        /// </summary>
        public async Task<ApiResponse<CodeLookupData>> LookupCodeAsync(
            string code,
            string? codeSystem = null,
            string? jurisdiction = null,
            string[]? include = null,
            bool fuzzy = true,
            CancellationToken cancellationToken = default)
        {
            var queryParams = new Dictionary<string, string>
            {
                ["code"] = code,
                ["fuzzy"] = fuzzy.ToString().ToLower()
            };

            if (!string.IsNullOrWhiteSpace(codeSystem))
                queryParams["code_system"] = codeSystem;
            
            if (!string.IsNullOrWhiteSpace(jurisdiction))
                queryParams["jurisdiction"] = jurisdiction;
            
            if (include != null && include.Length > 0)
                queryParams["include"] = string.Join(",", include);

            var path = BuildPath("/codes/lookup", queryParams);
            return await GetAsync<CodeLookupData>(path, cancellationToken);
        }

        /// <summary>
        /// Search and list policies
        /// </summary>
        public async Task<ApiResponse<List<PolicyListItem>>> ListPoliciesAsync(
            string? query = null,
            string mode = "keyword",
            string? policyType = null,
            string? jurisdiction = null,
            string? payer = null,
            string status = "active",
            string? cursor = null,
            int limit = 50,
            string[]? include = null,
            string? icd10 = null,
            string? format = null,
            CancellationToken cancellationToken = default)
        {
            var queryParams = new Dictionary<string, string>
            {
                ["mode"] = mode,
                ["status"] = status,
                ["limit"] = limit.ToString()
            };

            if (!string.IsNullOrWhiteSpace(query))
                queryParams["q"] = query;

            if (!string.IsNullOrWhiteSpace(policyType))
                queryParams["policy_type"] = policyType;

            if (!string.IsNullOrWhiteSpace(jurisdiction))
                queryParams["jurisdiction"] = jurisdiction;

            if (!string.IsNullOrWhiteSpace(payer))
                queryParams["payer"] = payer;

            if (!string.IsNullOrWhiteSpace(cursor))
                queryParams["cursor"] = cursor;

            if (include != null && include.Length > 0)
                queryParams["include"] = string.Join(",", include);

            if (!string.IsNullOrWhiteSpace(icd10))
                queryParams["icd10"] = icd10;

            if (!string.IsNullOrWhiteSpace(format))
                queryParams["format"] = format;

            var path = BuildPath("/policies", queryParams);
            return await GetAsync<List<PolicyListItem>>(path, cancellationToken);
        }

        /// <summary>
        /// Get a policy by ID
        /// </summary>
        public async Task<ApiResponse<PolicyDetail>> GetPolicyAsync(
            string policyId,
            string[]? include = null,
            CancellationToken cancellationToken = default)
        {
            var queryParams = new Dictionary<string, string>();
            
            if (include != null && include.Length > 0)
                queryParams["include"] = string.Join(",", include);

            var path = BuildPath($"/policies/{policyId}", queryParams);
            return await GetAsync<PolicyDetail>(path, cancellationToken);
        }

        /// <summary>
        /// Check prior authorization requirements
        /// </summary>
        public async Task<ApiResponse<PriorAuthResult>> CheckPriorAuthAsync(
            string[] procedureCodes,
            string[]? diagnosisCodes = null,
            string? state = null,
            string payer = "medicare",
            int criteriaPage = 1,
            int criteriaPerPage = 25,
            string? idempotencyKey = null,
            CancellationToken cancellationToken = default)
        {
            var body = new Dictionary<string, object>
            {
                ["procedure_codes"] = procedureCodes,
                ["payer"] = payer,
                ["criteria_page"] = criteriaPage,
                ["criteria_per_page"] = criteriaPerPage
            };

            if (diagnosisCodes != null && diagnosisCodes.Length > 0)
                body["diagnosis_codes"] = diagnosisCodes;
            
            if (!string.IsNullOrWhiteSpace(state))
                body["state"] = state;

            var headers = new Dictionary<string, string>();
            if (!string.IsNullOrWhiteSpace(idempotencyKey))
                headers["X-Idempotency-Key"] = idempotencyKey;

            return await PostAsync<PriorAuthResult>("/prior-auth/check", body, headers, cancellationToken);
        }

        /// <summary>
        /// Get policy change feed
        /// </summary>
        public async Task<ApiResponse<List<PolicyChange>>> GetPolicyChangesAsync(
            string? since = null,
            string? policyId = null,
            string? changeType = null,
            string? cursor = null,
            int limit = 50,
            CancellationToken cancellationToken = default)
        {
            var queryParams = new Dictionary<string, string>
            {
                ["limit"] = limit.ToString()
            };

            if (!string.IsNullOrWhiteSpace(since))
                queryParams["since"] = since;

            if (!string.IsNullOrWhiteSpace(policyId))
                queryParams["policy_id"] = policyId;

            if (!string.IsNullOrWhiteSpace(changeType))
                queryParams["change_type"] = changeType;

            if (!string.IsNullOrWhiteSpace(cursor))
                queryParams["cursor"] = cursor;

            var path = BuildPath("/policies/changes", queryParams);
            return await GetAsync<List<PolicyChange>>(path, cancellationToken);
        }

        /// <summary>
        /// Compare policies across jurisdictions
        /// </summary>
        public async Task<ApiResponse<object>> ComparePoliciesAsync(
            string[] procedureCodes,
            string? policyType = null,
            string[]? jurisdictions = null,
            string? idempotencyKey = null,
            CancellationToken cancellationToken = default)
        {
            var body = new Dictionary<string, object>
            {
                ["procedure_codes"] = procedureCodes
            };

            if (!string.IsNullOrWhiteSpace(policyType))
                body["policy_type"] = policyType;

            if (jurisdictions != null && jurisdictions.Length > 0)
                body["jurisdictions"] = jurisdictions;

            var headers = new Dictionary<string, string>();
            if (!string.IsNullOrWhiteSpace(idempotencyKey))
                headers["X-Idempotency-Key"] = idempotencyKey;

            return await PostAsync<object>("/policies/compare", body, headers, cancellationToken);
        }

        /// <summary>
        /// Search coverage criteria
        /// </summary>
        public async Task<ApiResponse<List<CriteriaBlock>>> SearchCriteriaAsync(
            string query,
            string? section = null,
            string? policyType = null,
            string? jurisdiction = null,
            string? cursor = null,
            int limit = 50,
            CancellationToken cancellationToken = default)
        {
            var queryParams = new Dictionary<string, string>
            {
                ["q"] = query,
                ["limit"] = limit.ToString()
            };

            if (!string.IsNullOrWhiteSpace(section))
                queryParams["section"] = section;

            if (!string.IsNullOrWhiteSpace(policyType))
                queryParams["policy_type"] = policyType;

            if (!string.IsNullOrWhiteSpace(jurisdiction))
                queryParams["jurisdiction"] = jurisdiction;

            if (!string.IsNullOrWhiteSpace(cursor))
                queryParams["cursor"] = cursor;

            var path = BuildPath("/coverage/criteria", queryParams);
            return await GetAsync<List<CriteriaBlock>>(path, cancellationToken);
        }

        /// <summary>
        /// List MAC jurisdictions
        /// </summary>
        public async Task<ApiResponse<List<Jurisdiction>>> ListJurisdictionsAsync(
            CancellationToken cancellationToken = default)
        {
            return await GetAsync<List<Jurisdiction>>("/jurisdictions", cancellationToken);
        }

        /// <summary>
        /// Research prior authorization requirements using AI-powered web research
        /// </summary>
        public async Task<ApiResponse<PriorAuthResearchResult>> ResearchPriorAuthAsync(
            string[] procedureCodes,
            string? payer = null,
            string? state = null,
            string[]? diagnosisCodes = null,
            string? clinicalContext = null,
            bool sync = false,
            CancellationToken cancellationToken = default)
        {
            var body = new Dictionary<string, object>
            {
                ["procedure_codes"] = procedureCodes,
                ["sync"] = sync
            };

            if (!string.IsNullOrWhiteSpace(payer))
                body["payer"] = payer;

            if (!string.IsNullOrWhiteSpace(state))
                body["state"] = state;

            if (diagnosisCodes != null && diagnosisCodes.Length > 0)
                body["diagnosis_codes"] = diagnosisCodes;

            if (!string.IsNullOrWhiteSpace(clinicalContext))
                body["clinical_context"] = clinicalContext;

            return await PostAsync<PriorAuthResearchResult>("/prior-auth/research", body, null, cancellationToken);
        }

        /// <summary>
        /// Get the status and results of a prior authorization research task
        /// </summary>
        public async Task<ApiResponse<PriorAuthResearchResult>> GetPriorAuthResearchAsync(
            string researchId,
            CancellationToken cancellationToken = default)
        {
            return await GetAsync<PriorAuthResearchResult>($"/prior-auth/research/{researchId}", cancellationToken);
        }

        /// <summary>
        /// Get Medicaid spending data by HCPCS code
        /// </summary>
        public async Task<ApiResponse<Dictionary<string, CodeSpendingData>>> GetSpendingByCodeAsync(
            string? code = null,
            string[]? codes = null,
            int? year = null,
            CancellationToken cancellationToken = default)
        {
            var queryParams = new Dictionary<string, string>();

            if (!string.IsNullOrWhiteSpace(code))
                queryParams["code"] = code;
            else if (codes != null && codes.Length > 0)
                queryParams["codes"] = string.Join(",", codes);

            if (year.HasValue)
                queryParams["year"] = year.Value.ToString();

            var path = BuildPath("/spending/by-code", queryParams);
            return await GetAsync<Dictionary<string, CodeSpendingData>>(path, cancellationToken);
        }

        /// <summary>
        /// Batch look up multiple medical codes in a single request
        /// </summary>
        public async Task<ApiResponse<BatchCodeLookupData>> BatchLookupCodesAsync(
            string[] codes,
            string? codeSystem = null,
            string[]? include = null,
            CancellationToken cancellationToken = default)
        {
            var body = new Dictionary<string, object>
            {
                ["codes"] = codes
            };

            if (!string.IsNullOrWhiteSpace(codeSystem))
                body["code_system"] = codeSystem;

            if (include != null && include.Length > 0)
                body["include"] = include;

            return await PostAsync<BatchCodeLookupData>("/codes/batch", body, null, cancellationToken);
        }

        /// <summary>
        /// Evaluate coverage for a policy against a set of parameters
        /// </summary>
        public async Task<ApiResponse<CoverageEvaluationData>> EvaluateCoverageAsync(
            string policyId,
            Dictionary<string, object> parameters,
            CancellationToken cancellationToken = default)
        {
            var body = new Dictionary<string, object>
            {
                ["policy_id"] = policyId,
                ["parameters"] = parameters
            };

            return await PostAsync<CoverageEvaluationData>("/coverage/evaluate", body, null, cancellationToken);
        }

        /// <summary>
        /// List all webhook endpoints
        /// </summary>
        public async Task<ApiResponse<List<WebhookEndpoint>>> ListWebhooksAsync(
            CancellationToken cancellationToken = default)
        {
            return await GetAsync<List<WebhookEndpoint>>("/webhooks", cancellationToken);
        }

        /// <summary>
        /// Create a new webhook endpoint
        /// </summary>
        public async Task<ApiResponse<WebhookEndpoint>> CreateWebhookAsync(
            string url,
            string[] events,
            CancellationToken cancellationToken = default)
        {
            var body = new Dictionary<string, object>
            {
                ["url"] = url,
                ["events"] = events
            };

            return await PostAsync<WebhookEndpoint>("/webhooks", body, null, cancellationToken);
        }

        /// <summary>
        /// Update an existing webhook endpoint
        /// </summary>
        public async Task<ApiResponse<WebhookEndpoint>> UpdateWebhookAsync(
            int webhookId,
            string? url = null,
            string[]? events = null,
            string? status = null,
            CancellationToken cancellationToken = default)
        {
            var body = new Dictionary<string, object>();

            if (!string.IsNullOrWhiteSpace(url))
                body["url"] = url;

            if (events != null && events.Length > 0)
                body["events"] = events;

            if (!string.IsNullOrWhiteSpace(status))
                body["status"] = status;

            return await PatchAsync<WebhookEndpoint>($"/webhooks/{webhookId}", body, cancellationToken);
        }

        /// <summary>
        /// Delete a webhook endpoint
        /// </summary>
        public async Task<ApiResponse<object>> DeleteWebhookAsync(
            int webhookId,
            CancellationToken cancellationToken = default)
        {
            return await DeleteAsync<object>($"/webhooks/{webhookId}", cancellationToken);
        }

        /// <summary>
        /// Send a test event to a webhook endpoint
        /// </summary>
        public async Task<ApiResponse<WebhookTestResult>> TestWebhookAsync(
            int webhookId,
            CancellationToken cancellationToken = default)
        {
            return await PostAsync<WebhookTestResult>($"/webhooks/{webhookId}/test", new { }, null, cancellationToken);
        }

        private async Task<ApiResponse<T>> GetAsync<T>(string path, CancellationToken cancellationToken)
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}{path}", cancellationToken);
            return await ProcessResponseAsync<T>(response);
        }

        private async Task<ApiResponse<T>> PostAsync<T>(
            string path,
            object body,
            Dictionary<string, string>? headers = null,
            CancellationToken cancellationToken = default)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, $"{_baseUrl}{path}")
            {
                Content = new StringContent(
                    JsonConvert.SerializeObject(body),
                    Encoding.UTF8,
                    "application/json")
            };

            if (headers != null)
            {
                foreach (var header in headers)
                {
                    request.Headers.Add(header.Key, header.Value);
                }
            }

            var response = await _httpClient.SendAsync(request, cancellationToken);
            return await ProcessResponseAsync<T>(response);
        }

        private async Task<ApiResponse<T>> PatchAsync<T>(
            string path,
            object body,
            CancellationToken cancellationToken = default)
        {
            var request = new HttpRequestMessage(new HttpMethod("PATCH"), $"{_baseUrl}{path}")
            {
                Content = new StringContent(
                    JsonConvert.SerializeObject(body),
                    Encoding.UTF8,
                    "application/json")
            };

            var response = await _httpClient.SendAsync(request, cancellationToken);
            return await ProcessResponseAsync<T>(response);
        }

        private async Task<ApiResponse<T>> DeleteAsync<T>(
            string path,
            CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.DeleteAsync($"{_baseUrl}{path}", cancellationToken);
            return await ProcessResponseAsync<T>(response);
        }

        private async Task<ApiResponse<T>> ProcessResponseAsync<T>(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();
            
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiResponse<T>>(content)!;
            }

            var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(content);
            if (errorResponse?.Error != null)
            {
                throw new VerityException(
                    errorResponse.Error.Message,
                    errorResponse.Error.Code,
                    (int)response.StatusCode);
            }

            throw new VerityException($"HTTP {(int)response.StatusCode}", null, (int)response.StatusCode);
        }

        private static string BuildPath(string basePath, Dictionary<string, string> queryParams)
        {
            if (queryParams.Count == 0)
                return basePath;

            var query = string.Join("&", 
                Array.ConvertAll(queryParams.Keys.ToArray(), 
                    key => $"{Uri.EscapeDataString(key)}={Uri.EscapeDataString(queryParams[key])}"));

            return $"{basePath}?{query}";
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _httpClient?.Dispose();
                _disposed = true;
            }
        }
    }
}
