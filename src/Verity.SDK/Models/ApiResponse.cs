using Newtonsoft.Json;

namespace Verity.SDK.Models
{
    public class ApiResponse<T>
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("data")]
        public T? Data { get; set; }

        [JsonProperty("meta")]
        public object? Meta { get; set; }
    }

    public class ErrorResponse
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("error")]
        public ErrorDetail? Error { get; set; }
    }

    public class ErrorDetail
    {
        [JsonProperty("code")]
        public string? Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; } = string.Empty;

        [JsonProperty("hint")]
        public string? Hint { get; set; }
    }
}
