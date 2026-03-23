using ERP.HRM.Domain.Entities;
using ERP.HRM.Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.HRM.Domain.Interfaces.Repositories
{
    public interface IDepartmentRepository : IPagedRepository<Department>
    {
        Task<IEnumerable<Department>> GetAllAsync();
        Task<Department?> GetByIdAsync(int id);
        Task AddAsync(Department department);
        Task UpdateAsync(Department department);
        Task DeleteAsync(int id);
        Task<bool> ExistsByNameAsync(string departmentName);
        Task<int> GetEmployeeCountAsync(int departmentId);
    }
}
