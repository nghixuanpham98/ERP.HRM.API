namespace ERP.HRM.Application.DTOs.Metrics
{
    /// <summary>
    /// Department performance metrics DTO
    /// Shows KPIs for a specific department
    /// </summary>
    public class DepartmentMetricsDto
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public int TotalEmployees { get; set; }
        public int ActiveEmployees { get; set; }
        public int InactiveEmployees { get; set; }
        public decimal AverageSalary { get; set; }
        public decimal TotalMonthlyCost { get; set; }
        public decimal CostPerEmployee { get; set; }
        public decimal AverageInsuranceCost { get; set; }
    }

    /// <summary>
    /// Department comparison metrics DTO
    /// Compares multiple departments
    /// </summary>
    public class DepartmentComparisonDto
    {
        public List<DepartmentMetricsDto> Departments { get; set; }
        public DepartmentMetricsDto BestPerformingDepartment { get; set; }
        public DepartmentMetricsDto HighestCostDepartment { get; set; }
        public decimal CompanyAverageSalary { get; set; }
    }

    /// <summary>
    /// Employee metrics by department DTO
    /// Shows employee distribution and statistics
    /// </summary>
    public class EmployeeDistributionDto
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public int EmployeeCount { get; set; }
        public decimal Percentage { get; set; }
    }

    /// <summary>
    /// Department payroll trends DTO
    /// Tracks payroll changes over time
    /// </summary>
    public class DepartmentPayrollTrendDto
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public decimal TotalPayroll { get; set; }
        public decimal AveragePayroll { get; set; }
        public int ProcessedEmployees { get; set; }
    }

    /// <summary>
    /// Department leave analysis DTO
    /// Shows leave usage by department
    /// </summary>
    public class DepartmentLeaveAnalysisDto
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public int TotalLeaveRequests { get; set; }
        public int ApprovedLeaves { get; set; }
        public int PendingLeaves { get; set; }
        public int RejectedLeaves { get; set; }
        public decimal ApprovalRate { get; set; }
        public decimal LeavePerEmployee { get; set; }
    }

    /// <summary>
    /// Department cost breakdown DTO
    /// Shows salary vs insurance vs other costs
    /// </summary>
    public class DepartmentCostBreakdownDto
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public decimal SalaryCost { get; set; }
        public decimal InsuranceCost { get; set; }
        public decimal TotalCost { get; set; }
        public decimal SalaryCostPercentage { get; set; }
        public decimal InsuranceCostPercentage { get; set; }
    }

    /// <summary>
    /// Comprehensive department analytics DTO
    /// Aggregates all department metrics
    /// </summary>
    public class DepartmentAnalyticsDto
    {
        public DepartmentMetricsDto Metrics { get; set; }
        public DepartmentPayrollTrendDto RecentPayrollTrend { get; set; }
        public DepartmentLeaveAnalysisDto LeaveAnalysis { get; set; }
        public DepartmentCostBreakdownDto CostBreakdown { get; set; }
        public List<EmployeeDistributionDto> EmployeeComparison { get; set; }
        public DateTime GeneratedDate { get; set; }
    }
}
