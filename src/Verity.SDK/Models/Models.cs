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
}
