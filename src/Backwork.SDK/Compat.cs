using System;

// Compatibility shims for the Verity -> Backwork rename. Code written against the published
// Verity.SDK 1.0.2 package keeps compiling against this assembly. Everything here is obsolete and
// exists only until consumers migrate. Do not add anything new to this namespace.
namespace Verity.SDK
{
    // This is the base class of BackworkException rather than a subclass of it. A subclass would
    // never be thrown, so existing catch clauses would stop firing with no compile error to warn
    // anyone. It carries Code and StatusCode so the inherited surface is unchanged by the rename.
    /// <summary>
    /// Former name of <see cref="Backwork.SDK.BackworkException"/>.
    /// </summary>
    [Obsolete("Renamed to Backwork.SDK.BackworkException.")]
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
    /// Former name of <see cref="Backwork.SDK.BackworkClient"/>.
    /// </summary>
    [Obsolete("Renamed to Backwork.SDK.BackworkClient.")]
    public class VerityClient : global::Backwork.SDK.BackworkClient
    {
        // These forward instead of restating the default base URL, so BackworkClient stays the
        // single place that owns it.
        public VerityClient(string apiKey)
            : base(apiKey)
        {
        }

        public VerityClient(string apiKey, string baseUrl)
            : base(apiKey, baseUrl)
        {
        }
    }
}
