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
    /// Handler for getting employee by ID
    /// </summary>
    public class GetEmployeeByIdQueryHandler : IRequestHandler<GetEmployeeByIdQuery, EmployeeDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<GetEmployeeByIdQueryHandler> _logger;

        public GetEmployeeByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetEmployeeByIdQueryHandler> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<EmployeeDto> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Getting employee with ID: {EmployeeId}", request.EmployeeId);

                var employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(request.EmployeeId);
                if (employee == null)
                {
                    var ex = new Domain.Exceptions.NotFoundException("Employee", request.EmployeeId);
                    _logger.LogWarning("Employee not found with ID: {EmployeeId}", request.EmployeeId);
                    throw ex;
                }

                return _mapper.Map<EmployeeDto>(employee);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting employee with ID: {EmployeeId}", request.EmployeeId);
                throw;
            }
        }
    }
}
