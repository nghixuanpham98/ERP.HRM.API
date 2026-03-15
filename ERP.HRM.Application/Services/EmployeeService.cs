using AutoMapper;
using ERP.HRM.Application.DTOs;
using ERP.HRM.Application.Interfaces;
using ERP.HRM.Domain.Interfaces.Repositories;
using ERP.HRM.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync()
        {
            var employees = await _employeeRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<EmployeeDto>>(employees);
        }

        public async Task<EmployeeDto?> GetEmployeeByIdAsync(int id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            return _mapper.Map<EmployeeDto?>(employee);
        }

        public async Task AddEmployeeAsync(CreateEmployeeDto dto)
        {
            var employee = _mapper.Map<Employee>(dto);
            await _employeeRepository.AddAsync(employee);
        }

        public async Task UpdateEmployeeAsync(UpdateEmployeeDto dto)
        {
            var employee = _mapper.Map<Employee>(dto);
            await _employeeRepository.UpdateAsync(employee);
        }

        public async Task DeleteEmployeeAsync(int id)
        {
            await _employeeRepository.DeleteAsync(id);
        }
    }
}