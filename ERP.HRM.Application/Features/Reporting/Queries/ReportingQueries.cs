using ERP.HRM.Application.DTOs.Reporting;
using MediatR;

namespace ERP.HRM.Application.Features.Reporting.Queries
{
    /// <summary>
    /// Query to get monthly payroll summary
    /// </summary>
    public class GetMonthlyPayrollSummaryQuery : IRequest<MonthlyPayrollSummaryDto>
    {
        public int Year { get; set; }
        public int Month { get; set; }
    }

    /// <summary>
    /// Query to get tax summary report
    /// </summary>
    public class GetTaxSummaryReportQuery : IRequest<TaxSummaryReportDto>
    {
        public int Year { get; set; }
        public int Month { get; set; }
    }

    /// <summary>
    /// Query to get insurance summary report
    /// </summary>
    public class GetInsuranceSummaryReportQuery : IRequest<InsuranceSummaryReportDto>
    {
        public int Year { get; set; }
        public int Month { get; set; }
    }

    /// <summary>
    /// Query to get leave summary report
    /// </summary>
    public class GetLeaveSummaryReportQuery : IRequest<LeaveSummaryReportDto>
    {
        public int Year { get; set; }
        public int Month { get; set; }
    }

    /// <summary>
    /// Query to get HR metrics
    /// </summary>
    public class GetHRMetricsQuery : IRequest<HRMetricsDto>
    {
        public int Year { get; set; }
    }
}
