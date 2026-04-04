using ERP.HRM.API;
using ERP.HRM.Domain.Entities;
using ERP.HRM.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ERP.HRM.Infrastructure.Repositories
{
    public class ResignationDecisionRepository : BaseRepository<ResignationDecision>, IResignationDecisionRepository
    {
        public ResignationDecisionRepository(ERPDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<ResignationDecision>> GetByEmployeeIdAsync(int employeeId)
        {
            return await Context.ResignationDecisions
                .Where(x => x.EmployeeId == employeeId && !x.IsDeleted)
                .OrderByDescending(x => x.NoticeDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<ResignationDecision>> GetByResignationTypeAsync(string resignationType)
        {
            return await Context.ResignationDecisions
                .Where(x => x.ResignationType == resignationType && !x.IsDeleted)
                .OrderByDescending(x => x.NoticeDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<ResignationDecision>> GetByStatusAsync(string status)
        {
            return await Context.ResignationDecisions
                .Where(x => x.Status == status && !x.IsDeleted)
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();
        }

        public override async Task<ResignationDecision?> GetByIdAsync(int id)
        {
            return await Context.ResignationDecisions
                .FirstOrDefaultAsync(x => x.ResignationDecisionId == id && !x.IsDeleted);
        }

        public async Task<IEnumerable<ResignationDecision>> GetPendingDecisionsAsync()
        {
            return await Context.ResignationDecisions
                .Where(x => x.Status == "Pending" && !x.IsDeleted)
                .OrderBy(x => x.CreatedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<ResignationDecision>> GetApprovedDecisionsAsync()
        {
            return await Context.ResignationDecisions
                .Where(x => x.Status == "Approved" && !x.IsDeleted)
                .OrderByDescending(x => x.EffectiveDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<ResignationDecision>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var start = DateOnly.FromDateTime(startDate);
            var end = DateOnly.FromDateTime(endDate);

            return await Context.ResignationDecisions
                .Where(x => x.EffectiveDate >= start && x.EffectiveDate <= end && !x.IsDeleted)
                .OrderBy(x => x.EffectiveDate)
                .ToListAsync();
        }

        public async Task<ResignationDecision?> GetLatestByEmployeeIdAsync(int employeeId)
        {
            return await Context.ResignationDecisions
                .Where(x => x.EmployeeId == employeeId && !x.IsDeleted)
                .OrderByDescending(x => x.NoticeDate)
                .FirstOrDefaultAsync();
        }

        public async Task AddAsync(ResignationDecision decision)
        {
            await Context.ResignationDecisions.AddAsync(decision);
        }

        public async Task UpdateAsync(ResignationDecision decision)
        {
            Context.ResignationDecisions.Update(decision);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(int decisionId)
        {
            var decision = await GetByIdAsync(decisionId);
            if (decision != null)
            {
                decision.IsDeleted = true;
                Context.ResignationDecisions.Update(decision);
            }
        }

        public override async Task<IEnumerable<ResignationDecision>> GetAllAsync()
        {
            return await Context.ResignationDecisions
                .Where(x => !x.IsDeleted)
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();
        }
    }
}
