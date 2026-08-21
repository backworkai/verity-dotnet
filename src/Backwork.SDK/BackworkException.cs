using System;

namespace Backwork.SDK
{
    /// <summary>
    /// Exception thrown when an error occurs with the Backwork API
    /// </summary>
    public class BackworkException : Exception
    {
        public string? Code { get; }

        public int StatusCode { get; }

        public BackworkException(string message, string? code = null, int statusCode = 0)
            : base(message)
        {
            Code = code;
            StatusCode = statusCode;
        }

        public BackworkException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }

    /// <summary>
    /// Exception thrown when authentication fails
    /// </summary>
    public class AuthenticationException : BackworkException
    {
        public AuthenticationException(string message, string? code = null)
            : base(message, code, 401)
        {
        }
    }

    /// <summary>
    /// Exception thrown when a resource is not found
    /// </summary>
    public class NotFoundException : BackworkException
    {
        public NotFoundException(string message, string? code = null)
            : base(message, code, 404)
        {
        }
    }

    /// <summary>
    /// Exception thrown when rate limit is exceeded
    /// </summary>
    public class RateLimitException : BackworkException
    {
        public RateLimitException(string message, string? code = null)
            : base(message, code, 429)
        {
        }
    }
}
