using ERP.HRM.Application.DTOs.Reporting;
using ERP.HRM.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Logging;
using System.Globalization;

namespace ERP.HRM.Application.Services
{
    /// <summary>
    /// Interface for reporting and analytics service
    /// Provides business intelligence for payroll, tax, insurance, and HR metrics
    /// </summary>
    public interface IReportingService
    {
        /// <summary>
        /// Get monthly payroll summary for a specific month
        /// </summary>
        Task<MonthlyPayrollSummaryDto> GetMonthlyPayrollSummaryAsync(int year, int month, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get tax summary report for a specific month
        /// </summary>
        Task<TaxSummaryReportDto> GetTaxSummaryReportAsync(int year, int month, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get insurance summary report for a specific month
        /// </summary>
        Task<InsuranceSummaryReportDto> GetInsuranceSummaryReportAsync(int year, int month, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get leave summary report for a specific month
        /// </summary>
        Task<LeaveSummaryReportDto> GetLeaveSummaryReportAsync(int year, int month, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get comprehensive HR metrics for a year
        /// </summary>
        Task<HRMetricsDto> GetHRMetricsAsync(int year, CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Service for generating reports and analytics
    /// Supports payroll, tax, insurance, leave, and HR analytics
    /// </summary>
    public class ReportingService : IReportingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ReportingService> _logger;

        public ReportingService(IUnitOfWork unitOfWork, ILogger<ReportingService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        /// <summary>
        /// Get monthly payroll summary for a specific month
        /// </summary>
        public async Task<MonthlyPayrollSummaryDto> GetMonthlyPayrollSummaryAsync(int year, int month, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Getting monthly payroll summary for {Year}-{Month}", year, month);

            try
            {
                var payrollRecords = await _unitOfWork.PayrollRecordRepository.GetAllAsync();

                var filteredRecords = payrollRecords
                    .Where(p => p.PayrollPeriod.Year == year && p.PayrollPeriod.Month == month)
                    .ToList();

                var monthName = new DateTime(year, month, 1).ToString("MMMM", CultureInfo.InvariantCulture);

                var summary = new MonthlyPayrollSummaryDto
                {
                    Year = year,
                    Month = month,
                    MonthName = monthName,
                    TotalEmployees = filteredRecords.Count,
                    TotalGrossSalary = filteredRecords.Sum(p => p.GrossSalary),
                    TotalInsuranceDeduction = filteredRecords.Sum(p => p.InsuranceDeduction),
                    TotalTaxDeduction = filteredRecords.Sum(p => p.TaxDeduction),
                    TotalNetSalary = filteredRecords.Sum(p => p.NetSalary),
                    AverageSalary = filteredRecords.Count > 0 ? filteredRecords.Average(p => p.NetSalary) : 0
                };

                _logger.LogInformation("Monthly payroll summary retrieved for {Year}-{Month}", year, month);
                return summary;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting monthly payroll summary for {Year}-{Month}", year, month);
                throw;
            }
        }

        /// <summary>
        /// Get tax summary report for a specific month
        /// </summary>
        public async Task<TaxSummaryReportDto> GetTaxSummaryReportAsync(int year, int month, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Getting tax summary report for {Year}-{Month}", year, month);

            try
            {
                var payrollRecords = await _unitOfWork.PayrollRecordRepository.GetAllAsync();

                var filteredRecords = payrollRecords
                    .Where(p => p.PayrollPeriod.Year == year && p.PayrollPeriod.Month == month)
                    .ToList();

                var report = new TaxSummaryReportDto
                {
                    Year = year,
                    Month = month,
                    TotalEmployees = filteredRecords.Count,
                    TotalTaxableIncome = filteredRecords.Sum(p => p.GrossSalary - p.InsuranceDeduction),
                    TotalTaxDeducted = filteredRecords.Sum(p => p.TaxDeduction),
                    AverageTaxPerEmployee = filteredRecords.Count > 0 ? filteredRecords.Average(p => p.TaxDeduction) : 0
                };

                _logger.LogInformation("Tax summary report retrieved for {Year}-{Month}", year, month);
                return report;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting tax summary report for {Year}-{Month}", year, month);
                throw;
            }
        }

        /// <summary>
        /// Get insurance summary report for a specific month
        /// </summary>
        public async Task<InsuranceSummaryReportDto> GetInsuranceSummaryReportAsync(int year, int month, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Getting insurance summary report for {Year}-{Month}", year, month);

            try
            {
                var payrollRecords = await _unitOfWork.PayrollRecordRepository.GetAllAsync();

                var filteredRecords = payrollRecords
                    .Where(p => p.PayrollPeriod.Year == year && p.PayrollPeriod.Month == month)
                    .ToList();

                var report = new InsuranceSummaryReportDto
                {
                    Year = year,
                    Month = month,
                    TotalEmployees = filteredRecords.Count,
                    TotalInsuranceDeduction = filteredRecords.Sum(p => p.InsuranceDeduction),
                    AverageInsurancePerEmployee = filteredRecords.Count > 0 ? filteredRecords.Average(p => p.InsuranceDeduction) : 0
                };

                _logger.LogInformation("Insurance summary report retrieved for {Year}-{Month}", year, month);
                return report;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting insurance summary report for {Year}-{Month}", year, month);
                throw;
            }
        }

        /// <summary>
        /// Get leave summary report for a specific month
        /// </summary>
        public async Task<LeaveSummaryReportDto> GetLeaveSummaryReportAsync(int year, int month, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Getting leave summary report for {Year}-{Month}", year, month);

            try
            {
                var leaveRequests = await _unitOfWork.LeaveRequestRepository.GetAllAsync();

                var filteredRequests = leaveRequests
                    .Where(l => l.StartDate.Year == year && l.StartDate.Month == month)
                    .ToList();

                var report = new LeaveSummaryReportDto
                {
                    Year = year,
                    Month = month,
                    ApprovedLeaveRequests = filteredRequests.Count(l => l.ApprovalStatus == "Approved"),
                    PendingLeaveRequests = filteredRequests.Count(l => l.ApprovalStatus == "Pending"),
                    RejectedLeaveRequests = filteredRequests.Count(l => l.ApprovalStatus == "Rejected")
                };

                _logger.LogInformation("Leave summary report retrieved for {Year}-{Month}", year, month);
                return report;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting leave summary report for {Year}-{Month}", year, month);
                throw;
            }
        }

        /// <summary>
        /// Get comprehensive HR metrics for a year
        /// </summary>
        public async Task<HRMetricsDto> GetHRMetricsAsync(int year, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Getting HR metrics for {Year}", year);

            try
            {
                var employees = await _unitOfWork.EmployeeRepository.GetAllAsync();
                var payrollRecords = await _unitOfWork.PayrollRecordRepository.GetAllAsync();

                var activeEmployees = employees.Where(e => e.Status == "Active").ToList();
                var yearPayroll = payrollRecords.Where(p => p.PayrollPeriod.Year == year).ToList();

                var totalPayrollCost = yearPayroll.Sum(p => p.GrossSalary);
                var averageSalary = yearPayroll.Count > 0 ? yearPayroll.Average(p => p.NetSalary) : 0;

                var metrics = new HRMetricsDto
                {
                    Year = year,
                    TotalEmployees = employees.Count(),
                    ActiveEmployees = activeEmployees.Count,
                    InactiveEmployees = employees.Count() - activeEmployees.Count,
                    AverageSalary = averageSalary,
                    TotalPayrollCost = totalPayrollCost,
                    CostPerEmployee = activeEmployees.Count > 0 ? totalPayrollCost / activeEmployees.Count : 0
                };

                _logger.LogInformation("HR metrics retrieved for {Year}", year);
                return metrics;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting HR metrics for {Year}", year);
                throw;
            }
        }
    }
}
