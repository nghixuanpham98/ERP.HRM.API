namespace ERP.HRM.Application.DTOs.Payroll
{
    /// <summary>
    /// DTO for production output
    /// </summary>
    public class ProductionOutputDto
    {
        public int ProductionOutputId { get; set; }
        public int EmployeeId { get; set; }
        public int PayrollPeriodId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public DateTime ProductionDate { get; set; }
        public string QualityStatus { get; set; } = "OK";
        public string? Notes { get; set; }
        public decimal Amount { get; set; }
    }

    public class CreateProductionOutputDto
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

    public class UpdateProductionOutputDto
    {
        public int ProductionOutputId { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string QualityStatus { get; set; } = "OK";
        public string? Notes { get; set; }
    }
}
