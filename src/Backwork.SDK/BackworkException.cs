using System;

// The base type below is the pre-rename VerityException and is deliberately obsolete.
#pragma warning disable 612, 618

namespace Backwork.SDK
{
    // BackworkException derives from the pre-rename Verity.SDK.VerityException (see Compat.cs)
    // so that existing `catch (VerityException)` blocks still catch errors raised by this client.
    // A side-by-side alias would compile but silently stop matching, turning a rename into a
    // runtime break for consumers.
    /// <summary>
    /// Exception thrown when an error occurs with the Backwork API
    /// </summary>
    public class BackworkException : global::Verity.SDK.VerityException
    {
        public BackworkException(string message, string? code = null, int statusCode = 0)
            : base(message, code, statusCode)
        {
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

#pragma warning restore 612, 618
