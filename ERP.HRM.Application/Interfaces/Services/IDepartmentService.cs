using ERP.HRM.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.HRM.Application.Interfaces.Services
{
    public interface IDepartmentService
    {
        Task<IEnumerable<DepartmentDto>> GetAllDepartmentsAsync();
        Task<DepartmentDto> GetDepartmentByIdAsync(int id);
        Task<DepartmentDto> AddDepartmentAsync(CreateDepartmentDto dto);
        Task<DepartmentDto> UpdateDepartmentAsync(UpdateDepartmentDto dto);
        Task DeleteDepartmentAsync(int id);
    }
}
