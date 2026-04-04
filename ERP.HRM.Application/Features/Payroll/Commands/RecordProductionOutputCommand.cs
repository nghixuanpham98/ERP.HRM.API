using ERP.HRM.Application.DTOs.Payroll;
using MediatR;

namespace ERP.HRM.Application.Features.Payroll.Commands
{
    /// <summary>
    /// Command to record production output for a worker
    /// </summary>
    public class RecordProductionOutputCommand : IRequest<ProductionOutputDto>
    {
        public int EmployeeId { get; set; }
        public int PayrollPeriodId { get; set; }
        public int ProductId { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public DateTime ProductionDate { get; set; }
        public string QualityStatus { get; set; } = "OK";
        public string? Notes { get; set; }
    }
}
