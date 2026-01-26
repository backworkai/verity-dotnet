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
