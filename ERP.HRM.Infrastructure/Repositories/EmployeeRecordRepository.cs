using ERP.HRM.API;
using ERP.HRM.Domain.Entities;
using ERP.HRM.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ERP.HRM.Infrastructure.Repositories
{
    public class EmployeeRecordRepository : BaseRepository<EmployeeRecord>, IEmployeeRecordRepository
    {
        public EmployeeRecordRepository(ERPDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<EmployeeRecord>> GetByEmployeeIdAsync(int employeeId)
        {
            return await Context.EmployeeRecords
                .Where(x => x.EmployeeId == employeeId && !x.IsDeleted)
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<EmployeeRecord>> GetByStatusAsync(string status)
        {
            return await Context.EmployeeRecords
                .Where(x => x.Status == status && !x.IsDeleted)
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<EmployeeRecord>> GetByDocumentTypeAsync(string documentType)
        {
            return await Context.EmployeeRecords
                .Where(x => x.DocumentType == documentType && !x.IsDeleted)
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();
        }

        public override async Task<EmployeeRecord?> GetByIdAsync(int id)
        {
            return await Context.EmployeeRecords
                .FirstOrDefaultAsync(x => x.EmployeeRecordId == id && !x.IsDeleted);
        }

        public async Task<IEnumerable<EmployeeRecord>> GetActiveRecordsAsync()
        {
            var today = DateTime.Today;
            return await Context.EmployeeRecords
                .Where(x => x.Status == "Active" && !x.IsDeleted && (x.ExpiryDate == null || x.ExpiryDate >= today))
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<EmployeeRecord>> GetExpiredRecordsAsync()
        {
            var today = DateTime.Today;
            return await Context.EmployeeRecords
                .Where(x => x.ExpiryDate.HasValue && x.ExpiryDate < today && !x.IsDeleted)
                .OrderByDescending(x => x.ExpiryDate)
                .ToListAsync();
        }

        public async Task AddAsync(EmployeeRecord record)
        {
            await Context.EmployeeRecords.AddAsync(record);
        }

        public async Task UpdateAsync(EmployeeRecord record)
        {
            Context.EmployeeRecords.Update(record);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(int recordId)
        {
            var record = await GetByIdAsync(recordId);
            if (record != null)
            {
                record.IsDeleted = true;
                Context.EmployeeRecords.Update(record);
            }
        }

        public override async Task<IEnumerable<EmployeeRecord>> GetAllAsync()
        {
            return await Context.EmployeeRecords
                .Where(x => !x.IsDeleted)
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();
        }
    }
}
