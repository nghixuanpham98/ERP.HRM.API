using ERP.HRM.Application.Features.Employees.Commands;
using ERP.HRM.Domain.Exceptions;
using ERP.HRM.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ERP.HRM.Application.Features.Employees.Handlers
{
    /// <summary>
    /// Handler for deleting (soft delete) an employee
    /// </summary>
    public class DeleteEmployeeCommandHandler : IRequestHandler<DeleteEmployeeCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DeleteEmployeeCommandHandler> _logger;

        public DeleteEmployeeCommandHandler(IUnitOfWork unitOfWork, ILogger<DeleteEmployeeCommandHandler> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<bool> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Deleting employee with ID: {EmployeeId}", request.EmployeeId);

                // Find the employee
                var employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(request.EmployeeId);
                if (employee == null)
                {
                    var ex = new NotFoundException("Employee", request.EmployeeId);
                    _logger.LogWarning("Employee not found with ID: {EmployeeId}", request.EmployeeId);
                    throw ex;
                }

                // Soft delete
                employee.IsDeleted = true;
                employee.ModifiedDate = DateTime.UtcNow;
                await _unitOfWork.EmployeeRepository.UpdateAsync(employee);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Employee with ID: {EmployeeId} deleted successfully", request.EmployeeId);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting employee with ID: {EmployeeId}", request.EmployeeId);
                throw;
            }
        }
    }
}
