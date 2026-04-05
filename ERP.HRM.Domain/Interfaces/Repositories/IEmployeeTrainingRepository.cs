using ERP.HRM.Application.Interfaces.Repositories;
using ERP.HRM.Domain.Entities;

namespace ERP.HRM.Domain.Interfaces.Repositories
{
    public interface IEmployeeTrainingRepository : IPagedRepository<EmployeeTraining>
    {
        Task<IEnumerable<EmployeeTraining>> GetByEmployeeAsync(int employeeId);
        Task<IEnumerable<EmployeeTraining>> GetCompletedTrainingsAsync(int employeeId);
        Task<IEnumerable<EmployeeTraining>> GetUpcomingTrainingsAsync();
        Task<IEnumerable<EmployeeTraining>> GetByCategoryAsync(string category);
        Task<decimal> GetTotalTrainingHoursAsync(int employeeId);
        Task<decimal> GetTotalTrainingCostAsync(int employeeId);
    }
}
