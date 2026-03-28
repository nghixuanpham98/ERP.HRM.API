using AutoMapper;
using ERP.HRM.Application.DTOs.Department;
using ERP.HRM.Application.Features.Departments.Commands;
using ERP.HRM.Domain.Entities;
using ERP.HRM.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ERP.HRM.Application.Features.Departments.Handlers
{
    public class CreateDepartmentCommandHandler : IRequestHandler<CreateDepartmentCommand, DepartmentDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateDepartmentCommandHandler> _logger;

        public CreateDepartmentCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<CreateDepartmentCommandHandler> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<DepartmentDto> Handle(CreateDepartmentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Creating department with name: {DepartmentName}", request.DepartmentName);

                var department = _mapper.Map<Department>(request);
                await _unitOfWork.DepartmentRepository.AddAsync(department);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Department '{DepartmentName}' created successfully", request.DepartmentName);

                return _mapper.Map<DepartmentDto>(department);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating department");
                throw;
            }
        }
    }
}
