using ERP.HRM.Application.DTOs.Payroll;
using ERP.HRM.Application.Features.Payroll.Queries;
using ERP.HRM.Domain.Exceptions;
using ERP.HRM.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ERP.HRM.Application.Features.Payroll.Handlers
{
    /// <summary>
    /// Handler for getting production output records by employee and period
    /// </summary>
    public class GetProductionOutputByEmployeeAndPeriodQueryHandler : IRequestHandler<GetProductionOutputByEmployeeAndPeriodQuery, IEnumerable<ProductionOutputDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GetProductionOutputByEmployeeAndPeriodQueryHandler> _logger;

        public GetProductionOutputByEmployeeAndPeriodQueryHandler(IUnitOfWork unitOfWork, ILogger<GetProductionOutputByEmployeeAndPeriodQueryHandler> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<ProductionOutputDto>> Handle(GetProductionOutputByEmployeeAndPeriodQuery request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Handling GetProductionOutputByEmployeeAndPeriodQuery for Employee: {EmployeeId}, Period: {PayrollPeriodId}",
                    request.EmployeeId, request.PayrollPeriodId);

                // Validate employee exists
                var employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(request.EmployeeId);
                if (employee == null)
                    throw new NotFoundException("Employee", request.EmployeeId);

                // Validate payroll period exists
                var period = await _unitOfWork.PayrollPeriodRepository.GetByIdAsync(request.PayrollPeriodId);
                if (period == null)
                    throw new NotFoundException("Payroll Period", request.PayrollPeriodId);

                // Get production output records
                var productionOutputs = await _unitOfWork.ProductionOutputRepository.GetByEmployeeAndPeriodAsync(
                    request.EmployeeId, request.PayrollPeriodId);

                _logger.LogInformation("Retrieved {Count} production output records for Employee: {EmployeeId}, Period: {PayrollPeriodId}",
                    productionOutputs.Count(), request.EmployeeId, request.PayrollPeriodId);

                // Map to DTOs
                var dtos = productionOutputs.Select(po => new ProductionOutputDto
                {
                    ProductionOutputId = po.ProductionOutputId,
                    EmployeeId = po.EmployeeId,
                    PayrollPeriodId = po.PayrollPeriodId,
                    ProductId = po.ProductId,
                    Quantity = po.Quantity,
                    UnitPrice = po.UnitPrice,
                    ProductionDate = po.ProductionDate,
                    QualityStatus = po.QualityStatus,
                    Amount = po.Amount
                }).OrderBy(po => po.ProductionDate).ToList();

                return dtos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling GetProductionOutputByEmployeeAndPeriodQuery");
                throw;
            }
        }
    }
}
