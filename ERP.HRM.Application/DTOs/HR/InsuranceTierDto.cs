using System;

namespace ERP.HRM.Application.DTOs.HR
{
    public class InsuranceTierDto
    {
        public int InsuranceTierId { get; set; }
        public string TierName { get; set; } = null!;
        public string InsuranceType { get; set; } = null!;
        public decimal MinSalary { get; set; }
        public decimal MaxSalary { get; set; }
        public decimal EmployeeRate { get; set; }
        public decimal EmployerRate { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsActive { get; set; }
    }

    public class CreateInsuranceTierDto
    {
        public string TierName { get; set; } = null!;
        public string InsuranceType { get; set; } = null!;
        public decimal MinSalary { get; set; }
        public decimal MaxSalary { get; set; }
        public decimal EmployeeRate { get; set; }
        public decimal EmployerRate { get; set; }
        public DateTime EffectiveDate { get; set; }
    }

    public class UpdateInsuranceTierDto
    {
        public string TierName { get; set; } = null!;
        public string InsuranceType { get; set; } = null!;
        public decimal MinSalary { get; set; }
        public decimal MaxSalary { get; set; }
        public decimal EmployeeRate { get; set; }
        public decimal EmployerRate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsActive { get; set; }
    }

    public class InsuranceCalculationResultDto
    {
        public int EmployeeId { get; set; }
        public string InsuranceType { get; set; } = null!;
        public decimal BaseSalary { get; set; }
        public decimal EmployeeContributionRate { get; set; }
        public decimal EmployerContributionRate { get; set; }
        public decimal EmployeeContributionAmount { get; set; }
        public decimal EmployerContributionAmount { get; set; }
        public decimal TotalContributionAmount { get; set; }
        public DateTime CalculatedAt { get; set; }
    }
}
