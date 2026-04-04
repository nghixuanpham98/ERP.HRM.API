using ERP.HRM.API;
using ERP.HRM.Domain.Entities;
using ERP.HRM.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ERP.HRM.Infrastructure.Repositories
{
    public class SalaryAdjustmentDecisionRepository : BaseRepository<SalaryAdjustmentDecision>, ISalaryAdjustmentDecisionRepository
    {
        public SalaryAdjustmentDecisionRepository(ERPDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<SalaryAdjustmentDecision>> GetByEmployeeIdAsync(int employeeId)
        {
            return await Context.SalaryAdjustmentDecisions
                .Where(x => x.EmployeeId == employeeId && !x.IsDeleted)
                .OrderByDescending(x => x.DecisionDate)
                .ToListAsync();
        }

        public async Task<SalaryAdjustmentDecision?> GetLatestByEmployeeIdAsync(int employeeId)
        {
            return await Context.SalaryAdjustmentDecisions
                .Where(x => x.EmployeeId == employeeId && !x.IsDeleted && x.Status == "Applied")
                .OrderByDescending(x => x.EffectiveImplementationDate)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<SalaryAdjustmentDecision>> GetPendingDecisionsAsync()
        {
            return await Context.SalaryAdjustmentDecisions
                .Where(x => !x.IsDeleted && x.Status == "Pending")
                .OrderBy(x => x.DecisionDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<SalaryAdjustmentDecision>> GetByStatusAsync(string status)
        {
            return await Context.SalaryAdjustmentDecisions
                .Where(x => !x.IsDeleted && x.Status == status)
                .OrderByDescending(x => x.DecisionDate)
                .ToListAsync();
        }

        public override async Task<SalaryAdjustmentDecision?> GetByIdAsync(int id)
        {
            return await Context.SalaryAdjustmentDecisions
                .FirstOrDefaultAsync(x => x.SalaryAdjustmentDecisionId == id && !x.IsDeleted);
        }

        public async Task AddAsync(SalaryAdjustmentDecision decision)
        {
            await Context.SalaryAdjustmentDecisions.AddAsync(decision);
        }

        public async Task UpdateAsync(SalaryAdjustmentDecision decision)
        {
            Context.SalaryAdjustmentDecisions.Update(decision);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(int decisionId)
        {
            var decision = await GetByIdAsync(decisionId);
            if (decision != null)
            {
                decision.IsDeleted = true;
                Context.SalaryAdjustmentDecisions.Update(decision);
            }
        }
    }
}
