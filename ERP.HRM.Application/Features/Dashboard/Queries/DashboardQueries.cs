using ERP.HRM.Application.DTOs.Dashboard;
using MediatR;

namespace ERP.HRM.Application.Features.Dashboard.Queries
{
    /// <summary>
    /// Query to get comprehensive payroll dashboard
    /// </summary>
    public class GetPayrollDashboardQuery : IRequest<PayrollDashboardDto>
    {
        public int? Year { get; set; }
        public int? Month { get; set; }
    }

    /// <summary>
    /// Query to get dashboard summary cards
    /// </summary>
    public class GetDashboardSummaryQuery : IRequest<DashboardSummaryDto>
    {
    }

    /// <summary>
    /// Query to get recent payroll information
    /// </summary>
    public class GetRecentPayrollQuery : IRequest<RecentPayrollDto>
    {
    }

    /// <summary>
    /// Query to get department-wise breakdown
    /// </summary>
    public class GetDepartmentBreakdownQuery : IRequest<List<DepartmentSummaryDto>>
    {
    }

    /// <summary>
    /// Query to get salary distribution analysis
    /// </summary>
    public class GetSalaryDistributionQuery : IRequest<List<SalaryRangeDistributionDto>>
    {
    }
}
