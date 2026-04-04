using ERP.HRM.Application.DTOs.Payroll;
using ERP.HRM.Application.Features.Payroll.Commands;
using ERP.HRM.Application.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ERP.HRM.Application.Features.Payroll.Handlers
{
    /// <summary>
    /// Handler for calculating monthly salary
    /// </summary>
    public class CalculateMonthlySalaryCommandHandler : IRequestHandler<CalculateMonthlySalaryCommand, PayrollRecordDto>
    {
        private readonly IPayrollService _payrollService;
        private readonly ILogger<CalculateMonthlySalaryCommandHandler> _logger;

        public CalculateMonthlySalaryCommandHandler(IPayrollService payrollService, ILogger<CalculateMonthlySalaryCommandHandler> logger)
        {
            _payrollService = payrollService ?? throw new ArgumentNullException(nameof(payrollService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<PayrollRecordDto> Handle(CalculateMonthlySalaryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Handling CalculateMonthlySalaryCommand for Employee: {EmployeeId}", request.EmployeeId);

                var result = await _payrollService.CalculateMonthlySalaryAsync(
                    request.EmployeeId,
                    request.PayrollPeriodId,
                    request.OverrideBaseSalary,
                    request.OverrideAllowance);

                _logger.LogInformation("Monthly salary calculated successfully for Employee: {EmployeeId}", request.EmployeeId);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling CalculateMonthlySalaryCommand");
                throw;
            }
        }
    }
}
