using ERP.HRM.API;
using ERP.HRM.Domain.Entities;
using ERP.HRM.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERP.HRM.Infrastructure.Repositories;

public class PositionRepository : IPositionRepository
{
    private readonly ERPDbContext _context;

    public PositionRepository(ERPDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Position>> GetAllAsync()
        => await _context.Positions.ToListAsync();

    public async Task<Position?> GetByIdAsync(int id)
        => await _context.Positions.FindAsync(id);

    public async Task AddAsync(Position position)
    {
        await _context.Positions.AddAsync(position);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Position position)
    {
        _context.Positions.Update(position);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var ent = await _context.Positions.FindAsync(id);
        if (ent != null)
        {
            _context.Positions.Remove(ent);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsByCodeAsync(string positionCode)
    {
        return await _context.Positions.AnyAsync(p => p.PositionCode == positionCode);
    }

    public async Task<int> GetEmployeeCountAsync(int positionId)
    {
        return await _context.Employees.CountAsync(e => e.PositionId == positionId);
    }

    public async Task<(IEnumerable<Position>, int)> GetPagedAsync(int pageNumber, int pageSize)
    {
        var positions = await _context.Positions
            .FromSqlRaw("EXEC sp_GetPositionsPaged @PageNumber={0}, @PageSize={1}", pageNumber, pageSize)
            .ToListAsync();

        var totalCount = await _context.Positions.CountAsync();
        return (positions, totalCount);
    }
}
