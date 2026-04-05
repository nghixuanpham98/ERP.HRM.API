using ERP.HRM.Application.Interfaces.Repositories;
using ERP.HRM.Domain.Entities;

namespace ERP.HRM.Domain.Interfaces.Repositories
{
    public interface IEmployeeSkillMatrixRepository : IPagedRepository<EmployeeSkillMatrix>
    {
        Task<IEnumerable<EmployeeSkillMatrix>> GetByEmployeeAsync(int employeeId);
        Task<IEnumerable<EmployeeSkillMatrix>> GetRequiredSkillsAsync(int employeeId);
        Task<IEnumerable<EmployeeSkillMatrix>> GetBySkillNameAsync(string skillName);
        Task<IEnumerable<EmployeeSkillMatrix>> GetRequiringAssessmentAsync();
        Task<IEnumerable<EmployeeSkillMatrix>> GetByLevelAsync(int level);
        Task<bool> HasSkillAsync(int employeeId, string skillName, int minimumLevel);
    }
}
