namespace ERP.HRM.Application.Common
{
    /// <summary>
    /// Standardized error response model
    /// </summary>
    public class ErrorResponse
    {
        public bool Success { get; set; } = false;

        /// <summary>
        /// Error code for API clients to handle specific error types
        /// </summary>
        public string ErrorCode { get; set; } = "UNKNOWN_ERROR";

        /// <summary>
        /// Full name of the exception type
        /// </summary>
        public string ErrorType { get; set; } = string.Empty;

        /// <summary>
        /// User-friendly error message
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Validation errors (for validation exceptions)
        /// </summary>
        public IEnumerable<string>? Errors { get; set; }

        /// <summary>
        /// Detailed error information (stack trace in development)
        /// </summary>
        public string? Detail { get; set; }

        /// <summary>
        /// Timestamp when the error occurred
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Unique request/trace ID for correlation
        /// </summary>
        public string? TraceId { get; set; }
    }
}
