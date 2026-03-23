using AutoMapper;
using ERP.HRM.Domain.Entities;
using ERP.HRM.Application.Common;
using ERP.HRM.Application.DTOs.Employee;
using ERP.HRM.Application.Interfaces;
using ERP.HRM.Domain.Exceptions;
using ERP.HRM.Domain.Interfaces.Repositories;

namespace ERP.HRM.Application.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;

        public EmployeeService(IEmployeeRepository employeeRepository, IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _mapper = mapper;
        }

        public async Task<PagedResult<EmployeeDto>> GetAllEmployeesAsync(int pageNumber, int pageSize)
        {
            var (employees, totalCount) = await _employeeRepository.GetPagedAsync(pageNumber, pageSize);

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
            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null)
                throw new NotFoundException($"Employee with Id {id} not found");

            return _mapper.Map<EmployeeDto>(employee);
        }

        public async Task<EmployeeDto> AddEmployeeAsync(CreateEmployeeDto dto)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(dto.FullName))
                errors.Add("FullName is required");

            if (dto.DepartmentId <= 0)
                errors.Add("DepartmentId must be valid");

            if (errors.Any())
            {
                var ex = new ValidationException("Employee data is invalid");
                ex.Data["Errors"] = errors;
                throw ex;
            }

            var employee = _mapper.Map<Employee>(dto);
            await _employeeRepository.AddAsync(employee);

            return _mapper.Map<EmployeeDto>(employee);
        }

        public async Task<EmployeeDto> UpdateEmployeeAsync(UpdateEmployeeDto dto)
        {
            var employee = await _employeeRepository.GetByIdAsync(dto.EmployeeId);
            if (employee == null)
                throw new NotFoundException($"Employee with Id {dto.EmployeeId} not found");

            _mapper.Map(dto, employee);
            await _employeeRepository.UpdateAsync(employee);

            return _mapper.Map<EmployeeDto>(employee);
        }

        public async Task DeleteEmployeeAsync(int id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null)
                throw new NotFoundException($"Employee with Id {id} not found");

            await _employeeRepository.DeleteAsync(id);
        }
    }
}
