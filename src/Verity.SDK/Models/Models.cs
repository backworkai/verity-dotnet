using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Verity.SDK.Models
{
    public class HealthStatus
    {
        [JsonProperty("status")]
        public string Status { get; set; } = string.Empty;

        [JsonProperty("version")]
        public string Version { get; set; } = string.Empty;

        [JsonProperty("timestamp")]
        public DateTime Timestamp { get; set; }
    }

    public class CodeLookupData
    {
        [JsonProperty("code")]
        public string Code { get; set; } = string.Empty;

        [JsonProperty("code_system")]
        public string CodeSystem { get; set; } = string.Empty;

        [JsonProperty("found")]
        public bool Found { get; set; }

        [JsonProperty("description")]
        public string? Description { get; set; }

        [JsonProperty("short_description")]
        public string? ShortDescription { get; set; }

        [JsonProperty("rvu")]
        public RvuData? Rvu { get; set; }

        [JsonProperty("policies")]
        public List<PolicyMatch>? Policies { get; set; }
    }

    public class RvuData
    {
        [JsonProperty("work_rvu")]
        public string? WorkRvu { get; set; }

        [JsonProperty("non_facility_price")]
        public string? NonFacilityPrice { get; set; }

        [JsonProperty("facility_price")]
        public string? FacilityPrice { get; set; }

        [JsonProperty("year")]
        public int? Year { get; set; }
    }

    public class PolicyMatch
    {
        [JsonProperty("policy_id")]
        public string PolicyId { get; set; } = string.Empty;

        [JsonProperty("title")]
        public string Title { get; set; } = string.Empty;

        [JsonProperty("policy_type")]
        public string PolicyType { get; set; } = string.Empty;

        [JsonProperty("disposition")]
        public string Disposition { get; set; } = string.Empty;

        [JsonProperty("jurisdiction")]
        public string? Jurisdiction { get; set; }

        [JsonProperty("effective_date")]
        public string? EffectiveDate { get; set; }
    }

    public class PolicyListItem
    {
        [JsonProperty("policy_id")]
        public string PolicyId { get; set; } = string.Empty;

        [JsonProperty("title")]
        public string Title { get; set; } = string.Empty;

        [JsonProperty("policy_type")]
        public string PolicyType { get; set; } = string.Empty;

        [JsonProperty("jurisdiction")]
        public string? Jurisdiction { get; set; }

        [JsonProperty("effective_date")]
        public string? EffectiveDate { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; } = string.Empty;

        [JsonProperty("summary")]
        public string? Summary { get; set; }
    }

    public class PolicyDetail : PolicyListItem
    {
        [JsonProperty("description")]
        public string? Description { get; set; }

        [JsonProperty("version")]
        public string? Version { get; set; }

        [JsonProperty("pdf_url")]
        public string? PdfUrl { get; set; }

        [JsonProperty("specialty")]
        public List<string>? Specialty { get; set; }

        [JsonProperty("keywords")]
        public List<string>? Keywords { get; set; }
    }

    public class PriorAuthResult
    {
        [JsonProperty("pa_required")]
        public bool PaRequired { get; set; }

        [JsonProperty("confidence")]
        public string Confidence { get; set; } = string.Empty;

        [JsonProperty("reason")]
        public string Reason { get; set; } = string.Empty;

        [JsonProperty("matched_policies")]
        public List<PolicyMatch>? MatchedPolicies { get; set; }

        [JsonProperty("documentation_checklist")]
        public List<string>? DocumentationChecklist { get; set; }
    }

    public class PolicyChange
    {
        [JsonProperty("policy_id")]
        public string PolicyId { get; set; } = string.Empty;

        [JsonProperty("change_type")]
        public string ChangeType { get; set; } = string.Empty;

        [JsonProperty("change_summary")]
        public string? ChangeSummary { get; set; }

        [JsonProperty("changed_fields")]
        public List<string>? ChangedFields { get; set; }

        [JsonProperty("old_version")]
        public string? OldVersion { get; set; }

        [JsonProperty("new_version")]
        public string? NewVersion { get; set; }

        [JsonProperty("timestamp")]
        public string? Timestamp { get; set; }
    }

    public class Jurisdiction
    {
        [JsonProperty("mac_name")]
        public string MacName { get; set; } = string.Empty;

        [JsonProperty("mac_code")]
        public string? MacCode { get; set; }

        [JsonProperty("jurisdiction_code")]
        public string JurisdictionCode { get; set; } = string.Empty;

        [JsonProperty("jurisdiction_name")]
        public string? JurisdictionName { get; set; }

        [JsonProperty("states")]
        public List<string>? States { get; set; }
    }

    public class CriteriaBlock
    {
        [JsonProperty("block_id")]
        public string? BlockId { get; set; }

        [JsonProperty("text")]
        public string? Text { get; set; }

        [JsonProperty("tags")]
        public List<string>? Tags { get; set; }

        [JsonProperty("policy_id")]
        public string? PolicyId { get; set; }

        [JsonProperty("policy_title")]
        public string? PolicyTitle { get; set; }

        [JsonProperty("section")]
        public string? Section { get; set; }
    }

    public class PriorAuthResearchResult
    {
        [JsonProperty("research_id")]
        public string ResearchId { get; set; } = string.Empty;

        [JsonProperty("status")]
        public string Status { get; set; } = string.Empty;

        [JsonProperty("created_at")]
        public string? CreatedAt { get; set; }

        [JsonProperty("finished_at")]
        public string? FinishedAt { get; set; }

        [JsonProperty("poll_url")]
        public string? PollUrl { get; set; }

        [JsonProperty("result")]
        public Dictionary<string, object>? Result { get; set; }

        [JsonProperty("cost")]
        public ResearchCost? Cost { get; set; }

        [JsonProperty("error")]
        public string? Error { get; set; }
    }

    public class ResearchCost
    {
        [JsonProperty("num_searches")]
        public int NumSearches { get; set; }

        [JsonProperty("num_pages")]
        public int NumPages { get; set; }

        [JsonProperty("reasoning_tokens")]
        public int ReasoningTokens { get; set; }

        [JsonProperty("total_dollars")]
        public double TotalDollars { get; set; }
    }

    public class CodeSpendingData
    {
        [JsonProperty("total_paid")]
        public string TotalPaid { get; set; } = string.Empty;

        [JsonProperty("total_claims")]
        public int TotalClaims { get; set; }

        [JsonProperty("unique_beneficiaries")]
        public int UniqueBeneficiaries { get; set; }

        [JsonProperty("unique_providers")]
        public int UniqueProviders { get; set; }

        [JsonProperty("date_range")]
        public DateRange? DateRange { get; set; }

        [JsonProperty("by_year")]
        public List<YearlySpending>? ByYear { get; set; }
    }

    public class DateRange
    {
        [JsonProperty("min")]
        public string? Min { get; set; }

        [JsonProperty("max")]
        public string? Max { get; set; }
    }

    public class YearlySpending
    {
        [JsonProperty("year")]
        public int Year { get; set; }

        [JsonProperty("total_paid")]
        public string TotalPaid { get; set; } = string.Empty;

        [JsonProperty("total_claims")]
        public int TotalClaims { get; set; }

        [JsonProperty("unique_beneficiaries")]
        public int UniqueBeneficiaries { get; set; }
    }

    public class BatchCodeLookupData
    {
        [JsonProperty("results")]
        public Dictionary<string, CodeLookupData> Results { get; set; } = new();
    }

    public class CoverageEvaluationData
    {
        [JsonProperty("covered")] public bool Covered { get; set; }
        [JsonProperty("confidence")] public double Confidence { get; set; }
        [JsonProperty("reasons")] public List<string> Reasons { get; set; } = new();
        [JsonProperty("matched_criteria")] public List<string> MatchedCriteria { get; set; } = new();
        [JsonProperty("unmatched_criteria")] public List<string> UnmatchedCriteria { get; set; } = new();
        [JsonProperty("skipped_criteria")] public List<string> SkippedCriteria { get; set; } = new();
        [JsonProperty("blocks_evaluated")] public int BlocksEvaluated { get; set; }
        [JsonProperty("blocks_without_ast")] public int BlocksWithoutAst { get; set; }
        [JsonProperty("policy")] public PolicyRef? Policy { get; set; }
    }

    public class PolicyRef
    {
        [JsonProperty("policy_id")] public string PolicyId { get; set; } = "";
        [JsonProperty("title")] public string Title { get; set; } = "";
        [JsonProperty("policy_type")] public string PolicyType { get; set; } = "";
    }

    public class WebhookEndpoint
    {
        [JsonProperty("id")] public int Id { get; set; }
        [JsonProperty("url")] public string Url { get; set; } = "";
        [JsonProperty("events")] public List<string> Events { get; set; } = new();
        [JsonProperty("status")] public string Status { get; set; } = "";
        [JsonProperty("failure_count")] public int FailureCount { get; set; }
        [JsonProperty("secret")] public string? Secret { get; set; }
        [JsonProperty("created_at")] public string? CreatedAt { get; set; }
        [JsonProperty("updated_at")] public string? UpdatedAt { get; set; }
    }

    public class WebhookTestResult
    {
        [JsonProperty("delivery_id")] public int DeliveryId { get; set; }
        [JsonProperty("endpoint_id")] public int EndpointId { get; set; }
        [JsonProperty("event")] public string Event { get; set; } = "";
        [JsonProperty("http_status")] public int? HttpStatus { get; set; }
        [JsonProperty("success")] public bool Success { get; set; }
        [JsonProperty("error")] public string? Error { get; set; }
        [JsonProperty("created_at")] public string? CreatedAt { get; set; }
    }
}
