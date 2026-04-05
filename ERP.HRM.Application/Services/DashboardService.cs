using ERP.HRM.Application.DTOs.Dashboard;
using ERP.HRM.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Logging;
using System.Globalization;

namespace ERP.HRM.Application.Services
{
    /// <summary>
    /// Interface for dashboard service
    /// Provides aggregated data for payroll dashboards
    /// </summary>
    public interface IDashboardService
    {
        /// <summary>
        /// Get comprehensive dashboard summary
        /// </summary>
        Task<PayrollDashboardDto> GetPayrollDashboardAsync(int? year = null, int? month = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get dashboard summary cards only
        /// </summary>
        Task<DashboardSummaryDto> GetDashboardSummaryAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Get recent payroll information
        /// </summary>
        Task<RecentPayrollDto> GetRecentPayrollAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Get department-wise breakdown
        /// </summary>
        Task<List<DepartmentSummaryDto>> GetDepartmentBreakdownAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Get salary distribution analysis
        /// </summary>
        Task<List<SalaryRangeDistributionDto>> GetSalaryDistributionAsync(CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Service for generating dashboard data
    /// Aggregates information from multiple repositories
    /// </summary>
    public class DashboardService : IDashboardService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DashboardService> _logger;

        public DashboardService(IUnitOfWork unitOfWork, ILogger<DashboardService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        /// <summary>
        /// Get comprehensive dashboard summary
        /// </summary>
        public async Task<PayrollDashboardDto> GetPayrollDashboardAsync(int? year = null, int? month = null, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Getting comprehensive payroll dashboard");

            try
            {
                // Use current month if not specified
                var currentDate = DateTime.Now;
                year ??= currentDate.Year;
                month ??= currentDate.Month;

                var summary = await GetDashboardSummaryAsync(cancellationToken);
                var recentPayroll = await GetRecentPayrollAsync(cancellationToken);
                var departmentBreakdown = await GetDepartmentBreakdownAsync(cancellationToken);
                var leaveStatistics = await GetLeaveStatisticsAsync(cancellationToken);
                var salaryDistribution = await GetSalaryDistributionAsync(cancellationToken);

                var dashboard = new PayrollDashboardDto
                {
                    Summary = summary,
                    RecentPayroll = recentPayroll,
                    DepartmentBreakdown = departmentBreakdown,
                    LeaveStatistics = leaveStatistics,
                    SalaryDistribution = salaryDistribution,
                    GeneratedDate = DateTime.Now
                };

                _logger.LogInformation("Payroll dashboard retrieved successfully");
                return dashboard;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting payroll dashboard");
                throw;
            }
        }

        /// <summary>
        /// Get dashboard summary cards only
        /// </summary>
        public async Task<DashboardSummaryDto> GetDashboardSummaryAsync(CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Getting dashboard summary");

            try
            {
                var employees = await _unitOfWork.EmployeeRepository.GetAllAsync();
                var payrollRecords = await _unitOfWork.PayrollRecordRepository.GetAllAsync();
                var leaveRequests = await _unitOfWork.LeaveRequestRepository.GetAllAsync();

                var currentDate = DateTime.Now;
                var currentMonthPayroll = payrollRecords
                    .Where(p => p.PayrollPeriod.Year == currentDate.Year && p.PayrollPeriod.Month == currentDate.Month)
                    .ToList();

                var activeEmployees = employees.Where(e => e.Status == "Active").ToList();
                var pendingLeaves = leaveRequests.Where(l => l.ApprovalStatus == "Pending").Count();

                var summary = new DashboardSummaryDto
                {
                    TotalEmployees = employees.Count(),
                    ActiveEmployees = activeEmployees.Count,
                    AverageMonthlySalary = currentMonthPayroll.Count > 0 ? currentMonthPayroll.Average(p => p.NetSalary) : 0,
                    TotalMonthlyPayroll = currentMonthPayroll.Sum(p => p.GrossSalary),
                    AverageInsuranceCost = currentMonthPayroll.Count > 0 ? currentMonthPayroll.Average(p => p.InsuranceDeduction) : 0,
                    PendingLeaveRequests = pendingLeaves
                };

                _logger.LogInformation("Dashboard summary retrieved");
                return summary;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting dashboard summary");
                throw;
            }
        }

        /// <summary>
        /// Get recent payroll information
        /// </summary>
        public async Task<RecentPayrollDto> GetRecentPayrollAsync(CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Getting recent payroll information");

            try
            {
                var payrollRecords = await _unitOfWork.PayrollRecordRepository.GetAllAsync();
                var payrollPeriods = await _unitOfWork.PayrollPeriodRepository.GetAllAsync();

                // Get the most recent payroll period
                var recentPeriod = payrollPeriods
                    .OrderByDescending(p => p.Year)
                    .ThenByDescending(p => p.Month)
                    .FirstOrDefault();

                if (recentPeriod == null)
                {
                    _logger.LogWarning("No payroll period found");
                    return new RecentPayrollDto();
                }

                var periodRecords = payrollRecords
                    .Where(p => p.PayrollPeriodId == recentPeriod.PayrollPeriodId)
                    .ToList();

                var monthName = new DateTime(recentPeriod.Year, recentPeriod.Month, 1).ToString("MMMM yyyy", CultureInfo.InvariantCulture);

                var recentPayroll = new RecentPayrollDto
                {
                    PayrollPeriodId = recentPeriod.PayrollPeriodId,
                    Year = recentPeriod.Year,
                    Month = recentPeriod.Month,
                    PeriodName = monthName,
                    ProcessedCount = periodRecords.Count,
                    TotalGrossSalary = periodRecords.Sum(p => p.GrossSalary),
                    TotalNetSalary = periodRecords.Sum(p => p.NetSalary),
                    TotalTaxDeduction = periodRecords.Sum(p => p.TaxDeduction),
                    AverageNetSalary = periodRecords.Count > 0 ? periodRecords.Average(p => p.NetSalary) : 0,
                    ProcessedDate = DateTime.Now
                };

                _logger.LogInformation("Recent payroll retrieved for period {Year}-{Month}", recentPeriod.Year, recentPeriod.Month);
                return recentPayroll;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting recent payroll");
                throw;
            }
        }

        /// <summary>
        /// Get department-wise breakdown
        /// </summary>
        public async Task<List<DepartmentSummaryDto>> GetDepartmentBreakdownAsync(CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Getting department breakdown");

            try
            {
                var employees = await _unitOfWork.EmployeeRepository.GetAllAsync();
                var payrollRecords = await _unitOfWork.PayrollRecordRepository.GetAllAsync();
                var departments = await _unitOfWork.DepartmentRepository.GetAllAsync();

                var currentDate = DateTime.Now;
                var currentMonthPayroll = payrollRecords
                    .Where(p => p.PayrollPeriod.Year == currentDate.Year && p.PayrollPeriod.Month == currentDate.Month)
                    .ToList();

                var breakdown = departments.Select(dept =>
                {
                    var deptEmployees = employees.Where(e => e.DepartmentId == dept.DepartmentId).ToList();
                    var deptPayroll = currentMonthPayroll
                        .Where(p => deptEmployees.Any(e => e.EmployeeId == p.EmployeeId))
                        .ToList();

                    var activeEmployees = deptEmployees.Count(e => e.Status == "Active");

                    return new DepartmentSummaryDto
                    {
                        DepartmentId = dept.DepartmentId,
                        DepartmentName = dept.DepartmentName,
                        EmployeeCount = deptEmployees.Count,
                        TotalMonthlySalary = deptPayroll.Sum(p => p.GrossSalary),
                        AverageMonthlySalary = deptPayroll.Count > 0 ? deptPayroll.Average(p => p.NetSalary) : 0,
                        ActiveEmployees = activeEmployees,
                        InactiveEmployees = deptEmployees.Count - activeEmployees
                    };
                }).ToList();

                _logger.LogInformation("Department breakdown retrieved for {Count} departments", breakdown.Count);
                return breakdown;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting department breakdown");
                throw;
            }
        }

        /// <summary>
        /// Get leave statistics
        /// </summary>
        private async Task<LeaveStatisticsDto> GetLeaveStatisticsAsync(CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Getting leave statistics");

            try
            {
                var leaveRequests = await _unitOfWork.LeaveRequestRepository.GetAllAsync();

                var totalRequests = leaveRequests.Count();
                var approvedLeaves = leaveRequests.Count(l => l.ApprovalStatus == "Approved");
                var pendingLeaves = leaveRequests.Count(l => l.ApprovalStatus == "Pending");
                var rejectedLeaves = leaveRequests.Count(l => l.ApprovalStatus == "Rejected");

                var approvalRate = totalRequests > 0 ? (decimal)approvedLeaves / totalRequests * 100 : 0;

                var statistics = new LeaveStatisticsDto
                {
                    TotalLeaveRequests = totalRequests,
                    ApprovedLeaves = approvedLeaves,
                    PendingLeaves = pendingLeaves,
                    RejectedLeaves = rejectedLeaves,
                    ApprovalRate = approvalRate
                };

                _logger.LogInformation("Leave statistics retrieved");
                return statistics;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting leave statistics");
                throw;
            }
        }

        /// <summary>
        /// Get salary distribution analysis
        /// </summary>
        public async Task<List<SalaryRangeDistributionDto>> GetSalaryDistributionAsync(CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Getting salary distribution");

            try
            {
                var payrollRecords = await _unitOfWork.PayrollRecordRepository.GetAllAsync();

                var currentDate = DateTime.Now;
                var currentMonthPayroll = payrollRecords
                    .Where(p => p.PayrollPeriod.Year == currentDate.Year && p.PayrollPeriod.Month == currentDate.Month)
                    .ToList();

                if (currentMonthPayroll.Count == 0)
                {
                    _logger.LogWarning("No payroll records for current month");
                    return new List<SalaryRangeDistributionDto>();
                }

                // Define salary ranges
                var salaryRanges = new[]
                {
                    ("< 5M", 0m, 5000000m),
                    ("5M - 10M", 5000000m, 10000000m),
                    ("10M - 15M", 10000000m, 15000000m),
                    ("15M - 20M", 15000000m, 20000000m),
                    ("> 20M", 20000000m, decimal.MaxValue)
                };

                var totalEmployees = currentMonthPayroll.Count;
                var distribution = new List<SalaryRangeDistributionDto>();

                foreach (var (label, minSalary, maxSalary) in salaryRanges)
                {
                    var count = currentMonthPayroll.Count(p => p.NetSalary >= minSalary && p.NetSalary < maxSalary);
                    var percentage = totalEmployees > 0 ? (decimal)count / totalEmployees * 100 : 0;

                    distribution.Add(new SalaryRangeDistributionDto
                    {
                        SalaryRange = label,
                        EmployeeCount = count,
                        Percentage = percentage
                    });
                }

                _logger.LogInformation("Salary distribution retrieved");
                return distribution;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting salary distribution");
                throw;
            }
        }
    }
}
