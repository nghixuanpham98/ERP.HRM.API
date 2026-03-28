using AutoMapper;
using ERP.HRM.Application.DTOs.Department;
using ERP.HRM.Application.Features.Departments.Queries;
using ERP.HRM.Domain.Exceptions;
using ERP.HRM.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ERP.HRM.Application.Features.Departments.Handlers
{
    public class GetDepartmentByIdQueryHandler : IRequestHandler<GetDepartmentByIdQuery, DepartmentDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<GetDepartmentByIdQueryHandler> _logger;

        public GetDepartmentByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetDepartmentByIdQueryHandler> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<DepartmentDto> Handle(GetDepartmentByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Fetching department with Id: {DepartmentId}", request.DepartmentId);

                var department = await _unitOfWork.DepartmentRepository.GetByIdAsync(request.DepartmentId);
                if (department == null)
                {
                    _logger.LogWarning("Department with Id {DepartmentId} not found", request.DepartmentId);
                    throw new NotFoundException($"Department with Id {request.DepartmentId} not found");
                }

                return _mapper.Map<DepartmentDto>(department);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching department");
                throw;
            }
        }
    }
}
