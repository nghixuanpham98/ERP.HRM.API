using ERP.HRM.API;
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
            => await _context.Departments.ToListAsync();

        public async Task<Department?> GetByIdAsync(int id)
            => await _context.Departments.FindAsync(id);

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

        public async Task DeleteAsync(int id)
        {
            var dept = await _context.Departments.FindAsync(id);
            if (dept != null)
            {
                _context.Departments.Remove(dept);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsByNameAsync(string departmentName)
        {
            return await _context.Departments.AnyAsync(d => d.DepartmentName == departmentName);
        }

        public async Task<int> GetEmployeeCountAsync(int departmentId)
        {
            return await _context.Employees.CountAsync(e => e.DepartmentId == departmentId);
        }
    }
}
