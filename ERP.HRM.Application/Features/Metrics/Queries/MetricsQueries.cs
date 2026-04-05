using ERP.HRM.Application.DTOs.Metrics;
using MediatR;

namespace ERP.HRM.Application.Features.Metrics.Queries
{
    /// <summary>
    /// Query to get department metrics
    /// </summary>
    public class GetDepartmentMetricsQuery : IRequest<DepartmentMetricsDto>
    {
        public int DepartmentId { get; set; }
    }

    /// <summary>
    /// Query to compare departments
    /// </summary>
    public class GetDepartmentComparisonQuery : IRequest<DepartmentComparisonDto>
    {
    }

    /// <summary>
    /// Query to get employee distribution
    /// </summary>
    public class GetEmployeeDistributionQuery : IRequest<List<EmployeeDistributionDto>>
    {
    }

    /// <summary>
    /// Query to get payroll trends
    /// </summary>
    public class GetDepartmentPayrollTrendQuery : IRequest<DepartmentPayrollTrendDto>
    {
        public int DepartmentId { get; set; }
    }

    /// <summary>
    /// Query to get comprehensive department analytics
    /// </summary>
    public class GetDepartmentAnalyticsQuery : IRequest<DepartmentAnalyticsDto>
    {
        public int DepartmentId { get; set; }
    }
}
