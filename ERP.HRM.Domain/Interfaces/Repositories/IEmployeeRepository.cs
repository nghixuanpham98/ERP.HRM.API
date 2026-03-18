using ERP.HRM.API;
using ERP.HRM.Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.HRM.Domain.Interfaces.Repositories
{
    public interface IEmployeeRepository : IPagedRepository<Employee>
    {
        Task<IEnumerable<Employee>> GetAllAsync();
        Task<Employee?> GetByIdAsync(int id);
        Task AddAsync(Employee employee);
        Task UpdateAsync(Employee employee);
        Task DeleteAsync(int id);
        Task<IEnumerable<Employee>> GetByDepartmentAsync(int departmentId);
        Task<IEnumerable<Employee>> GetByPositionAsync(int positionId);
    }
}
