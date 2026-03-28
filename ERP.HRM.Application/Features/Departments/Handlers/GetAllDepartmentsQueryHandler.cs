using AutoMapper;
using ERP.HRM.Application.Common;
using ERP.HRM.Application.DTOs.Department;
using ERP.HRM.Application.Features.Departments.Queries;
using ERP.HRM.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ERP.HRM.Application.Features.Departments.Handlers
{
    public class GetAllDepartmentsQueryHandler : IRequestHandler<GetAllDepartmentsQuery, PagedResult<DepartmentDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<GetAllDepartmentsQueryHandler> _logger;

        public GetAllDepartmentsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetAllDepartmentsQueryHandler> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PagedResult<DepartmentDto>> Handle(GetAllDepartmentsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Fetching all departments. Page: {PageNumber}, Size: {PageSize}", request.PageNumber, request.PageSize);

                var (departments, totalCount) = await _unitOfWork.DepartmentRepository.GetPagedAsync(request.PageNumber, request.PageSize);

                return new PagedResult<DepartmentDto>
                {
                    Items = _mapper.Map<IEnumerable<DepartmentDto>>(departments),
                    TotalCount = totalCount,
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching departments");
                throw;
            }
        }
    }
}
