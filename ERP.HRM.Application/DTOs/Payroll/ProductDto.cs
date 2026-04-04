namespace ERP.HRM.Application.DTOs.Payroll
{
    /// <summary>
    /// DTO for product
    /// </summary>
    public class ProductDto
    {
        public int ProductId { get; set; }
        public string ProductCode { get; set; } = null!;
        public string ProductName { get; set; } = null!;
        public string? Description { get; set; }
        public string Unit { get; set; } = "cái";
        public string? Category { get; set; }
        public string Status { get; set; } = "Active";
    }

    public class CreateProductDto
    {
        public string ProductCode { get; set; } = null!;
        public string ProductName { get; set; } = null!;
        public string? Description { get; set; }
        public string Unit { get; set; } = "cái";
        public string? Category { get; set; }
    }

    public class UpdateProductDto
    {
        public int ProductId { get; set; }
        public string ProductCode { get; set; } = null!;
        public string ProductName { get; set; } = null!;
        public string? Description { get; set; }
        public string Unit { get; set; } = "cái";
        public string? Category { get; set; }
        public string Status { get; set; } = "Active";
    }
}
