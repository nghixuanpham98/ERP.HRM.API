using ERP.HRM.Application.Features.Departments.Commands;
using ERP.HRM.Domain.Exceptions;
using ERP.HRM.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ERP.HRM.Application.Features.Departments.Handlers
{
    public class DeleteDepartmentCommandHandler : IRequestHandler<DeleteDepartmentCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DeleteDepartmentCommandHandler> _logger;

        public DeleteDepartmentCommandHandler(IUnitOfWork unitOfWork, ILogger<DeleteDepartmentCommandHandler> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteDepartmentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Deleting department with Id: {DepartmentId}", request.DepartmentId);

                var department = await _unitOfWork.DepartmentRepository.GetByIdAsync(request.DepartmentId);
                if (department == null)
                {
                    _logger.LogWarning("Department with Id {DepartmentId} not found", request.DepartmentId);
                    throw new NotFoundException($"Department with Id {request.DepartmentId} not found");
                }

                await _unitOfWork.DepartmentRepository.SoftDeleteAsync(request.DepartmentId);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Department with Id {DepartmentId} deleted successfully", request.DepartmentId);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting department");
                throw;
            }
        }
    }
}
