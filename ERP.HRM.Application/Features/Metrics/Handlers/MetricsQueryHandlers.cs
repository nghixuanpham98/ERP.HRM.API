using ERP.HRM.Application.DTOs.Metrics;
using ERP.HRM.Application.Features.Metrics.Queries;
using ERP.HRM.Application.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ERP.HRM.Application.Features.Metrics.Handlers
{
    /// <summary>
    /// Handler for GetDepartmentMetricsQuery
    /// </summary>
    public class GetDepartmentMetricsQueryHandler : IRequestHandler<GetDepartmentMetricsQuery, DepartmentMetricsDto>
    {
        private readonly IMetricsService _metricsService;
        private readonly ILogger<GetDepartmentMetricsQueryHandler> _logger;

        public GetDepartmentMetricsQueryHandler(IMetricsService metricsService, ILogger<GetDepartmentMetricsQueryHandler> logger)
        {
            _metricsService = metricsService;
            _logger = logger;
        }

        public async Task<DepartmentMetricsDto> Handle(GetDepartmentMetricsQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetDepartmentMetricsQuery for department {DepartmentId}", request.DepartmentId);
            return await _metricsService.GetDepartmentMetricsAsync(request.DepartmentId, cancellationToken);
        }
    }

    /// <summary>
    /// Handler for GetDepartmentComparisonQuery
    /// </summary>
    public class GetDepartmentComparisonQueryHandler : IRequestHandler<GetDepartmentComparisonQuery, DepartmentComparisonDto>
    {
        private readonly IMetricsService _metricsService;
        private readonly ILogger<GetDepartmentComparisonQueryHandler> _logger;

        public GetDepartmentComparisonQueryHandler(IMetricsService metricsService, ILogger<GetDepartmentComparisonQueryHandler> logger)
        {
            _metricsService = metricsService;
            _logger = logger;
        }

        public async Task<DepartmentComparisonDto> Handle(GetDepartmentComparisonQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetDepartmentComparisonQuery");
            return await _metricsService.GetDepartmentComparisonAsync(cancellationToken);
        }
    }

    /// <summary>
    /// Handler for GetEmployeeDistributionQuery
    /// </summary>
    public class GetEmployeeDistributionQueryHandler : IRequestHandler<GetEmployeeDistributionQuery, List<EmployeeDistributionDto>>
    {
        private readonly IMetricsService _metricsService;
        private readonly ILogger<GetEmployeeDistributionQueryHandler> _logger;

        public GetEmployeeDistributionQueryHandler(IMetricsService metricsService, ILogger<GetEmployeeDistributionQueryHandler> logger)
        {
            _metricsService = metricsService;
            _logger = logger;
        }

        public async Task<List<EmployeeDistributionDto>> Handle(GetEmployeeDistributionQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetEmployeeDistributionQuery");
            return await _metricsService.GetEmployeeDistributionAsync(cancellationToken);
        }
    }

    /// <summary>
    /// Handler for GetDepartmentPayrollTrendQuery
    /// </summary>
    public class GetDepartmentPayrollTrendQueryHandler : IRequestHandler<GetDepartmentPayrollTrendQuery, DepartmentPayrollTrendDto>
    {
        private readonly IMetricsService _metricsService;
        private readonly ILogger<GetDepartmentPayrollTrendQueryHandler> _logger;

        public GetDepartmentPayrollTrendQueryHandler(IMetricsService metricsService, ILogger<GetDepartmentPayrollTrendQueryHandler> logger)
        {
            _metricsService = metricsService;
            _logger = logger;
        }

        public async Task<DepartmentPayrollTrendDto> Handle(GetDepartmentPayrollTrendQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetDepartmentPayrollTrendQuery for department {DepartmentId}", request.DepartmentId);
            return await _metricsService.GetDepartmentPayrollTrendAsync(request.DepartmentId, cancellationToken);
        }
    }

    /// <summary>
    /// Handler for GetDepartmentAnalyticsQuery
    /// </summary>
    public class GetDepartmentAnalyticsQueryHandler : IRequestHandler<GetDepartmentAnalyticsQuery, DepartmentAnalyticsDto>
    {
        private readonly IMetricsService _metricsService;
        private readonly ILogger<GetDepartmentAnalyticsQueryHandler> _logger;

        public GetDepartmentAnalyticsQueryHandler(IMetricsService metricsService, ILogger<GetDepartmentAnalyticsQueryHandler> logger)
        {
            _metricsService = metricsService;
            _logger = logger;
        }

        public async Task<DepartmentAnalyticsDto> Handle(GetDepartmentAnalyticsQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetDepartmentAnalyticsQuery for department {DepartmentId}", request.DepartmentId);
            return await _metricsService.GetDepartmentAnalyticsAsync(request.DepartmentId, cancellationToken);
        }
    }
}
