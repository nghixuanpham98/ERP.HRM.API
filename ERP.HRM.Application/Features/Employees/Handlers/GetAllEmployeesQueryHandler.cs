using AutoMapper;
using ERP.HRM.Application.Common;
using ERP.HRM.Application.DTOs.Employee;
using ERP.HRM.Application.Features.Employees.Queries;
using ERP.HRM.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ERP.HRM.Application.Features.Employees.Handlers
{
    /// <summary>
    /// Handler for getting all employees with pagination and filtering
    /// </summary>
    public class GetAllEmployeesQueryHandler : IRequestHandler<GetAllEmployeesQuery, PagedResult<EmployeeDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<GetAllEmployeesQueryHandler> _logger;

        public GetAllEmployeesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetAllEmployeesQueryHandler> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<PagedResult<EmployeeDto>> Handle(GetAllEmployeesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Getting all employees. Page: {PageNumber}, Size: {PageSize}, Search: {SearchTerm}", 
                    request.PageNumber, request.PageSize, request.SearchTerm);

                var (employees, totalCount) = await _unitOfWork.EmployeeRepository.GetPagedAsync(
                    request.PageNumber,
                    request.PageSize);

                // Apply client-side filtering for search, department, and status
                var filteredEmployees = employees.AsEnumerable();

                if (!string.IsNullOrEmpty(request.SearchTerm))
                {
                    filteredEmployees = filteredEmployees.Where(e =>
                        e.FullName.Contains(request.SearchTerm) ||
                        e.EmployeeCode.Contains(request.SearchTerm) ||
                        (e.Email != null && e.Email.Contains(request.SearchTerm)));
                }

                if (request.DepartmentId.HasValue)
                {
                    filteredEmployees = filteredEmployees.Where(e => e.DepartmentId == request.DepartmentId);
                }

                if (!string.IsNullOrEmpty(request.Status))
                {
                    filteredEmployees = filteredEmployees.Where(e => e.Status == request.Status);
                }

                var filteredList = filteredEmployees.ToList();
                var filteredCount = filteredList.Count;

                return new PagedResult<EmployeeDto>
                {
                    Items = _mapper.Map<IEnumerable<EmployeeDto>>(filteredList),
                    TotalCount = filteredCount,
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting employees");
                throw;
            }
        }
    }
}
