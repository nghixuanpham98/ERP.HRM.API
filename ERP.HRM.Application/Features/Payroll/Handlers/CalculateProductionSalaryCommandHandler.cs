using ERP.HRM.Application.DTOs.Payroll;
using ERP.HRM.Application.Features.Payroll.Commands;
using ERP.HRM.Application.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ERP.HRM.Application.Features.Payroll.Handlers
{
    /// <summary>
    /// Handler for calculating production-based salary
    /// </summary>
    public class CalculateProductionSalaryCommandHandler : IRequestHandler<CalculateProductionSalaryCommand, PayrollRecordDto>
    {
        private readonly IPayrollService _payrollService;
        private readonly ILogger<CalculateProductionSalaryCommandHandler> _logger;

        public CalculateProductionSalaryCommandHandler(IPayrollService payrollService, ILogger<CalculateProductionSalaryCommandHandler> logger)
        {
            _payrollService = payrollService ?? throw new ArgumentNullException(nameof(payrollService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<PayrollRecordDto> Handle(CalculateProductionSalaryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Handling CalculateProductionSalaryCommand for Employee: {EmployeeId}", request.EmployeeId);

                var result = await _payrollService.CalculateProductionSalaryAsync(
                    request.EmployeeId,
                    request.PayrollPeriodId,
                    request.OverrideUnitPrice,
                    request.OverrideAllowance);

                _logger.LogInformation("Production salary calculated successfully for Employee: {EmployeeId}", request.EmployeeId);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling CalculateProductionSalaryCommand");
                throw;
            }
        }
    }
}
