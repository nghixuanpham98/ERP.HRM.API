using ERP.HRM.Application.DTOs.Dashboard;
using ERP.HRM.Application.Features.Dashboard.Queries;
using ERP.HRM.Application.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ERP.HRM.Application.Features.Dashboard.Handlers
{
    /// <summary>
    /// Handler for GetPayrollDashboardQuery
    /// </summary>
    public class GetPayrollDashboardQueryHandler : IRequestHandler<GetPayrollDashboardQuery, PayrollDashboardDto>
    {
        private readonly IDashboardService _dashboardService;
        private readonly ILogger<GetPayrollDashboardQueryHandler> _logger;

        public GetPayrollDashboardQueryHandler(IDashboardService dashboardService, ILogger<GetPayrollDashboardQueryHandler> logger)
        {
            _dashboardService = dashboardService;
            _logger = logger;
        }

        public async Task<PayrollDashboardDto> Handle(GetPayrollDashboardQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetPayrollDashboardQuery");
            return await _dashboardService.GetPayrollDashboardAsync(request.Year, request.Month, cancellationToken);
        }
    }

    /// <summary>
    /// Handler for GetDashboardSummaryQuery
    /// </summary>
    public class GetDashboardSummaryQueryHandler : IRequestHandler<GetDashboardSummaryQuery, DashboardSummaryDto>
    {
        private readonly IDashboardService _dashboardService;
        private readonly ILogger<GetDashboardSummaryQueryHandler> _logger;

        public GetDashboardSummaryQueryHandler(IDashboardService dashboardService, ILogger<GetDashboardSummaryQueryHandler> logger)
        {
            _dashboardService = dashboardService;
            _logger = logger;
        }

        public async Task<DashboardSummaryDto> Handle(GetDashboardSummaryQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetDashboardSummaryQuery");
            return await _dashboardService.GetDashboardSummaryAsync(cancellationToken);
        }
    }

    /// <summary>
    /// Handler for GetRecentPayrollQuery
    /// </summary>
    public class GetRecentPayrollQueryHandler : IRequestHandler<GetRecentPayrollQuery, RecentPayrollDto>
    {
        private readonly IDashboardService _dashboardService;
        private readonly ILogger<GetRecentPayrollQueryHandler> _logger;

        public GetRecentPayrollQueryHandler(IDashboardService dashboardService, ILogger<GetRecentPayrollQueryHandler> logger)
        {
            _dashboardService = dashboardService;
            _logger = logger;
        }

        public async Task<RecentPayrollDto> Handle(GetRecentPayrollQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetRecentPayrollQuery");
            return await _dashboardService.GetRecentPayrollAsync(cancellationToken);
        }
    }

    /// <summary>
    /// Handler for GetDepartmentBreakdownQuery
    /// </summary>
    public class GetDepartmentBreakdownQueryHandler : IRequestHandler<GetDepartmentBreakdownQuery, List<DepartmentSummaryDto>>
    {
        private readonly IDashboardService _dashboardService;
        private readonly ILogger<GetDepartmentBreakdownQueryHandler> _logger;

        public GetDepartmentBreakdownQueryHandler(IDashboardService dashboardService, ILogger<GetDepartmentBreakdownQueryHandler> logger)
        {
            _dashboardService = dashboardService;
            _logger = logger;
        }

        public async Task<List<DepartmentSummaryDto>> Handle(GetDepartmentBreakdownQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetDepartmentBreakdownQuery");
            return await _dashboardService.GetDepartmentBreakdownAsync(cancellationToken);
        }
    }

    /// <summary>
    /// Handler for GetSalaryDistributionQuery
    /// </summary>
    public class GetSalaryDistributionQueryHandler : IRequestHandler<GetSalaryDistributionQuery, List<SalaryRangeDistributionDto>>
    {
        private readonly IDashboardService _dashboardService;
        private readonly ILogger<GetSalaryDistributionQueryHandler> _logger;

        public GetSalaryDistributionQueryHandler(IDashboardService dashboardService, ILogger<GetSalaryDistributionQueryHandler> logger)
        {
            _dashboardService = dashboardService;
            _logger = logger;
        }

        public async Task<List<SalaryRangeDistributionDto>> Handle(GetSalaryDistributionQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetSalaryDistributionQuery");
            return await _dashboardService.GetSalaryDistributionAsync(cancellationToken);
        }
    }
}
