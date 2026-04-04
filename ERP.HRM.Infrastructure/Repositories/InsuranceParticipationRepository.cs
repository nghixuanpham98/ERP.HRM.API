using ERP.HRM.API;
using ERP.HRM.Domain.Entities;
using ERP.HRM.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ERP.HRM.Infrastructure.Repositories
{
    public class InsuranceParticipationRepository : BaseRepository<InsuranceParticipation>, IInsuranceParticipationRepository
    {
        public InsuranceParticipationRepository(ERPDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<InsuranceParticipation>> GetByEmployeeIdAsync(int employeeId)
        {
            return await Context.InsuranceParticipations
                .Where(x => x.EmployeeId == employeeId && !x.IsDeleted)
                .OrderByDescending(x => x.StartDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<InsuranceParticipation>> GetByInsuranceTypeAsync(string insuranceType)
        {
            return await Context.InsuranceParticipations
                .Where(x => x.InsuranceType == insuranceType && !x.IsDeleted)
                .OrderByDescending(x => x.StartDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<InsuranceParticipation>> GetByStatusAsync(string status)
        {
            return await Context.InsuranceParticipations
                .Where(x => x.Status == status && !x.IsDeleted)
                .OrderByDescending(x => x.StartDate)
                .ToListAsync();
        }

        public override async Task<InsuranceParticipation?> GetByIdAsync(int id)
        {
            return await Context.InsuranceParticipations
                .FirstOrDefaultAsync(x => x.InsuranceParticipationId == id && !x.IsDeleted);
        }

        public async Task<IEnumerable<InsuranceParticipation>> GetActiveInsurancesAsync()
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            return await Context.InsuranceParticipations
                .Where(x => x.Status == "Active" && !x.IsDeleted && x.StartDate <= today && (x.EndDate == null || x.EndDate >= today))
                .OrderBy(x => x.InsuranceType)
                .ToListAsync();
        }

        public async Task<InsuranceParticipation?> GetActiveInsuranceByEmployeeAndTypeAsync(int employeeId, string insuranceType)
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            return await Context.InsuranceParticipations
                .Where(x => x.EmployeeId == employeeId 
                    && x.InsuranceType == insuranceType 
                    && x.Status == "Active"
                    && !x.IsDeleted 
                    && x.StartDate <= today 
                    && (x.EndDate == null || x.EndDate >= today))
                .OrderByDescending(x => x.StartDate)
                .FirstOrDefaultAsync();
        }

        public async Task AddAsync(InsuranceParticipation participation)
        {
            await Context.InsuranceParticipations.AddAsync(participation);
        }

        public async Task UpdateAsync(InsuranceParticipation participation)
        {
            Context.InsuranceParticipations.Update(participation);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(int participationId)
        {
            var participation = await GetByIdAsync(participationId);
            if (participation != null)
            {
                participation.IsDeleted = true;
                Context.InsuranceParticipations.Update(participation);
            }
        }

        public override async Task<IEnumerable<InsuranceParticipation>> GetAllAsync()
        {
            return await Context.InsuranceParticipations
                .Where(x => !x.IsDeleted)
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();
        }
    }
}
