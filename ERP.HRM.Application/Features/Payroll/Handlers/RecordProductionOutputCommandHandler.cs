using ERP.HRM.Application.DTOs.Payroll;
using ERP.HRM.Application.Features.Payroll.Commands;
using ERP.HRM.Domain.Entities;
using ERP.HRM.Domain.Exceptions;
using ERP.HRM.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ERP.HRM.Application.Features.Payroll.Handlers
{
    /// <summary>
    /// Handler for recording employee production output
    /// </summary>
    public class RecordProductionOutputCommandHandler : IRequestHandler<RecordProductionOutputCommand, ProductionOutputDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<RecordProductionOutputCommandHandler> _logger;

        public RecordProductionOutputCommandHandler(IUnitOfWork unitOfWork, ILogger<RecordProductionOutputCommandHandler> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ProductionOutputDto> Handle(RecordProductionOutputCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Handling RecordProductionOutputCommand for Employee: {EmployeeId}, Product: {ProductId}", 
                    request.EmployeeId, request.ProductId);

                // Validate employee exists
                var employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(request.EmployeeId);
                if (employee == null)
                    throw new NotFoundException("Employee", request.EmployeeId);

                // Validate payroll period exists
                var period = await _unitOfWork.PayrollPeriodRepository.GetByIdAsync(request.PayrollPeriodId);
                if (period == null)
                    throw new NotFoundException("Payroll Period", request.PayrollPeriodId);

                // Validate product exists
                var product = await _unitOfWork.ProductRepository.GetByIdAsync(request.ProductId);
                if (product == null)
                    throw new NotFoundException("Product", request.ProductId);

                // Create production output record
                var productionOutput = new ProductionOutput
                {
                    EmployeeId = request.EmployeeId,
                    PayrollPeriodId = request.PayrollPeriodId,
                    ProductId = request.ProductId,
                    Quantity = request.Quantity,
                    UnitPrice = request.UnitPrice,
                    ProductionDate = request.ProductionDate,
                    QualityStatus = request.QualityStatus ?? "OK",
                    Amount = request.Quantity * request.UnitPrice,
                    CreatedDate = DateTime.UtcNow,
                    ModifiedDate = DateTime.UtcNow
                };

                await _unitOfWork.ProductionOutputRepository.AddAsync(productionOutput);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Production output recorded successfully - Employee: {EmployeeId}, Quantity: {Quantity}, Amount: {Amount}",
                    request.EmployeeId, request.Quantity, productionOutput.Amount);

                var dto = new ProductionOutputDto
                {
                    ProductionOutputId = productionOutput.ProductionOutputId,
                    EmployeeId = productionOutput.EmployeeId,
                    PayrollPeriodId = productionOutput.PayrollPeriodId,
                    ProductId = productionOutput.ProductId,
                    Quantity = productionOutput.Quantity,
                    UnitPrice = productionOutput.UnitPrice,
                    ProductionDate = productionOutput.ProductionDate,
                    QualityStatus = productionOutput.QualityStatus,
                    Amount = productionOutput.Amount
                };

                return dto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling RecordProductionOutputCommand");
                throw;
            }
        }
    }
}
