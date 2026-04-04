using ERP.HRM.API;
using ERP.HRM.Domain.Entities;
using ERP.HRM.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ERP.HRM.Infrastructure.Repositories
{
    public class TaxBracketRepository : BaseRepository<TaxBracket>, ITaxBracketRepository
    {
        public TaxBracketRepository(ERPDbContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<TaxBracket>> GetAllAsync()
        {
            return await Context.TaxBrackets
                .Where(x => !x.IsDeleted)
                .OrderBy(x => x.MinIncome)
                .ToListAsync();
        }

        public async Task<IEnumerable<TaxBracket>> GetActiveBracketsAsync(DateTime asOfDate)
        {
            return await Context.TaxBrackets
                .Where(x => !x.IsDeleted 
                    && x.IsActive 
                    && x.EffectiveDate <= asOfDate 
                    && (x.EndDate == null || x.EndDate >= asOfDate))
                .OrderBy(x => x.MinIncome)
                .ToListAsync();
        }

        public override async Task<TaxBracket?> GetByIdAsync(int id)
        {
            return await Context.TaxBrackets
                .FirstOrDefaultAsync(x => x.TaxBracketId == id && !x.IsDeleted);
        }

        public async Task<TaxBracket?> GetBracketForIncomeAsync(decimal income, DateTime asOfDate)
        {
            return await Context.TaxBrackets
                .Where(x => !x.IsDeleted 
                    && x.IsActive 
                    && x.EffectiveDate <= asOfDate 
                    && (x.EndDate == null || x.EndDate >= asOfDate)
                    && x.MinIncome <= income 
                    && income <= x.MaxIncome)
                .FirstOrDefaultAsync();
        }

        public async Task AddAsync(TaxBracket bracket)
        {
            await Context.TaxBrackets.AddAsync(bracket);
        }

        public async Task UpdateAsync(TaxBracket bracket)
        {
            Context.TaxBrackets.Update(bracket);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(int bracketId)
        {
            var bracket = await GetByIdAsync(bracketId);
            if (bracket != null)
            {
                bracket.IsDeleted = true;
                Context.TaxBrackets.Update(bracket);
            }
        }
    }
}
