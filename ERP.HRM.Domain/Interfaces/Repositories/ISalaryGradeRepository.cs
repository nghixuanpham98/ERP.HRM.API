using ERP.HRM.Domain.Entities;

namespace ERP.HRM.Domain.Interfaces.Repositories
{
    public interface ISalaryGradeRepository
    {
        Task<IEnumerable<SalaryGrade>> GetAllAsync();
        Task<SalaryGrade?> GetByIdAsync(int gradeId);
        Task<SalaryGrade?> GetByNameAsync(string gradeName);
        Task<SalaryGrade?> GetByLevelAsync(int gradeLevel);
        Task<SalaryGrade?> GetGradeForSalaryAsync(decimal salary);
        Task AddAsync(SalaryGrade grade);
        Task UpdateAsync(SalaryGrade grade);
        Task DeleteAsync(int gradeId);
    }
}
