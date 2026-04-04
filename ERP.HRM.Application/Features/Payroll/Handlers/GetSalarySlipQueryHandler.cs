using ERP.HRM.Application.DTOs.Payroll;
using ERP.HRM.Application.Features.Payroll.Queries;
using ERP.HRM.Application.Services;
using ERP.HRM.Domain.Exceptions;
using ERP.HRM.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ERP.HRM.Application.Features.Payroll.Handlers
{
    /// <summary>
    /// Handler for getting salary slip (formatted payroll record)
    /// </summary>
    public class GetSalarySlipQueryHandler : IRequestHandler<GetSalarySlipQuery, SalarySlipDto>
    {
        private readonly IPayrollService _payrollService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GetSalarySlipQueryHandler> _logger;

        public GetSalarySlipQueryHandler(IPayrollService payrollService, IUnitOfWork unitOfWork, ILogger<GetSalarySlipQueryHandler> logger)
        {
            _payrollService = payrollService ?? throw new ArgumentNullException(nameof(payrollService));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<SalarySlipDto> Handle(GetSalarySlipQuery request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Handling GetSalarySlipQuery for Employee: {EmployeeId}, Period: {PayrollPeriodId}", 
                    request.EmployeeId, request.PayrollPeriodId);

                // Validate employee exists
                var employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(request.EmployeeId);
                if (employee == null)
                    throw new NotFoundException("Employee", request.EmployeeId);

                // Validate payroll period exists
                var period = await _unitOfWork.PayrollPeriodRepository.GetByIdAsync(request.PayrollPeriodId);
                if (period == null)
                    throw new NotFoundException("Payroll Period", request.PayrollPeriodId);

                // Get salary slip from service
                var salarySlip = await _payrollService.GetSalarySlipAsync(request.EmployeeId, request.PayrollPeriodId);

                _logger.LogInformation("Salary slip retrieved successfully for Employee: {EmployeeId}, Period: {PayrollPeriodId}",
                    request.EmployeeId, request.PayrollPeriodId);

                return salarySlip;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling GetSalarySlipQuery");
                throw;
            }
        }
    }
}
