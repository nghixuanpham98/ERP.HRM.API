using ERP.HRM.API;
using ERP.HRM.Domain.Entities;
using ERP.HRM.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ERP.HRM.Infrastructure.Repositories
{
    public class EmploymentContractRepository : BaseRepository<EmploymentContract>, IEmploymentContractRepository
    {
        public EmploymentContractRepository(ERPDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<EmploymentContract>> GetByEmployeeIdAsync(int employeeId)
        {
            return await Context.EmploymentContracts
                .Where(x => x.EmployeeId == employeeId && !x.IsDeleted)
                .OrderByDescending(x => x.StartDate)
                .ToListAsync();
        }

        public async Task<EmploymentContract?> GetActiveContractByEmployeeIdAsync(int employeeId)
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            return await Context.EmploymentContracts
                .Where(x => x.EmployeeId == employeeId 
                    && !x.IsDeleted 
                    && x.IsActive 
                    && x.StartDate <= today 
                    && (x.EndDate == null || x.EndDate >= today))
                .OrderByDescending(x => x.StartDate)
                .FirstOrDefaultAsync();
        }

        public override async Task<EmploymentContract?> GetByIdAsync(int id)
        {
            return await Context.EmploymentContracts
                .FirstOrDefaultAsync(x => x.EmploymentContractId == id && !x.IsDeleted);
        }

        public async Task AddAsync(EmploymentContract contract)
        {
            await Context.EmploymentContracts.AddAsync(contract);
        }

        public async Task UpdateAsync(EmploymentContract contract)
        {
            Context.EmploymentContracts.Update(contract);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(int contractId)
        {
            var contract = await GetByIdAsync(contractId);
            if (contract != null)
            {
                contract.IsDeleted = true;
                Context.EmploymentContracts.Update(contract);
            }
        }
    }
}
