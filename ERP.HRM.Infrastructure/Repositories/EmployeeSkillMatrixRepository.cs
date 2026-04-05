using ERP.HRM.API;
using ERP.HRM.Domain.Entities;
using ERP.HRM.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ERP.HRM.Infrastructure.Repositories
{
    public class EmployeeSkillMatrixRepository : BaseRepository<EmployeeSkillMatrix>, IEmployeeSkillMatrixRepository
    {
        private readonly ERPDbContext _context;

        public EmployeeSkillMatrixRepository(ERPDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<EmployeeSkillMatrix>> GetByEmployeeAsync(int employeeId)
        {
            return await _context.EmployeeSkillMatrices
                .Where(esm => esm.EmployeeId == employeeId && !esm.IsDeleted)
                .OrderByDescending(esm => esm.Level)
                .ToListAsync();
        }

        public async Task<IEnumerable<EmployeeSkillMatrix>> GetRequiredSkillsAsync(int employeeId)
        {
            return await _context.EmployeeSkillMatrices
                .Where(esm => esm.EmployeeId == employeeId && esm.IsRequired && !esm.IsDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<EmployeeSkillMatrix>> GetBySkillNameAsync(string skillName)
        {
            return await _context.EmployeeSkillMatrices
                .Where(esm => esm.SkillName == skillName && !esm.IsDeleted)
                .OrderByDescending(esm => esm.Level)
                .ToListAsync();
        }

        public async Task<IEnumerable<EmployeeSkillMatrix>> GetRequiringAssessmentAsync()
        {
            var now = DateTime.Now;
            return await _context.EmployeeSkillMatrices
                .Where(esm => (esm.NextAssessmentDueDate == null || esm.NextAssessmentDueDate <= now) && !esm.IsDeleted)
                .OrderBy(esm => esm.NextAssessmentDueDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<EmployeeSkillMatrix>> GetByLevelAsync(int level)
        {
            return await _context.EmployeeSkillMatrices
                .Where(esm => esm.Level == level && !esm.IsDeleted)
                .OrderBy(esm => esm.EmployeeId)
                .ToListAsync();
        }

        public async Task<bool> HasSkillAsync(int employeeId, string skillName, int minimumLevel)
        {
            return await _context.EmployeeSkillMatrices
                .AnyAsync(esm => esm.EmployeeId == employeeId && 
                                esm.SkillName == skillName && 
                                esm.Level >= minimumLevel &&
                                !esm.IsDeleted);
        }
    }
}
