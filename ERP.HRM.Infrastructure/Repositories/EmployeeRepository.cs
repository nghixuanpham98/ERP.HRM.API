using ERP.HRM.API;
using ERP.HRM.Application.Interfaces.Repositories;
using ERP.HRM.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERP.HRM.Infrastructure.Repositories;

public class EmployeeRepository : IEmployeeRepository, IPagedRepository<Employee>
{
    private readonly ERPDbContext _context;

    public EmployeeRepository(ERPDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Employee>> GetAllAsync()
        => await _context.Employees
            .Include(e => e.Department)
            .Include(e => e.Position)
            .ToListAsync();

    public async Task<Employee?> GetByIdAsync(int id)
        => await _context.Employees
            .Include(e => e.Department)
            .Include(e => e.Position)
            .FirstOrDefaultAsync(e => e.EmployeeId == id);

    public async Task AddAsync(Employee employee)
    {
        await _context.Employees.AddAsync(employee);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Employee employee)
    {
        _context.Employees.Update(employee);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var ent = await _context.Employees.FindAsync(id);
        if (ent != null)
        {
            _context.Employees.Remove(ent);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Employee>> GetByDepartmentAsync(int departmentId)
    {
        return await _context.Employees
            .Where(e => e.DepartmentId == departmentId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Employee>> GetByPositionAsync(int positionId)
    {
        return await _context.Employees
            .Where(e => e.PositionId == positionId)
            .ToListAsync();
    }

    public async Task<(IEnumerable<Employee>, int)> GetPagedAsync(int pageNumber, int pageSize)
    {
        var employees = await _context.Employees
            .FromSqlRaw("EXEC sp_GetEmployeesPaged @PageNumber={0}, @PageSize={1}", pageNumber, pageSize)
            .ToListAsync();

        var totalCount = await _context.Employees.CountAsync();
        return (employees, totalCount);
    }
}
