using ERP.HRM.API;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace ERP.HRM.API.HealthChecks
{
    /// <summary>
    /// Health check response model
    /// </summary>
    public class HealthCheckResponse
    {
        public string Status { get; set; } = "Healthy";
        public string Timestamp { get; set; } = DateTime.UtcNow.ToString("O");
        public Dictionary<string, object> Checks { get; set; } = new();
    }

    /// <summary>
    /// Custom health check for database connectivity
    /// </summary>
    public class DatabaseHealthCheck : IHealthCheck
    {
        private readonly ERPDbContext _context;
        private readonly ILogger<DatabaseHealthCheck> _logger;

        public DatabaseHealthCheck(ERPDbContext context, ILogger<DatabaseHealthCheck> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                var canConnect = await _context.Database.CanConnectAsync(cancellationToken);
                if (canConnect)
                {
                    _logger.LogInformation("Database health check passed");
                    return HealthCheckResult.Healthy("Database connection successful");
                }

                _logger.LogWarning("Database health check failed: Cannot connect to database");
                return HealthCheckResult.Unhealthy("Cannot connect to database");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Database health check failed with exception");
                return HealthCheckResult.Unhealthy("Database health check failed", ex);
            }
        }
    }
}
