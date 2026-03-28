using ERP.HRM.API;
using ERP.HRM.Domain.Entities;
using ERP.HRM.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERP.HRM.Infrastructure.Repositories
{
    public class PositionRepository : IPositionRepository
    {
        private readonly ERPDbContext _context;

        public PositionRepository(ERPDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Position>> GetAllAsync()
            => await _context.Positions.Where(e => e.IsDeleted == false).ToListAsync();

        public async Task<Position?> GetByIdAsync(int id)
             => await _context.Positions.Where(e => e.IsDeleted == false)
            .FirstOrDefaultAsync(e => e.PositionId == id);

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

        public async Task SoftDeleteAsync(int id)
        {
            var ent = await _context.Positions.FindAsync(id);
            if (ent != null)
            {
                ent.IsDeleted = true;
                ent.ModifiedDate = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsByCodeAsync(string positionCode)
        {
            return await _context.Positions.AnyAsync(p => p.PositionCode == positionCode && p.IsDeleted == false);
        }

        public async Task<int> GetEmployeeCountAsync(int positionId)
        {
            return await _context.Employees.CountAsync(e => e.PositionId == positionId && e.IsDeleted == false);
        }

        public async Task<(IEnumerable<Position>, int)> GetPagedAsync(int pageNumber, int pageSize)
        {
            var positions = await _context.Positions
                .FromSqlRaw("EXEC sp_GetPositionsPaged @PageNumber={0}, @PageSize={1}", pageNumber, pageSize)
                .ToListAsync();

            var totalCount = await _context.Positions
                .Where(e => e.IsDeleted == false)
                .CountAsync();
            return (positions, totalCount);
        }
    }
}
