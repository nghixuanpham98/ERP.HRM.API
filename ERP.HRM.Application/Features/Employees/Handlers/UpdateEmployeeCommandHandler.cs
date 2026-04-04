using AutoMapper;
using ERP.HRM.Application.DTOs.Employee;
using ERP.HRM.Application.Features.Employees.Commands;
using ERP.HRM.Application.Extensions;
using ERP.HRM.Domain.Exceptions;
using ERP.HRM.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ERP.HRM.Application.Features.Employees.Handlers
{
    /// <summary>
    /// Handler for updating an existing employee
    /// </summary>
    public class UpdateEmployeeCommandHandler : IRequestHandler<UpdateEmployeeCommand, EmployeeDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateEmployeeCommandHandler> _logger;

        public UpdateEmployeeCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<UpdateEmployeeCommandHandler> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<EmployeeDto> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Updating employee with ID: {EmployeeId}", request.EmployeeId);

                // Find the employee
                var employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(request.EmployeeId);
                if (employee == null)
                {
                    var ex = new NotFoundException("Employee", request.EmployeeId);
                    _logger.LogWarning("Employee not found with ID: {EmployeeId}", request.EmployeeId);
                    throw ex;
                }

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

                // Check if employee code is being changed and if new code already exists
                if (employee.EmployeeCode != request.EmployeeCode)
                {
                    var allEmployees = await _unitOfWork.EmployeeRepository.GetAllAsync();
                    if (allEmployees.Any(e => e.EmployeeCode == request.EmployeeCode && e.EmployeeId != request.EmployeeId))
                    {
                        var ex = new ConflictException($"Employee code '{request.EmployeeCode}' already exists");
                        _logger.LogWarning("Duplicate employee code during update: {EmployeeCode}", request.EmployeeCode);
                        throw ex;
                    }
                }

                // Map and update
                _mapper.Map(request, employee);
                employee.ModifiedDate = DateTime.UtcNow;

                await _unitOfWork.EmployeeRepository.UpdateAsync(employee);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Employee with ID: {EmployeeId} updated successfully", request.EmployeeId);

                return _mapper.Map<EmployeeDto>(employee);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating employee with ID: {EmployeeId}", request.EmployeeId);
                throw;
            }
        }
    }
}
