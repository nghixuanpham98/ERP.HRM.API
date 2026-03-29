using System.Collections.Concurrent;

namespace ERP.HRM.API.Middlewares
{
    /// <summary>
    /// Simple rate limiting middleware (IP-based)
    /// For production, use more sophisticated solutions like AspNetCoreRateLimit package
    /// </summary>
    public class RateLimitingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RateLimitingMiddleware> _logger;
        private static readonly ConcurrentDictionary<string, RateLimitEntry> _clientRequests = 
            new();

        private const int MaxRequests = 100;
        private const int TimeWindowSeconds = 60;

        public RateLimitingMiddleware(RequestDelegate next, ILogger<RateLimitingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var clientId = GetClientIdentifier(context);
            var now = DateTime.UtcNow;

            if (!_clientRequests.TryGetValue(clientId, out var entry))
            {
                entry = new RateLimitEntry { FirstRequestTime = now, RequestCount = 1 };
                _clientRequests.AddOrUpdate(clientId, entry, (key, old) => entry);
                await _next(context);
                return;
            }

            // Check if time window has expired
            if ((now - entry.FirstRequestTime).TotalSeconds > TimeWindowSeconds)
            {
                entry.FirstRequestTime = now;
                entry.RequestCount = 1;
                await _next(context);
                return;
            }

            // Check if request limit exceeded
            if (entry.RequestCount >= MaxRequests)
            {
                _logger.LogWarning("Rate limit exceeded for client: {ClientId}", clientId);
                context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                context.Response.Headers.Add("Retry-After", "60");
                await context.Response.WriteAsync("Too many requests. Please try again later.");
                return;
            }

            entry.RequestCount++;
            await _next(context);
        }

        private static string GetClientIdentifier(HttpContext context)
        {
            // Try to get user ID first, then fall back to IP address
            var userId = context.User?.FindFirst("sub")?.Value;
            if (!string.IsNullOrEmpty(userId))
                return $"user_{userId}";

            var remoteIp = context.Connection.RemoteIpAddress?.ToString();
            return $"ip_{remoteIp}";
        }

        private class RateLimitEntry
        {
            public DateTime FirstRequestTime { get; set; }
            public int RequestCount { get; set; }
        }
    }
}
