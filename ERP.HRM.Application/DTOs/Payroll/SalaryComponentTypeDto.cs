namespace ERP.HRM.Application.DTOs.Payroll
{
    public class SalaryComponentTypeDto
    {
        public int SalaryComponentTypeId { get; set; }
        public string ComponentName { get; set; } = null!;
        public string ComponentCode { get; set; } = null!;
        public string? Description { get; set; }
        public string ComponentType { get; set; } = null!;
        public bool IsFixed { get; set; }
        public bool IsTaxableIncome { get; set; }
        public bool IsIncludedInInsurance { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
    }

    public class CreateSalaryComponentTypeDto
    {
        public string ComponentName { get; set; } = null!;
        public string ComponentCode { get; set; } = null!;
        public string? Description { get; set; }
        public string ComponentType { get; set; } = null!;
        public bool IsFixed { get; set; } = true;
        public bool IsTaxableIncome { get; set; } = true;
        public bool IsIncludedInInsurance { get; set; } = true;
        public int DisplayOrder { get; set; }
    }

    public class UpdateSalaryComponentTypeDto
    {
        public int SalaryComponentTypeId { get; set; }
        public string? ComponentName { get; set; }
        public string? Description { get; set; }
        public bool? IsActive { get; set; }
        public int? DisplayOrder { get; set; }
    }
}
