using AutoMapper;
using ERP.HRM.Application.Common;
using ERP.HRM.Application.DTOs.Employee;
using ERP.HRM.Application.Interfaces;
using ERP.HRM.Domain.Entities;
using ERP.HRM.Domain.Exceptions;
using ERP.HRM.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Logging;

namespace ERP.HRM.Application.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<EmployeeService> _logger;

        public EmployeeService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<EmployeeService> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PagedResult<EmployeeDto>> GetAllEmployeesAsync(int pageNumber, int pageSize)
        {
            _logger.LogInformation("Fetching all employees. Page: {PageNumber}, Size: {PageSize}", pageNumber, pageSize);
            var (employees, totalCount) = await _unitOfWork.EmployeeRepository.GetPagedAsync(pageNumber, pageSize);

            return new PagedResult<EmployeeDto>
            {
                Items = _mapper.Map<IEnumerable<EmployeeDto>>(employees),
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<EmployeeDto?> GetEmployeeByIdAsync(int id)
        {
            _logger.LogInformation("Fetching employee with Id {EmployeeId}", id);
            var employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(id);
            if (employee == null)
            {
                _logger.LogWarning("Employee with Id {EmployeeId} not found", id);
                throw new NotFoundException($"Employee with Id {id} not found");
            }

            return _mapper.Map<EmployeeDto>(employee);
        }

        public async Task<EmployeeDto> AddEmployeeAsync(CreateEmployeeDto dto)
        {
            try
            {
                var employee = _mapper.Map<Employee>(dto);
                await _unitOfWork.EmployeeRepository.AddAsync(employee);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Creating employee {Name}", dto.FullName);

                return _mapper.Map<EmployeeDto>(employee);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating employee");
                throw;
            }
        }

        public async Task<EmployeeDto> UpdateEmployeeAsync(UpdateEmployeeDto dto)
        {
            try
            {
                _logger.LogInformation("Updating employee with Id {EmployeeId}", dto.EmployeeId);
                var employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(dto.EmployeeId);
                if (employee == null)
                {
                    _logger.LogWarning("Employee with Id {EmployeeId} not found", dto.EmployeeId);
                    throw new NotFoundException($"Employee with Id {dto.EmployeeId} not found");
                }

                _mapper.Map(dto, employee);
                await _unitOfWork.EmployeeRepository.UpdateAsync(employee);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Employee with Id {EmployeeId} updated successfully", dto.EmployeeId);

                return _mapper.Map<EmployeeDto>(employee);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating employee");
                throw;
            }
        }

        public async Task DeleteEmployeeAsync(int id)
        {
            try
            {
                _logger.LogInformation("Deleting employee with Id {EmployeeId}", id);
                var employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(id);
                if (employee == null)
                {
                    _logger.LogWarning("Employee with Id {EmployeeId} not found", id);
                    throw new NotFoundException($"Employee with Id {id} not found");
                }

                await _unitOfWork.EmployeeRepository.SoftDeleteAsync(id);
                await _unitOfWork.SaveChangesAsync();
                _logger.LogInformation("Employee with Id {EmployeeId} deleted successfully", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting employee");
                throw;
            }
        }
    }
}
