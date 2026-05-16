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

        [JsonProperty("negotiated_rates")]
        public NegotiatedRateSummary? NegotiatedRates { get; set; }
    }

    public class NegotiatedRateSummary
    {
        [JsonProperty("min_rate")] public string? MinRate { get; set; }
        [JsonProperty("max_rate")] public string? MaxRate { get; set; }
        [JsonProperty("avg_rate")] public string? AvgRate { get; set; }
        [JsonProperty("num_rates")] public int? NumRates { get; set; }
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

    public class ClaimValidationData
    {
        [JsonProperty("payer")] public string? Payer { get; set; }
        [JsonProperty("plan_type")] public string? PlanType { get; set; }
        [JsonProperty("line_of_business")] public string? LineOfBusiness { get; set; }
        [JsonProperty("state")] public string? State { get; set; }
        [JsonProperty("site_of_service")] public string? SiteOfService { get; set; }
        [JsonProperty("provider_specialty")] public string? ProviderSpecialty { get; set; }
        [JsonProperty("modifiers")] public List<string> Modifiers { get; set; } = new();
        [JsonProperty("overall_risk")] public string OverallRisk { get; set; } = "";
        [JsonProperty("coverage_status")] public string CoverageStatus { get; set; } = "";
        [JsonProperty("prior_auth_required")] public bool PriorAuthRequired { get; set; }
        [JsonProperty("denial_risk")] public string DenialRisk { get; set; } = "";
        [JsonProperty("confidence")] public string Confidence { get; set; } = "";
        [JsonProperty("documentation_requirements")] public List<string> DocumentationRequirements { get; set; } = new();
        [JsonProperty("policy_sources")] public List<Dictionary<string, object>> PolicySources { get; set; } = new();
        [JsonProperty("requires_manual_review")] public bool RequiresManualReview { get; set; }
        [JsonProperty("known_gaps")] public List<string> KnownGaps { get; set; } = new();
        [JsonProperty("codes")] public List<Dictionary<string, object>> Codes { get; set; } = new();
        [JsonProperty("mac")] public Dictionary<string, object>? Mac { get; set; }
    }

    public class UnreviewedChange
    {
        [JsonProperty("diff_id")] public int DiffId { get; set; }
        [JsonProperty("policy_id")] public string PolicyId { get; set; } = "";
        [JsonProperty("policy_title")] public string PolicyTitle { get; set; } = "";
        [JsonProperty("policy_type")] public string PolicyType { get; set; } = "";
        [JsonProperty("payer_name")] public string? PayerName { get; set; }
        [JsonProperty("change_type")] public string ChangeType { get; set; } = "";
        [JsonProperty("change_summary")] public string ChangeSummary { get; set; } = "";
        [JsonProperty("changed_at")] public string ChangedAt { get; set; } = "";
    }

    public class AcknowledgeChangeData
    {
        [JsonProperty("id")] public int? Id { get; set; }
        [JsonProperty("acknowledged")] public bool Acknowledged { get; set; }
        [JsonProperty("already_acked")] public bool AlreadyAcked { get; set; }
    }

    public class BulkAcknowledgeChangesData
    {
        [JsonProperty("acknowledged")] public int Acknowledged { get; set; }
        [JsonProperty("already_acked")] public int AlreadyAcked { get; set; }
        [JsonProperty("invalid_ids")] public List<int> InvalidIds { get; set; } = new();
        [JsonProperty("total")] public int Total { get; set; }
    }

    public class ComplianceStats
    {
        [JsonProperty("total_changes_30d")] public int TotalChanges30d { get; set; }
        [JsonProperty("acknowledged_count")] public int AcknowledgedCount { get; set; }
        [JsonProperty("unreviewed_count")] public int UnreviewedCount { get; set; }
        [JsonProperty("acknowledgment_rate")] public int AcknowledgmentRate { get; set; }
        [JsonProperty("critical_unreviewed")] public int CriticalUnreviewed { get; set; }
    }

    public class DrugFormularyEvidence
    {
        [JsonProperty("source")] public string Source { get; set; } = "";
        [JsonProperty("payer_name")] public string PayerName { get; set; } = "";
        [JsonProperty("pbm_name")] public string? PbmName { get; set; }
        [JsonProperty("formulary_name")] public string? FormularyName { get; set; }
        [JsonProperty("plan_year")] public int? PlanYear { get; set; }
        [JsonProperty("effective_date")] public string? EffectiveDate { get; set; }
        [JsonProperty("drug_name")] public string DrugName { get; set; } = "";
        [JsonProperty("matched_text")] public string? MatchedText { get; set; }
        [JsonProperty("therapeutic_category")] public string? TherapeuticCategory { get; set; }
        [JsonProperty("drug_class")] public string? DrugClass { get; set; }
        [JsonProperty("tier")] public string? Tier { get; set; }
        [JsonProperty("coverage_status")] public string? CoverageStatus { get; set; }
        [JsonProperty("requirements")] public Dictionary<string, object>? Requirements { get; set; }
        [JsonProperty("alternatives")] public string? Alternatives { get; set; }
        [JsonProperty("preferred_alternatives")] public string? PreferredAlternatives { get; set; }
        [JsonProperty("source_url")] public string? SourceUrl { get; set; }
        [JsonProperty("source_page")] public int? SourcePage { get; set; }
    }
}
