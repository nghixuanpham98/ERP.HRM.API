namespace ERP.HRM.Application.DTOs.Reporting
{
    /// <summary>
    /// Monthly Payroll Summary DTO
    /// Provides aggregated payroll metrics for a specific month
    /// </summary>
    public class MonthlyPayrollSummaryDto
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public string MonthName { get; set; }
        public int TotalEmployees { get; set; }
        public decimal TotalGrossSalary { get; set; }
        public decimal TotalInsuranceDeduction { get; set; }
        public decimal TotalTaxDeduction { get; set; }
        public decimal TotalNetSalary { get; set; }
        public decimal AverageSalary { get; set; }
    }

    /// <summary>
    /// Tax Summary Report DTO
    /// Shows tax-related metrics and distribution
    /// </summary>
    public class TaxSummaryReportDto
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int TotalEmployees { get; set; }
        public decimal TotalTaxableIncome { get; set; }
        public decimal TotalTaxDeducted { get; set; }
        public decimal AverageTaxPerEmployee { get; set; }
    }

    /// <summary>
    /// Insurance Summary Report DTO
    /// Shows insurance costs breakdown
    /// </summary>
    public class InsuranceSummaryReportDto
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int TotalEmployees { get; set; }
        public decimal TotalInsuranceDeduction { get; set; }
        public decimal AverageInsurancePerEmployee { get; set; }
    }

    /// <summary>
    /// Leave Summary Report DTO
    /// Shows leave usage trends
    /// </summary>
    public class LeaveSummaryReportDto
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int ApprovedLeaveRequests { get; set; }
        public int PendingLeaveRequests { get; set; }
        public int RejectedLeaveRequests { get; set; }
    }

    /// <summary>
    /// HR Metrics DTO
    /// Comprehensive HR statistics
    /// </summary>
    public class HRMetricsDto
    {
        public int Year { get; set; }
        public int TotalEmployees { get; set; }
        public int ActiveEmployees { get; set; }
        public int InactiveEmployees { get; set; }
        public decimal AverageSalary { get; set; }
        public decimal TotalPayrollCost { get; set; }
        public decimal CostPerEmployee { get; set; }
    }
}
