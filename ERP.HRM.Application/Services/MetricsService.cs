using ERP.HRM.Application.DTOs.Metrics;
using ERP.HRM.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Logging;

namespace ERP.HRM.Application.Services
{
    /// <summary>
    /// Interface for metrics service
    /// Provides department-level analytics and metrics
    /// </summary>
    public interface IMetricsService
    {
        /// <summary>
        /// Get metrics for a specific department
        /// </summary>
        Task<DepartmentMetricsDto> GetDepartmentMetricsAsync(int departmentId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Compare metrics across all departments
        /// </summary>
        Task<DepartmentComparisonDto> GetDepartmentComparisonAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Get employee distribution across departments
        /// </summary>
        Task<List<EmployeeDistributionDto>> GetEmployeeDistributionAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Get payroll trends for a department
        /// </summary>
        Task<DepartmentPayrollTrendDto> GetDepartmentPayrollTrendAsync(int departmentId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get comprehensive analytics for a department
        /// </summary>
        Task<DepartmentAnalyticsDto> GetDepartmentAnalyticsAsync(int departmentId, CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Service for department metrics and analytics
    /// </summary>
    public class MetricsService : IMetricsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<MetricsService> _logger;

        public MetricsService(IUnitOfWork unitOfWork, ILogger<MetricsService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        /// <summary>
        /// Get metrics for a specific department
        /// </summary>
        public async Task<DepartmentMetricsDto> GetDepartmentMetricsAsync(int departmentId, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Getting metrics for department {DepartmentId}", departmentId);

            try
            {
                var employees = await _unitOfWork.EmployeeRepository.GetAllAsync();
                var payrollRecords = await _unitOfWork.PayrollRecordRepository.GetAllAsync();
                var department = await _unitOfWork.DepartmentRepository.GetByIdAsync(departmentId);

                var deptEmployees = employees.Where(e => e.DepartmentId == departmentId).ToList();
                var activeEmployees = deptEmployees.Count(e => e.Status == "Active");

                var currentDate = DateTime.Now;
                var deptPayroll = payrollRecords
                    .Where(p => deptEmployees.Any(e => e.EmployeeId == p.EmployeeId) &&
                                p.PayrollPeriod.Year == currentDate.Year &&
                                p.PayrollPeriod.Month == currentDate.Month)
                    .ToList();

                var metrics = new DepartmentMetricsDto
                {
                    DepartmentId = departmentId,
                    DepartmentName = department?.DepartmentName ?? "Unknown",
                    TotalEmployees = deptEmployees.Count,
                    ActiveEmployees = activeEmployees,
                    InactiveEmployees = deptEmployees.Count - activeEmployees,
                    AverageSalary = deptPayroll.Count > 0 ? deptPayroll.Average(p => p.NetSalary) : 0,
                    TotalMonthlyCost = deptPayroll.Sum(p => p.GrossSalary),
                    CostPerEmployee = activeEmployees > 0 ? deptPayroll.Sum(p => p.GrossSalary) / activeEmployees : 0,
                    AverageInsuranceCost = deptPayroll.Count > 0 ? deptPayroll.Average(p => p.InsuranceDeduction) : 0
                };

                _logger.LogInformation("Metrics retrieved for department {DepartmentId}", departmentId);
                return metrics;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting metrics for department {DepartmentId}", departmentId);
                throw;
            }
        }

        /// <summary>
        /// Compare metrics across all departments
        /// </summary>
        public async Task<DepartmentComparisonDto> GetDepartmentComparisonAsync(CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Getting department comparison");

            try
            {
                var departments = await _unitOfWork.DepartmentRepository.GetAllAsync();
                var departmentMetrics = new List<DepartmentMetricsDto>();

                foreach (var dept in departments)
                {
                    var metrics = await GetDepartmentMetricsAsync(dept.DepartmentId, cancellationToken);
                    departmentMetrics.Add(metrics);
                }

                var employees = await _unitOfWork.EmployeeRepository.GetAllAsync();
                var payrollRecords = await _unitOfWork.PayrollRecordRepository.GetAllAsync();

                var currentDate = DateTime.Now;
                var currentMonthPayroll = payrollRecords
                    .Where(p => p.PayrollPeriod.Year == currentDate.Year && p.PayrollPeriod.Month == currentDate.Month)
                    .ToList();

                var companyAverageSalary = currentMonthPayroll.Count > 0 ? currentMonthPayroll.Average(p => p.NetSalary) : 0;

                var comparison = new DepartmentComparisonDto
                {
                    Departments = departmentMetrics,
                    BestPerformingDepartment = departmentMetrics.OrderByDescending(d => d.AverageSalary).FirstOrDefault(),
                    HighestCostDepartment = departmentMetrics.OrderByDescending(d => d.TotalMonthlyCost).FirstOrDefault(),
                    CompanyAverageSalary = companyAverageSalary
                };

                _logger.LogInformation("Department comparison retrieved");
                return comparison;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting department comparison");
                throw;
            }
        }

        /// <summary>
        /// Get employee distribution across departments
        /// </summary>
        public async Task<List<EmployeeDistributionDto>> GetEmployeeDistributionAsync(CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Getting employee distribution");

            try
            {
                var employees = await _unitOfWork.EmployeeRepository.GetAllAsync();
                var departments = await _unitOfWork.DepartmentRepository.GetAllAsync();

                var totalEmployees = employees.Count();
                var distribution = departments.Select(dept =>
                {
                    var count = employees.Count(e => e.DepartmentId == dept.DepartmentId);
                    var percentage = totalEmployees > 0 ? (decimal)count / totalEmployees * 100 : 0;

                    return new EmployeeDistributionDto
                    {
                        DepartmentId = dept.DepartmentId,
                        DepartmentName = dept.DepartmentName,
                        EmployeeCount = count,
                        Percentage = percentage
                    };
                }).ToList();

                _logger.LogInformation("Employee distribution retrieved");
                return distribution;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting employee distribution");
                throw;
            }
        }

        /// <summary>
        /// Get payroll trends for a department
        /// </summary>
        public async Task<DepartmentPayrollTrendDto> GetDepartmentPayrollTrendAsync(int departmentId, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Getting payroll trend for department {DepartmentId}", departmentId);

            try
            {
                var employees = await _unitOfWork.EmployeeRepository.GetAllAsync();
                var payrollRecords = await _unitOfWork.PayrollRecordRepository.GetAllAsync();
                var payrollPeriods = await _unitOfWork.PayrollPeriodRepository.GetAllAsync();

                var deptEmployees = employees.Where(e => e.DepartmentId == departmentId).ToList();

                // Get the most recent payroll period
                var recentPeriod = payrollPeriods
                    .OrderByDescending(p => p.Year)
                    .ThenByDescending(p => p.Month)
                    .FirstOrDefault();

                if (recentPeriod == null)
                {
                    _logger.LogWarning("No payroll period found for department {DepartmentId}", departmentId);
                    return new DepartmentPayrollTrendDto();
                }

                var periodRecords = payrollRecords
                    .Where(p => p.PayrollPeriodId == recentPeriod.PayrollPeriodId &&
                                deptEmployees.Any(e => e.EmployeeId == p.EmployeeId))
                    .ToList();

                var trend = new DepartmentPayrollTrendDto
                {
                    DepartmentId = departmentId,
                    DepartmentName = employees.FirstOrDefault(e => e.DepartmentId == departmentId)?.Department?.DepartmentName ?? "Unknown",
                    Year = recentPeriod.Year,
                    Month = recentPeriod.Month,
                    TotalPayroll = periodRecords.Sum(p => p.GrossSalary),
                    AveragePayroll = periodRecords.Count > 0 ? periodRecords.Average(p => p.NetSalary) : 0,
                    ProcessedEmployees = periodRecords.Count
                };

                _logger.LogInformation("Payroll trend retrieved for department {DepartmentId}", departmentId);
                return trend;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting payroll trend for department {DepartmentId}", departmentId);
                throw;
            }
        }

        /// <summary>
        /// Get comprehensive analytics for a department
        /// </summary>
        public async Task<DepartmentAnalyticsDto> GetDepartmentAnalyticsAsync(int departmentId, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Getting comprehensive analytics for department {DepartmentId}", departmentId);

            try
            {
                var employees = await _unitOfWork.EmployeeRepository.GetAllAsync();
                var payrollRecords = await _unitOfWork.PayrollRecordRepository.GetAllAsync();
                var leaveRequests = await _unitOfWork.LeaveRequestRepository.GetAllAsync();

                var deptEmployees = employees.Where(e => e.DepartmentId == departmentId).ToList();

                var metrics = await GetDepartmentMetricsAsync(departmentId, cancellationToken);
                var payrollTrend = await GetDepartmentPayrollTrendAsync(departmentId, cancellationToken);
                var employeeDistribution = await GetEmployeeDistributionAsync(cancellationToken);

                // Leave Analysis
                var deptLeaveRequests = leaveRequests.Where(l => deptEmployees.Any(e => e.EmployeeId == l.EmployeeId)).ToList();
                var totalLeaves = deptLeaveRequests.Count;
                var approvedLeaves = deptLeaveRequests.Count(l => l.ApprovalStatus == "Approved");
                var pendingLeaves = deptLeaveRequests.Count(l => l.ApprovalStatus == "Pending");
                var rejectedLeaves = deptLeaveRequests.Count(l => l.ApprovalStatus == "Rejected");

                var leaveAnalysis = new DepartmentLeaveAnalysisDto
                {
                    DepartmentId = departmentId,
                    DepartmentName = metrics.DepartmentName,
                    TotalLeaveRequests = totalLeaves,
                    ApprovedLeaves = approvedLeaves,
                    PendingLeaves = pendingLeaves,
                    RejectedLeaves = rejectedLeaves,
                    ApprovalRate = totalLeaves > 0 ? (decimal)approvedLeaves / totalLeaves * 100 : 0,
                    LeavePerEmployee = deptEmployees.Count > 0 ? (decimal)totalLeaves / deptEmployees.Count : 0
                };

                // Cost Breakdown
                var currentDate = DateTime.Now;
                var deptPayroll = payrollRecords
                    .Where(p => deptEmployees.Any(e => e.EmployeeId == p.EmployeeId) &&
                                p.PayrollPeriod.Year == currentDate.Year &&
                                p.PayrollPeriod.Month == currentDate.Month)
                    .ToList();

                var totalSalaryCost = deptPayroll.Sum(p => p.GrossSalary);
                var totalInsuranceCost = deptPayroll.Sum(p => p.InsuranceDeduction);
                var totalCost = totalSalaryCost + totalInsuranceCost;

                var costBreakdown = new DepartmentCostBreakdownDto
                {
                    DepartmentId = departmentId,
                    DepartmentName = metrics.DepartmentName,
                    SalaryCost = totalSalaryCost,
                    InsuranceCost = totalInsuranceCost,
                    TotalCost = totalCost,
                    SalaryCostPercentage = totalCost > 0 ? totalSalaryCost / totalCost * 100 : 0,
                    InsuranceCostPercentage = totalCost > 0 ? totalInsuranceCost / totalCost * 100 : 0
                };

                var analytics = new DepartmentAnalyticsDto
                {
                    Metrics = metrics,
                    RecentPayrollTrend = payrollTrend,
                    LeaveAnalysis = leaveAnalysis,
                    CostBreakdown = costBreakdown,
                    EmployeeComparison = employeeDistribution,
                    GeneratedDate = DateTime.Now
                };

                _logger.LogInformation("Comprehensive analytics retrieved for department {DepartmentId}", departmentId);
                return analytics;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting comprehensive analytics for department {DepartmentId}", departmentId);
                throw;
            }
        }
    }
}
