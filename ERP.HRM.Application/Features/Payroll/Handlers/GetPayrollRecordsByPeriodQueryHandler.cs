using ERP.HRM.Application.Common;
using ERP.HRM.Application.DTOs.Payroll;
using ERP.HRM.Application.Features.Payroll.Queries;
using ERP.HRM.Domain.Exceptions;
using ERP.HRM.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ERP.HRM.Application.Features.Payroll.Handlers
{
    /// <summary>
    /// Handler for getting payroll records by period
    /// </summary>
    public class GetPayrollRecordsByPeriodQueryHandler : IRequestHandler<GetPayrollRecordsByPeriodQuery, PagedResult<PayrollRecordDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GetPayrollRecordsByPeriodQueryHandler> _logger;

        public GetPayrollRecordsByPeriodQueryHandler(IUnitOfWork unitOfWork, ILogger<GetPayrollRecordsByPeriodQueryHandler> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<PagedResult<PayrollRecordDto>> Handle(GetPayrollRecordsByPeriodQuery request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Handling GetPayrollRecordsByPeriodQuery for Period: {PayrollPeriodId}", request.PayrollPeriodId);

                // Validate payroll period exists
                var period = await _unitOfWork.PayrollPeriodRepository.GetByIdAsync(request.PayrollPeriodId);
                if (period == null)
                    throw new NotFoundException("Payroll Period", request.PayrollPeriodId);

                // Get payroll records for the period
                var payrollRecords = await _unitOfWork.PayrollRecordRepository.GetByPeriodAsync(request.PayrollPeriodId);

                _logger.LogInformation("Retrieved {Count} payroll records for Period: {PayrollPeriodId}", 
                    payrollRecords.Count(), request.PayrollPeriodId);

                // Map to DTOs
                var dtos = payrollRecords.Select(pr => new PayrollRecordDto
                {
                    PayrollRecordId = pr.PayrollRecordId,
                    EmployeeId = pr.EmployeeId,
                    PayrollPeriodId = pr.PayrollPeriodId,
                    SalaryType = pr.SalaryType.ToString(),
                    BaseSalary = pr.BaseSalary,
                    Allowance = pr.Allowance,
                    OvertimeCompensation = pr.OvertimeCompensation,
                    GrossSalary = pr.GrossSalary,
                    InsuranceDeduction = pr.InsuranceDeduction,
                    TaxDeduction = pr.TaxDeduction,
                    OtherDeductions = pr.OtherDeductions,
                    TotalDeductions = pr.TotalDeductions,
                    NetSalary = pr.NetSalary,
                    WorkingDays = pr.WorkingDays,
                    ProductionTotal = pr.ProductionTotal,
                    Status = pr.Status,
                    PaymentDate = pr.PaymentDate
                }).ToList();

                return new PagedResult<PayrollRecordDto>
                {
                    Items = dtos,
                    TotalCount = dtos.Count,
                    PageNumber = 1,
                    PageSize = dtos.Count
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling GetPayrollRecordsByPeriodQuery");
                throw;
            }
        }
    }
}
