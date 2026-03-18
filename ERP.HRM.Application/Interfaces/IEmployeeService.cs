using ERP.HRM.Application.Common;
using ERP.HRM.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.HRM.Application.Interfaces
{
    public interface IEmployeeService
    {
        Task<PagedResult<EmployeeDto>> GetAllEmployeesAsync(int pageNumber, int pageSize);
        Task<EmployeeDto?> GetEmployeeByIdAsync(int id);
        Task<EmployeeDto> AddEmployeeAsync(CreateEmployeeDto dto);
        Task<EmployeeDto> UpdateEmployeeAsync(UpdateEmployeeDto dto);
        Task DeleteEmployeeAsync(int id);
    }
}