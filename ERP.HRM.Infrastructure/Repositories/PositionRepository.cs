using ERP.HRM.API;
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
}
