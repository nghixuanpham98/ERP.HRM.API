using AutoMapper;
using ERP.HRM.Application.DTOs.Department;
using ERP.HRM.Application.Features.Departments.Commands;
using ERP.HRM.Domain.Exceptions;
using ERP.HRM.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ERP.HRM.Application.Features.Departments.Handlers
{
    public class UpdateDepartmentCommandHandler : IRequestHandler<UpdateDepartmentCommand, DepartmentDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateDepartmentCommandHandler> _logger;

        public UpdateDepartmentCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<UpdateDepartmentCommandHandler> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<DepartmentDto> Handle(UpdateDepartmentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Updating department with Id: {DepartmentId}", request.DepartmentId);

                var department = await _unitOfWork.DepartmentRepository.GetByIdAsync(request.DepartmentId);
                if (department == null)
                {
                    _logger.LogWarning("Department with Id {DepartmentId} not found", request.DepartmentId);
                    throw new NotFoundException($"Department with Id {request.DepartmentId} not found");
                }

                _mapper.Map(request, department);
                await _unitOfWork.DepartmentRepository.UpdateAsync(department);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Department with Id {DepartmentId} updated successfully", request.DepartmentId);

                return _mapper.Map<DepartmentDto>(department);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating department");
                throw;
            }
        }
    }
}
