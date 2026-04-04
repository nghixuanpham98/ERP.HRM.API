using ERP.HRM.Domain.Entities;

namespace ERP.HRM.Domain.Interfaces.Repositories
{
    public interface ITaxBracketRepository
    {
        Task<IEnumerable<TaxBracket>> GetAllAsync();
        Task<IEnumerable<TaxBracket>> GetActiveBracketsAsync(DateTime asOfDate);
        Task<TaxBracket?> GetByIdAsync(int bracketId);
        Task<TaxBracket?> GetBracketForIncomeAsync(decimal income, DateTime asOfDate);
        Task AddAsync(TaxBracket bracket);
        Task UpdateAsync(TaxBracket bracket);
        Task DeleteAsync(int bracketId);
    }
}
