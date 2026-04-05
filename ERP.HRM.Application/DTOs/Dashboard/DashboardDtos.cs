namespace ERP.HRM.Application.DTOs.Dashboard
{
    /// <summary>
    /// Dashboard summary card DTO
    /// Shows key metrics at a glance
    /// </summary>
    public class DashboardSummaryDto
    {
        public int TotalEmployees { get; set; }
        public int ActiveEmployees { get; set; }
        public decimal AverageMonthlySalary { get; set; }
        public decimal TotalMonthlyPayroll { get; set; }
        public decimal AverageInsuranceCost { get; set; }
        public int PendingLeaveRequests { get; set; }
    }

    /// <summary>
    /// Recent payroll summary DTO
    /// Shows the latest payroll period data
    /// </summary>
    public class RecentPayrollDto
    {
        public int PayrollPeriodId { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public string PeriodName { get; set; }
        public int ProcessedCount { get; set; }
        public decimal TotalGrossSalary { get; set; }
        public decimal TotalNetSalary { get; set; }
        public decimal TotalTaxDeduction { get; set; }
        public decimal AverageNetSalary { get; set; }
        public DateTime ProcessedDate { get; set; }
    }

    /// <summary>
    /// Department summary for dashboard DTO
    /// Shows metrics by department
    /// </summary>
    public class DepartmentSummaryDto
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public int EmployeeCount { get; set; }
        public decimal TotalMonthlySalary { get; set; }
        public decimal AverageMonthlySalary { get; set; }
        public int ActiveEmployees { get; set; }
        public int InactiveEmployees { get; set; }
    }

    /// <summary>
    /// Leave statistics for dashboard DTO
    /// Shows leave usage trends
    /// </summary>
    public class LeaveStatisticsDto
    {
        public int TotalLeaveRequests { get; set; }
        public int ApprovedLeaves { get; set; }
        public int PendingLeaves { get; set; }
        public int RejectedLeaves { get; set; }
        public decimal ApprovalRate { get; set; }
    }

    /// <summary>
    /// Salary range distribution DTO
    /// Shows employees grouped by salary ranges
    /// </summary>
    public class SalaryRangeDistributionDto
    {
        public string SalaryRange { get; set; }
        public int EmployeeCount { get; set; }
        public decimal Percentage { get; set; }
    }

    /// <summary>
    /// Complete dashboard data DTO
    /// Aggregates all dashboard information
    /// </summary>
    public class PayrollDashboardDto
    {
        public DashboardSummaryDto Summary { get; set; }
        public RecentPayrollDto RecentPayroll { get; set; }
        public List<DepartmentSummaryDto> DepartmentBreakdown { get; set; }
        public LeaveStatisticsDto LeaveStatistics { get; set; }
        public List<SalaryRangeDistributionDto> SalaryDistribution { get; set; }
        public DateTime GeneratedDate { get; set; }
    }
}
