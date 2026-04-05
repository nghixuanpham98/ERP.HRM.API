using ERP.HRM.API;
using ERP.HRM.Domain.Entities;
using ERP.HRM.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ERP.HRM.Infrastructure.Repositories
{
    public class EmployeeTrainingRepository : BaseRepository<EmployeeTraining>, IEmployeeTrainingRepository
    {
        private readonly ERPDbContext _context;

        public EmployeeTrainingRepository(ERPDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<EmployeeTraining>> GetByEmployeeAsync(int employeeId)
        {
            return await _context.EmployeeTrainings
                .Where(et => et.EmployeeId == employeeId && !et.IsDeleted)
                .OrderByDescending(et => et.StartDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<EmployeeTraining>> GetCompletedTrainingsAsync(int employeeId)
        {
            return await _context.EmployeeTrainings
                .Where(et => et.EmployeeId == employeeId && et.Status == "Completed" && !et.IsDeleted)
                .OrderByDescending(et => et.CompletionDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<EmployeeTraining>> GetUpcomingTrainingsAsync()
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            return await _context.EmployeeTrainings
                .Where(et => et.StartDate > today && (et.Status == "Scheduled" || et.Status == "InProgress") && !et.IsDeleted)
                .OrderBy(et => et.StartDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<EmployeeTraining>> GetByCategoryAsync(string category)
        {
            return await _context.EmployeeTrainings
                .Where(et => et.Category == category && !et.IsDeleted)
                .OrderByDescending(et => et.StartDate)
                .ToListAsync();
        }

        public async Task<decimal> GetTotalTrainingHoursAsync(int employeeId)
        {
            return await _context.EmployeeTrainings
                .Where(et => et.EmployeeId == employeeId && et.Status == "Completed" && !et.IsDeleted)
                .SumAsync(et => et.DurationHours);
        }

        public async Task<decimal> GetTotalTrainingCostAsync(int employeeId)
        {
            return await _context.EmployeeTrainings
                .Where(et => et.EmployeeId == employeeId && !et.IsDeleted)
                .SumAsync(et => et.TrainingCost ?? 0);
        }
    }
}
