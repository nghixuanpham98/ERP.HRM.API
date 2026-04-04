using ERP.HRM.Application.DTOs.Payroll;
using MediatR;

namespace ERP.HRM.Application.Features.Payroll.Commands
{
    /// <summary>
    /// Command to calculate monthly salary for an employee in a payroll period
    /// For office employees, mechanics, technicians, etc. (salary based on working days)
    /// </summary>
    public class CalculateMonthlySalaryCommand : IRequest<PayrollRecordDto>
    {
        public int EmployeeId { get; set; }
        public int PayrollPeriodId { get; set; }
        public decimal? OverrideBaseSalary { get; set; }
        public decimal? OverrideAllowance { get; set; }
        public string? Notes { get; set; }
    }
}
