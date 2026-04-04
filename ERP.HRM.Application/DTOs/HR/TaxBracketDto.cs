using System;

namespace ERP.HRM.Application.DTOs.HR
{
    public class TaxBracketDto
    {
        public int TaxBracketId { get; set; }
        public string BracketName { get; set; } = null!;
        public decimal MinIncome { get; set; }
        public decimal MaxIncome { get; set; }
        public decimal TaxRate { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsActive { get; set; }
    }

    public class CreateTaxBracketDto
    {
        public string BracketName { get; set; } = null!;
        public decimal MinIncome { get; set; }
        public decimal MaxIncome { get; set; }
        public decimal TaxRate { get; set; }
        public DateTime EffectiveDate { get; set; }
    }

    public class UpdateTaxBracketDto
    {
        public string BracketName { get; set; } = null!;
        public decimal MinIncome { get; set; }
        public decimal MaxIncome { get; set; }
        public decimal TaxRate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsActive { get; set; }
    }
}
