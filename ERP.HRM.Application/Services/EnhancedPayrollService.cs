using ERP.HRM.Application.DTOs.Payroll;
using ERP.HRM.Domain.Entities;
using ERP.HRM.Domain.Exceptions;
using ERP.HRM.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Logging;

namespace ERP.HRM.Application.Services
{
    /// <summary>
    /// Enhanced Payroll Service with HR integration
    /// Supports tiered insurance rates, progressive tax brackets, and dependent deductions
    /// </summary>
    public interface IEnhancedPayrollService
    {
        /// <summary>
        /// Calculate monthly salary with HR enhancements:
        /// - Tiered insurance (BHXH) based on salary bands
        /// - Progressive tax brackets (TNCN) 
        /// - Dependent deductions for tax
        /// </summary>
        Task<PayrollRecordDto> CalculateEnhancedMonthlySalaryAsync(
            int employeeId,
            int payrollPeriodId,
            decimal? overrideBaseSalary = null,
            decimal? overrideAllowance = null);

        /// <summary>
        /// Calculate tiered insurance deduction based on salary
        /// </summary>
        Task<decimal> CalculateTieredInsuranceAsync(decimal grossSalary, string insuranceType = "Health");

        /// <summary>
        /// Calculate progressive tax with brackets and dependent deductions
        /// </summary>
        Task<decimal> CalculateProgressiveTaxAsync(decimal grossSalary, int employeeId);

        /// <summary>
        /// Get dependent deduction amount
        /// </summary>
        Task<decimal> GetDependentDeductionAsync(int employeeId);
    }

    public class EnhancedPayrollService : IEnhancedPayrollService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IInsuranceTierRepository _insuranceTierRepository;
        private readonly ITaxBracketRepository _taxBracketRepository;
        private readonly IFamilyDependentRepository _familyDependentRepository;
        private readonly ILogger<EnhancedPayrollService> _logger;

        // Vietnamese tax configuration
        private const decimal NON_TAXABLE_THRESHOLD = 11_000_000m; // 11M VND non-taxable threshold
        private const decimal DEPENDENT_DEDUCTION_ANNUAL = 9_600_000m; // 9.6M per dependent per year
        private const decimal DEPENDENT_DEDUCTION_MONTHLY = DEPENDENT_DEDUCTION_ANNUAL / 12; // Monthly equivalent

        public EnhancedPayrollService(
            IUnitOfWork unitOfWork,
            IInsuranceTierRepository insuranceTierRepository,
            ITaxBracketRepository taxBracketRepository,
            IFamilyDependentRepository familyDependentRepository,
            ILogger<EnhancedPayrollService> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _insuranceTierRepository = insuranceTierRepository ?? throw new ArgumentNullException(nameof(insuranceTierRepository));
            _taxBracketRepository = taxBracketRepository ?? throw new ArgumentNullException(nameof(taxBracketRepository));
            _familyDependentRepository = familyDependentRepository ?? throw new ArgumentNullException(nameof(familyDependentRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<PayrollRecordDto> CalculateEnhancedMonthlySalaryAsync(
            int employeeId,
            int payrollPeriodId,
            decimal? overrideBaseSalary = null,
            decimal? overrideAllowance = null)
        {
            try
            {
                _logger.LogInformation("Calculating enhanced monthly salary for Employee: {EmployeeId}, Period: {PeriodId}",
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

                // Calculate base salary (use latest salary adjustment if available)
                decimal baseSalary = overrideBaseSalary ?? employee.BaseSalary ?? salaryConfig.BaseSalary;
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

                // Enhanced calculation: Tiered Insurance
                var tieredInsurance = await CalculateTieredInsuranceAsync(grossSalary, "Health");

                // Enhanced calculation: Progressive Tax with dependents
                var taxableIncome = grossSalary - tieredInsurance - NON_TAXABLE_THRESHOLD;
                var dependentDeduction = await GetDependentDeductionAsync(employeeId);
                var taxableAfterDependents = Math.Max(0, taxableIncome - dependentDeduction);
                var progressiveTax = await CalculateProgressiveTaxAsync(taxableAfterDependents, employeeId);

                // Other deductions (loans, fines, etc.)
                var otherDeductions = 0m;

                var totalDeductions = tieredInsurance + progressiveTax + otherDeductions;
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
                    InsuranceDeduction = tieredInsurance,
                    TaxDeduction = progressiveTax,
                    OtherDeductions = otherDeductions,
                    TotalDeductions = totalDeductions,
                    NetSalary = netSalary,
                    WorkingDays = actualWorkingDays,
                    Status = "Calculated",
                    CreatedDate = DateTime.UtcNow
                };

                await _unitOfWork.PayrollRecordRepository.AddAsync(payrollRecord);

                // Log detailed deductions
                var insuranceDeduction = new PayrollDeduction
                {
                    PayrollRecordId = payrollRecord.PayrollRecordId,
                    DeductionType = "BHXH",
                    Description = "Bảo hiểm xã hội (tiered)",
                    Amount = tieredInsurance,
                    Reason = $"Tiered insurance for salary {grossSalary:C0}"
                };

                var taxDeduction = new PayrollDeduction
                {
                    PayrollRecordId = payrollRecord.PayrollRecordId,
                    DeductionType = "Thuế",
                    Description = "Thuế thu nhập cá nhân (progressive)",
                    Amount = progressiveTax,
                    Reason = $"Progressive tax with {Math.Round(dependentDeduction / DEPENDENT_DEDUCTION_MONTHLY)} dependents"
                };

                // Note: PayrollDeduction would need to be added via a PayrollDeductionRepository
                // For now, we just track them in the PayrollRecord deduction fields

                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Enhanced monthly salary calculated successfully. NetSalary: {NetSalary}, Insurance: {Insurance}, Tax: {Tax}",
                    netSalary, tieredInsurance, progressiveTax);

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
                    InsuranceDeduction = tieredInsurance,
                    TaxDeduction = progressiveTax,
                    OtherDeductions = otherDeductions,
                    TotalDeductions = totalDeductions,
                    NetSalary = netSalary
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating enhanced monthly salary");
                throw;
            }
        }

        public async Task<decimal> CalculateTieredInsuranceAsync(decimal grossSalary, string insuranceType = "Health")
        {
            try
            {
                var tier = await _insuranceTierRepository.GetTierForSalaryAsync(grossSalary, insuranceType, DateTime.Now);
                if (tier == null)
                {
                    _logger.LogWarning("No insurance tier found for salary {Salary} and type {Type}. Using default 8%", grossSalary, insuranceType);
                    return grossSalary * 0.08m; // Fallback to 8%
                }

                var insuranceAmount = grossSalary * tier.EmployeeRate / 100;
                _logger.LogInformation("Calculated tiered insurance: {Amount} (Rate: {Rate}%) for salary {Salary}",
                    insuranceAmount, tier.EmployeeRate, grossSalary);

                return insuranceAmount;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating tiered insurance");
                throw;
            }
        }

        public async Task<decimal> CalculateProgressiveTaxAsync(decimal grossSalary, int employeeId)
        {
            try
            {
                if (grossSalary <= 0)
                    return 0;

                var bracket = await _taxBracketRepository.GetBracketForIncomeAsync(grossSalary, DateTime.Now);
                if (bracket == null)
                {
                    _logger.LogWarning("No tax bracket found for income {Income}. Using default 5%", grossSalary);
                    return grossSalary * 0.05m; // Fallback to 5%
                }

                var taxAmount = grossSalary * bracket.TaxRate / 100;
                _logger.LogInformation("Calculated progressive tax: {Amount} (Rate: {Rate}%) for income {Income}",
                    taxAmount, bracket.TaxRate, grossSalary);

                return taxAmount;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating progressive tax");
                throw;
            }
        }

        public async Task<decimal> GetDependentDeductionAsync(int employeeId)
        {
            try
            {
                var dependents = await _familyDependentRepository.GetQualifiedDependentsByEmployeeIdAsync(employeeId, DateTime.Now);
                var dependentCount = dependents.Count();

                var totalDeduction = dependentCount * DEPENDENT_DEDUCTION_MONTHLY;
                _logger.LogInformation("Calculated dependent deduction for employee {EmployeeId}: {Count} dependents = {Amount:C0}",
                    employeeId, dependentCount, totalDeduction);

                return totalDeduction;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating dependent deduction");
                return 0;
            }
        }
    }
}
