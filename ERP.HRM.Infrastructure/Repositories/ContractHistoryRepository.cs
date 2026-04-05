using ERP.HRM.API;
using ERP.HRM.Domain.Entities;
using ERP.HRM.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ERP.HRM.Infrastructure.Repositories
{
    public class ContractHistoryRepository : BaseRepository<ContractHistory>, IContractHistoryRepository
    {
        private readonly ERPDbContext _context;

        public ContractHistoryRepository(ERPDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ContractHistory>> GetByContractAsync(int employmentContractId)
        {
            return await _context.ContractHistories
                .Where(ch => ch.EmploymentContractId == employmentContractId && !ch.IsDeleted)
                .OrderByDescending(ch => ch.ModifiedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<ContractHistory>> GetByEmployeeAsync(int employeeId)
        {
            return await _context.ContractHistories
                .Where(ch => ch.EmploymentContract!.EmployeeId == employeeId && !ch.IsDeleted)
                .Include(ch => ch.EmploymentContract)
                .OrderByDescending(ch => ch.ModifiedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<ContractHistory>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.ContractHistories
                .Where(ch => ch.ModifiedDate >= startDate && ch.ModifiedDate <= endDate && !ch.IsDeleted)
                .OrderByDescending(ch => ch.ModifiedDate)
                .ToListAsync();
        }

        public async Task<ContractHistory?> GetLatestChangeAsync(int employmentContractId)
        {
            return await _context.ContractHistories
                .Where(ch => ch.EmploymentContractId == employmentContractId && !ch.IsDeleted)
                .OrderByDescending(ch => ch.ModifiedDate)
                .FirstOrDefaultAsync();
        }
    }
}
