using System.Security.Claims;

namespace ERP.HRM.API.Middlewares
{
    /// <summary>
    /// Middleware for capturing audit information (user, timestamp, operation)
    /// </summary>
    public class AuditLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<AuditLoggingMiddleware> _logger;

        public AuditLoggingMiddleware(RequestDelegate next, ILogger<AuditLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "Anonymous";
            var userName = context.User.FindFirst(ClaimTypes.Name)?.Value ?? "Unknown";
            var method = context.Request.Method;
            var path = context.Request.Path;
            var timestamp = DateTime.UtcNow;

            // Store audit info in HttpContext for later use
            context.Items["AuditUserId"] = userId;
            context.Items["AuditUserName"] = userName;
            context.Items["AuditTimestamp"] = timestamp;

            _logger.LogInformation(
                "Audit: User '{UserName}' ({UserId}) performed {Method} on {Path} at {Timestamp:O}",
                userName, userId, method, path, timestamp);

            await _next(context);
        }
    }
}
