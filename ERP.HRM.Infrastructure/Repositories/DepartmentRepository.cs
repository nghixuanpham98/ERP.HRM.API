using ERP.HRM.API;
using ERP.HRM.Domain.Entities;
using ERP.HRM.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.HRM.Infrastructure.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly ERPDbContext _context;

        public DepartmentRepository(ERPDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Department>> GetAllAsync()
            => await _context.Departments.Where(e => e.IsDeleted == false).ToListAsync();

        public async Task<Department?> GetByIdAsync(int id)
            => await _context.Departments.Where(e => e.IsDeleted == false)
            .FirstOrDefaultAsync(e => e.DepartmentId == id);

        public async Task AddAsync(Department department)
        {
            _context.Departments.Add(department);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Department department)
        {
            _context.Departments.Update(department);
            await _context.SaveChangesAsync();
        }

        public async Task SoftDeleteAsync(int id)
        {
            var dept = await _context.Departments.FindAsync(id);
            if (dept != null)
            {
                dept.IsDeleted = true;
                dept.ModifiedDate = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsByNameAsync(string departmentName)
        {
            return await _context.Departments
                .AnyAsync(d => d.DepartmentName == departmentName && d.IsDeleted == false);
        }

        public async Task<int> GetEmployeeCountAsync(int departmentId)
        {
            return await _context.Employees
                .CountAsync(e => e.DepartmentId == departmentId && e.IsDeleted == false);
        }

        public async Task<(IEnumerable<Department>, int)> GetPagedAsync(int pageNumber, int pageSize)
        {
            var departments = await _context.Departments
                .FromSqlRaw("EXEC sp_GetDepartmentsPaged @PageNumber={0}, @PageSize={1}", pageNumber, pageSize)
                .ToListAsync();

            var totalCount = await _context.Departments
                .Where(e => e.IsDeleted == false)
                .CountAsync();
            return (departments, totalCount);
        }
    }
}
