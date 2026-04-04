using ERP.HRM.Application.DTOs.Payroll;
using MediatR;

namespace ERP.HRM.Application.Features.Payroll.Commands
{
    /// <summary>
    /// Command to calculate production-based salary for a worker
    /// For factory workers (salary = unit price * quantity)
    /// </summary>
    public class CalculateProductionSalaryCommand : IRequest<PayrollRecordDto>
    {
        public int EmployeeId { get; set; }
        public int PayrollPeriodId { get; set; }
        public decimal? OverrideUnitPrice { get; set; }
        public decimal? OverrideAllowance { get; set; }
        public string? Notes { get; set; }
    }
}
