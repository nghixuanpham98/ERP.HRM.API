using ERP.HRM.API;
using ERP.HRM.Domain.Entities;
using ERP.HRM.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ERP.HRM.Infrastructure.Repositories
{
    public class PersonnelTransferRepository : BaseRepository<PersonnelTransfer>, IPersonnelTransferRepository
    {
        public PersonnelTransferRepository(ERPDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<PersonnelTransfer>> GetByEmployeeIdAsync(int employeeId)
        {
            return await Context.PersonnelTransfers
                .Where(x => x.EmployeeId == employeeId && !x.IsDeleted)
                .OrderByDescending(x => x.EffectiveDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<PersonnelTransfer>> GetByTransferTypeAsync(string transferType)
        {
            return await Context.PersonnelTransfers
                .Where(x => x.TransferType == transferType && !x.IsDeleted)
                .OrderByDescending(x => x.EffectiveDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<PersonnelTransfer>> GetByApprovalStatusAsync(string approvalStatus)
        {
            return await Context.PersonnelTransfers
                .Where(x => x.ApprovalStatus == approvalStatus && !x.IsDeleted)
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();
        }

        public override async Task<PersonnelTransfer?> GetByIdAsync(int id)
        {
            return await Context.PersonnelTransfers
                .FirstOrDefaultAsync(x => x.PersonnelTransferId == id && !x.IsDeleted);
        }

        public async Task<IEnumerable<PersonnelTransfer>> GetPendingTransfersAsync()
        {
            return await Context.PersonnelTransfers
                .Where(x => x.ApprovalStatus == "Pending" && !x.IsDeleted)
                .OrderBy(x => x.CreatedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<PersonnelTransfer>> GetByDepartmentAsync(int departmentId)
        {
            return await Context.PersonnelTransfers
                .Where(x => (x.FromDepartmentId == departmentId || x.ToDepartmentId == departmentId) && !x.IsDeleted)
                .OrderByDescending(x => x.EffectiveDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<PersonnelTransfer>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var start = DateOnly.FromDateTime(startDate);
            var end = DateOnly.FromDateTime(endDate);

            return await Context.PersonnelTransfers
                .Where(x => x.EffectiveDate >= start && x.EffectiveDate <= end && !x.IsDeleted)
                .OrderBy(x => x.EffectiveDate)
                .ToListAsync();
        }

        public async Task AddAsync(PersonnelTransfer transfer)
        {
            await Context.PersonnelTransfers.AddAsync(transfer);
        }

        public async Task UpdateAsync(PersonnelTransfer transfer)
        {
            Context.PersonnelTransfers.Update(transfer);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(int transferId)
        {
            var transfer = await GetByIdAsync(transferId);
            if (transfer != null)
            {
                transfer.IsDeleted = true;
                Context.PersonnelTransfers.Update(transfer);
            }
        }

        public override async Task<IEnumerable<PersonnelTransfer>> GetAllAsync()
        {
            return await Context.PersonnelTransfers
                .Where(x => !x.IsDeleted)
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();
        }
    }
}
