using ERP.HRM.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERP.HRM.Infrastructure.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly ERPDbContext _context;

    public EmployeeRepository(ERPDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Employee>> GetAllAsync()
        => await _context.Employees.ToListAsync();

    public async Task<Employee?> GetByIdAsync(int id)
        => await _context.Employees.FindAsync(id);

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
}
