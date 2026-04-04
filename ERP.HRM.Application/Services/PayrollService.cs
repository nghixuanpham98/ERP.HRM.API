using ERP.HRM.Application.DTOs.Payroll;
using ERP.HRM.Domain.Entities;
using ERP.HRM.Domain.Exceptions;
using ERP.HRM.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Logging;

namespace ERP.HRM.Application.Services
{
    /// <summary>
    /// Service for payroll calculation and management
    /// </summary>
    public interface IPayrollService
    {
        /// <summary>
        /// Calculate monthly salary based on working days
        /// Formula: (BaseSalary / TotalWorkingDaysInPeriod) * ActualWorkingDays
        /// </summary>
        Task<PayrollRecordDto> CalculateMonthlySalaryAsync(
            int employeeId, 
            int payrollPeriodId,
            decimal? overrideBaseSalary = null,
            decimal? overrideAllowance = null);

        /// <summary>
        /// Calculate production-based salary
        /// Formula: Sum of (UnitPrice * Quantity) for all production outputs
        /// </summary>
        Task<PayrollRecordDto> CalculateProductionSalaryAsync(
            int employeeId,
            int payrollPeriodId,
            decimal? overrideUnitPrice = null,
            decimal? overrideAllowance = null);

        /// <summary>
        /// Get salary slip for an employee in a period
        /// </summary>
        Task<SalarySlipDto> GetSalarySlipAsync(int employeeId, int payrollPeriodId);

        /// <summary>
        /// Calculate deductions (insurance, tax, etc.)
        /// </summary>
        Task<(decimal Insurance, decimal Tax, decimal Other)> CalculateDeductionsAsync(
            decimal grossSalary,
            decimal? insuranceRate = null,
            decimal? taxRate = null);
    }

    public class PayrollService : IPayrollService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<PayrollService> _logger;

        public PayrollService(IUnitOfWork unitOfWork, ILogger<PayrollService> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<PayrollRecordDto> CalculateMonthlySalaryAsync(
            int employeeId,
            int payrollPeriodId,
            decimal? overrideBaseSalary = null,
            decimal? overrideAllowance = null)
        {
            try
            {
                _logger.LogInformation("Calculating monthly salary for Employee: {EmployeeId}, Period: {PeriodId}", 
                    employeeId, payrollPeriodId);

                // Get employee
                var employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(employeeId);
                if (employee == null)
                    throw new NotFoundException("Employee", employeeId);

                // Get payroll period
                var period = await _unitOfWork.PayrollPeriodRepository.GetByIdAsync(payrollPeriodId);
                if (period == null)
                    throw new NotFoundException("PayrollPeriod", payrollPeriodId);

                // Get salary configuration
                var salaryConfig = await _unitOfWork.SalaryConfigurationRepository.GetActiveConfigByEmployeeIdAsync(employeeId);
                if (salaryConfig == null)
                    throw new NotFoundException("SalaryConfiguration", employeeId);

                // Get working days for employee
                var actualWorkingDays = await _unitOfWork.AttendanceRepository.GetTotalWorkingDaysAsync(employeeId, payrollPeriodId);

                // Calculate base salary
                var baseSalary = overrideBaseSalary ?? salaryConfig.BaseSalary;
                var dailySalary = baseSalary / period.TotalWorkingDays;
                var calculatedBaseSalary = dailySalary * actualWorkingDays;

                // Calculate allowance and overtime
                var allowance = overrideAllowance ?? (salaryConfig.Allowance ?? 0);

                // Get overtime compensation
                var attendanceRecords = await _unitOfWork.AttendanceRepository.GetByEmployeeAndPeriodAsync(employeeId, payrollPeriodId);
                var totalOvertimeHours = attendanceRecords.Sum(a => a.OvertimeHours ?? 0);
                var overtimeCompensation = (dailySalary / 8) * totalOvertimeHours * (attendanceRecords.FirstOrDefault()?.OvertimeMultiplier ?? 1.5m);

                // Calculate gross salary
                var grossSalary = calculatedBaseSalary + allowance + overtimeCompensation;

                // Calculate deductions
                var (insurance, tax, other) = await CalculateDeductionsAsync(
                    grossSalary,
                    salaryConfig.InsuranceRate,
                    salaryConfig.TaxRate);

                var totalDeductions = insurance + tax + other;
                var netSalary = grossSalary - totalDeductions;

                // Create payroll record
                var payrollRecord = new PayrollRecord
                {
                    EmployeeId = employeeId,
                    PayrollPeriodId = payrollPeriodId,
                    SalaryType = SalaryType.Monthly,
                    BaseSalary = calculatedBaseSalary,
                    Allowance = allowance,
                    OvertimeCompensation = overtimeCompensation,
                    GrossSalary = grossSalary,
                    InsuranceDeduction = insurance,
                    TaxDeduction = tax,
                    OtherDeductions = other,
                    TotalDeductions = totalDeductions,
                    NetSalary = netSalary,
                    WorkingDays = actualWorkingDays,
                    Status = "Calculated",
                    CreatedDate = DateTime.UtcNow
                };

                await _unitOfWork.PayrollRecordRepository.AddAsync(payrollRecord);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Monthly salary calculated successfully. NetSalary: {NetSalary}", netSalary);

                return new PayrollRecordDto
                {
                    PayrollRecordId = payrollRecord.PayrollRecordId,
                    EmployeeId = employeeId,
                    EmployeeName = employee.FullName,
                    PayrollPeriodId = payrollPeriodId,
                    SalaryType = "Monthly",
                    BaseSalary = calculatedBaseSalary,
                    Allowance = allowance,
                    OvertimeCompensation = overtimeCompensation,
                    GrossSalary = grossSalary,
                    InsuranceDeduction = insurance,
                    TaxDeduction = tax,
                    OtherDeductions = other,
                    TotalDeductions = totalDeductions,
                    NetSalary = netSalary,
                    WorkingDays = actualWorkingDays,
                    Status = "Calculated"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating monthly salary for Employee: {EmployeeId}", employeeId);
                throw;
            }
        }

        public async Task<PayrollRecordDto> CalculateProductionSalaryAsync(
            int employeeId,
            int payrollPeriodId,
            decimal? overrideUnitPrice = null,
            decimal? overrideAllowance = null)
        {
            try
            {
                _logger.LogInformation("Calculating production salary for Employee: {EmployeeId}, Period: {PeriodId}", 
                    employeeId, payrollPeriodId);

                // Get employee
                var employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(employeeId);
                if (employee == null)
                    throw new NotFoundException("Employee", employeeId);

                // Get payroll period
                var period = await _unitOfWork.PayrollPeriodRepository.GetByIdAsync(payrollPeriodId);
                if (period == null)
                    throw new NotFoundException("PayrollPeriod", payrollPeriodId);

                // Get salary configuration
                var salaryConfig = await _unitOfWork.SalaryConfigurationRepository.GetActiveConfigByEmployeeIdAsync(employeeId);
                if (salaryConfig == null)
                    throw new NotFoundException("SalaryConfiguration", employeeId);

                // Get production output
                var productionTotal = await _unitOfWork.ProductionOutputRepository.GetTotalProductionAmountAsync(employeeId, payrollPeriodId);

                // Calculate salary (Quantity * UnitPrice)
                var calculatedBaseSalary = productionTotal;
                var allowance = overrideAllowance ?? (salaryConfig.Allowance ?? 0);

                // Calculate gross salary
                var grossSalary = calculatedBaseSalary + allowance;

                // Calculate deductions
                var (insurance, tax, other) = await CalculateDeductionsAsync(
                    grossSalary,
                    salaryConfig.InsuranceRate,
                    salaryConfig.TaxRate);

                var totalDeductions = insurance + tax + other;
                var netSalary = grossSalary - totalDeductions;

                // Create payroll record
                var payrollRecord = new PayrollRecord
                {
                    EmployeeId = employeeId,
                    PayrollPeriodId = payrollPeriodId,
                    SalaryType = SalaryType.Production,
                    BaseSalary = calculatedBaseSalary,
                    Allowance = allowance,
                    OvertimeCompensation = 0,
                    GrossSalary = grossSalary,
                    InsuranceDeduction = insurance,
                    TaxDeduction = tax,
                    OtherDeductions = other,
                    TotalDeductions = totalDeductions,
                    NetSalary = netSalary,
                    ProductionTotal = productionTotal,
                    Status = "Calculated",
                    CreatedDate = DateTime.UtcNow
                };

                await _unitOfWork.PayrollRecordRepository.AddAsync(payrollRecord);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Production salary calculated successfully. NetSalary: {NetSalary}", netSalary);

                return new PayrollRecordDto
                {
                    PayrollRecordId = payrollRecord.PayrollRecordId,
                    EmployeeId = employeeId,
                    EmployeeName = employee.FullName,
                    PayrollPeriodId = payrollPeriodId,
                    SalaryType = "Production",
                    BaseSalary = calculatedBaseSalary,
                    Allowance = allowance,
                    OvertimeCompensation = 0,
                    GrossSalary = grossSalary,
                    InsuranceDeduction = insurance,
                    TaxDeduction = tax,
                    OtherDeductions = other,
                    TotalDeductions = totalDeductions,
                    NetSalary = netSalary,
                    ProductionTotal = productionTotal,
                    Status = "Calculated"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating production salary for Employee: {EmployeeId}", employeeId);
                throw;
            }
        }

        public async Task<SalarySlipDto> GetSalarySlipAsync(int employeeId, int payrollPeriodId)
        {
            try
            {
                _logger.LogInformation("Getting salary slip for Employee: {EmployeeId}, Period: {PeriodId}", 
                    employeeId, payrollPeriodId);

                // Get payroll record
                var payrollRecord = await _unitOfWork.PayrollRecordRepository.GetByEmployeeAndPeriodAsync(employeeId, payrollPeriodId);
                if (payrollRecord == null)
                    throw new NotFoundException("PayrollRecord", $"Employee: {employeeId}, Period: {payrollPeriodId}");

                // Get employee details
                var employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(employeeId);
                
                // Get period details
                var period = await _unitOfWork.PayrollPeriodRepository.GetByIdAsync(payrollPeriodId);

                return new SalarySlipDto
                {
                    PayrollRecordId = payrollRecord.PayrollRecordId,
                    EmployeeCode = employee?.EmployeeCode ?? "",
                    EmployeeName = employee?.FullName ?? "",
                    DepartmentName = "HR", // Would need to join with Department
                    PositionName = "Staff", // Would need to join with Position
                    Period = period?.PeriodName ?? "",
                    BaseSalary = payrollRecord.BaseSalary,
                    Allowance = payrollRecord.Allowance,
                    OvertimeCompensation = payrollRecord.OvertimeCompensation,
                    GrossSalary = payrollRecord.GrossSalary,
                    InsuranceDeduction = payrollRecord.InsuranceDeduction,
                    TaxDeduction = payrollRecord.TaxDeduction,
                    OtherDeductions = payrollRecord.OtherDeductions,
                    TotalDeductions = payrollRecord.TotalDeductions,
                    NetSalary = payrollRecord.NetSalary,
                    CreatedDate = payrollRecord.CreatedDate ?? DateTime.UtcNow
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting salary slip for Employee: {EmployeeId}", employeeId);
                throw;
            }
        }

        public async Task<(decimal Insurance, decimal Tax, decimal Other)> CalculateDeductionsAsync(
            decimal grossSalary,
            decimal? insuranceRate = null,
            decimal? taxRate = null)
        {
            var insurance = insuranceRate.HasValue ? (grossSalary * insuranceRate.Value / 100) : 0;
            var tax = taxRate.HasValue ? (grossSalary * taxRate.Value / 100) : 0;
            var other = 0m;

            return await Task.FromResult((insurance, tax, other));
        }
    }
}
