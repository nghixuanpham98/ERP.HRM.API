using ERP.HRM.API;
using ERP.HRM.Domain.Entities;
using ERP.HRM.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ERP.HRM.Infrastructure.Repositories
{
    public class PayrollExceptionRepository : BaseRepository<PayrollException>, IPayrollExceptionRepository
    {
        private readonly ERPDbContext _context;

        public PayrollExceptionRepository(ERPDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PayrollException>> GetByPayrollPeriodAsync(int payrollPeriodId)
        {
            return await _context.PayrollExceptions
                .Where(pe => pe.PayrollPeriodId == payrollPeriodId && !pe.IsDeleted)
                .OrderByDescending(pe => pe.CreatedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<PayrollException>> GetByEmployeeAsync(int employeeId)
        {
            return await _context.PayrollExceptions
                .Where(pe => pe.EmployeeId == employeeId && !pe.IsDeleted)
                .OrderByDescending(pe => pe.CreatedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<PayrollException>> GetOpenExceptionsAsync()
        {
            return await _context.PayrollExceptions
                .Where(pe => (pe.Status == "Open" || pe.Status == "InProgress") && !pe.IsDeleted)
                .OrderByDescending(pe => pe.Severity)
                .ToListAsync();
        }

        public async Task<IEnumerable<PayrollException>> GetBlockingExceptionsAsync()
        {
            return await _context.PayrollExceptions
                .Where(pe => pe.IsBlocking && (pe.Status == "Open" || pe.Status == "InProgress") && !pe.IsDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<PayrollException>> GetByTypeAsync(string exceptionType)
        {
            return await _context.PayrollExceptions
                .Where(pe => pe.ExceptionType == exceptionType && !pe.IsDeleted)
                .OrderByDescending(pe => pe.CreatedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<PayrollException>> GetOverdueExceptionsAsync(DateTime asOfDate)
        {
            return await _context.PayrollExceptions
                .Where(pe => pe.TargetResolutionDate < asOfDate && 
                            (pe.Status == "Open" || pe.Status == "InProgress") && 
                            !pe.IsDeleted)
                .ToListAsync();
        }

        public async Task UpdateExceptionStatusAsync(int exceptionId, string status, Guid? resolvedByUserId, string? resolutionNotes)
        {
            var exception = await _context.PayrollExceptions.FindAsync(exceptionId);
            if (exception != null)
            {
                exception.Status = status;
                if (resolvedByUserId.HasValue)
                {
                    exception.ResolvedByUserId = resolvedByUserId;
                    exception.ResolvedDate = DateTime.Now;
                }
                if (!string.IsNullOrEmpty(resolutionNotes))
                {
                    exception.ResolutionNotes = resolutionNotes;
                }
                _context.PayrollExceptions.Update(exception);
                await _context.SaveChangesAsync();
            }
        }
    }
}
