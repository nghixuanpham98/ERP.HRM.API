using ERP.HRM.API;
using ERP.HRM.Domain.Entities;
using ERP.HRM.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ERP.HRM.Infrastructure.Repositories
{
    public class FamilyDependentRepository : BaseRepository<FamilyDependent>, IFamilyDependentRepository
    {
        public FamilyDependentRepository(ERPDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<FamilyDependent>> GetByEmployeeIdAsync(int employeeId)
        {
            return await Context.FamilyDependents
                .Where(x => x.EmployeeId == employeeId && !x.IsDeleted)
                .OrderBy(x => x.Relationship)
                .ToListAsync();
        }

        public async Task<IEnumerable<FamilyDependent>> GetQualifiedDependentsByEmployeeIdAsync(int employeeId, DateTime asOfDate)
        {
            var dateOnly = DateOnly.FromDateTime(asOfDate);
            return await Context.FamilyDependents
                .Where(x => x.EmployeeId == employeeId 
                    && !x.IsDeleted 
                    && x.IsQualified
                    && (x.QualificationStartDate == null || x.QualificationStartDate <= dateOnly)
                    && (x.QualificationEndDate == null || x.QualificationEndDate >= dateOnly))
                .OrderBy(x => x.Relationship)
                .ToListAsync();
        }

        public override async Task<FamilyDependent?> GetByIdAsync(int id)
        {
            return await Context.FamilyDependents
                .FirstOrDefaultAsync(x => x.FamilyDependentId == id && !x.IsDeleted);
        }

        public async Task AddAsync(FamilyDependent dependent)
        {
            await Context.FamilyDependents.AddAsync(dependent);
        }

        public async Task UpdateAsync(FamilyDependent dependent)
        {
            Context.FamilyDependents.Update(dependent);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(int dependentId)
        {
            var dependent = await GetByIdAsync(dependentId);
            if (dependent != null)
            {
                dependent.IsDeleted = true;
                Context.FamilyDependents.Update(dependent);
            }
        }
    }
}
