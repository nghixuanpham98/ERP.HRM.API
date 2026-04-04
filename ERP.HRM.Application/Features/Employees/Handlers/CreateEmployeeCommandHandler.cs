using AutoMapper;
using ERP.HRM.Application.DTOs.Employee;
using ERP.HRM.Application.Features.Employees.Commands;
using ERP.HRM.Application.Extensions;
using ERP.HRM.Domain.Entities;
using ERP.HRM.Domain.Exceptions;
using ERP.HRM.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ERP.HRM.Application.Features.Employees.Handlers
{
    /// <summary>
    /// Handler for creating a new employee
    /// </summary>
    public class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand, EmployeeDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateEmployeeCommandHandler> _logger;

        public CreateEmployeeCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<CreateEmployeeCommandHandler> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<EmployeeDto> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Creating employee with code: {EmployeeCode}, Name: {FullName}", 
                    request.EmployeeCode, request.FullName);

                // Validate email if provided
                if (!request.Email.IsNullOrEmpty() && !request.Email.IsValidEmail())
                {
                    var ex = new ValidationException(nameof(request.Email), "Invalid email format");
                    _logger.LogWarning("Email validation failed for: {Email}", request.Email);
                    throw ex;
                }

                // Validate phone if provided
                if (!request.PhoneNumber.IsNullOrEmpty() && !request.PhoneNumber.IsValidPhoneNumber())
                {
                    var ex = new ValidationException(nameof(request.PhoneNumber), "Invalid phone number format");
                    _logger.LogWarning("Phone validation failed for: {Phone}", request.PhoneNumber);
                    throw ex;
                }

                // Validate date of birth if provided
                if (request.DateOfBirth.HasValue && !DataValidationExtensions.IsValidAge(request.DateOfBirth.Value))
                {
                    var ex = new ValidationException(nameof(request.DateOfBirth), "Employee age must be between 18 and 65");
                    _logger.LogWarning("Age validation failed for DOB: {DOB}", request.DateOfBirth);
                    throw ex;
                }

                // Validate salary if provided
                if (request.BaseSalary.HasValue && !DataValidationExtensions.IsValidSalary(request.BaseSalary.Value))
                {
                    var ex = new ValidationException(nameof(request.BaseSalary), "Invalid salary amount");
                    _logger.LogWarning("Salary validation failed for: {Salary}", request.BaseSalary);
                    throw ex;
                }

                // Check if employee code already exists
                var employeeRepo = _unitOfWork.EmployeeRepository;
                var existingEmployee = await employeeRepo.GetAllAsync();
                if (existingEmployee.Any(e => e.EmployeeCode == request.EmployeeCode))
                {
                    var ex = new ConflictException($"Employee code '{request.EmployeeCode}' already exists");
                    _logger.LogWarning("Duplicate employee code: {EmployeeCode}", request.EmployeeCode);
                    throw ex;
                }

                var employee = _mapper.Map<Employee>(request);
                employee.Status ??= "Active";
                employee.CreatedDate = DateTime.UtcNow;

                await _unitOfWork.EmployeeRepository.AddAsync(employee);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Employee '{FullName}' ({EmployeeCode}) created successfully with ID: {EmployeeId}", 
                    request.FullName, request.EmployeeCode, employee.EmployeeId);

                return _mapper.Map<EmployeeDto>(employee);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating employee: {EmployeeCode}", request.EmployeeCode);
                throw;
            }
        }
    }
}
