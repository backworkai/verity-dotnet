using System;

namespace Verity.SDK
{
    /// <summary>
    /// Exception thrown when an error occurs with the Verity API
    /// </summary>
    public class VerityException : Exception
    {
        /// <summary>
        /// Error code from the API
        /// </summary>
        public string? Code { get; }

        /// <summary>
        /// HTTP status code
        /// </summary>
        public int StatusCode { get; }

        public VerityException(string message, string? code = null, int statusCode = 0)
            : base(message)
        {
            Code = code;
            StatusCode = statusCode;
        }

        public VerityException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }

    /// <summary>
    /// Exception thrown when authentication fails
    /// </summary>
    public class AuthenticationException : VerityException
    {
        public AuthenticationException(string message, string? code = null)
            : base(message, code, 401)
        {
        }
    }

    /// <summary>
    /// Exception thrown when a resource is not found
    /// </summary>
    public class NotFoundException : VerityException
    {
        public NotFoundException(string message, string? code = null)
            : base(message, code, 404)
        {
        }
    }

    /// <summary>
    /// Exception thrown when rate limit is exceeded
    /// </summary>
    public class RateLimitException : VerityException
    {
        public RateLimitException(string message, string? code = null)
            : base(message, code, 429)
        {
        }
    }
}
