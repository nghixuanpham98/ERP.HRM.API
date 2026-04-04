using ERP.HRM.API;
using ERP.HRM.Domain.Entities;
using ERP.HRM.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ERP.HRM.Infrastructure.Repositories
{
    public class SalaryGradeRepository : BaseRepository<SalaryGrade>, ISalaryGradeRepository
    {
        public SalaryGradeRepository(ERPDbContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<SalaryGrade>> GetAllAsync()
        {
            return await Context.SalaryGrades
                .Where(x => !x.IsDeleted && x.IsActive)
                .OrderBy(x => x.GradeLevel)
                .ToListAsync();
        }

        public override async Task<SalaryGrade?> GetByIdAsync(int id)
        {
            return await Context.SalaryGrades
                .FirstOrDefaultAsync(x => x.SalaryGradeId == id && !x.IsDeleted);
        }

        public async Task<SalaryGrade?> GetByNameAsync(string gradeName)
        {
            return await Context.SalaryGrades
                .FirstOrDefaultAsync(x => x.GradeName == gradeName && !x.IsDeleted && x.IsActive);
        }

        public async Task<SalaryGrade?> GetByLevelAsync(int gradeLevel)
        {
            return await Context.SalaryGrades
                .FirstOrDefaultAsync(x => x.GradeLevel == gradeLevel && !x.IsDeleted && x.IsActive);
        }

        public async Task<SalaryGrade?> GetGradeForSalaryAsync(decimal salary)
        {
            return await Context.SalaryGrades
                .Where(x => !x.IsDeleted && x.IsActive && x.MinSalary <= salary && salary <= x.MaxSalary)
                .OrderBy(x => x.GradeLevel)
                .FirstOrDefaultAsync();
        }

        public async Task AddAsync(SalaryGrade grade)
        {
            await Context.SalaryGrades.AddAsync(grade);
        }

        public async Task UpdateAsync(SalaryGrade grade)
        {
            Context.SalaryGrades.Update(grade);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(int gradeId)
        {
            var grade = await GetByIdAsync(gradeId);
            if (grade != null)
            {
                grade.IsDeleted = true;
                Context.SalaryGrades.Update(grade);
            }
        }
    }
}
