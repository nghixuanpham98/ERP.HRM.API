using System;

namespace ERP.HRM.Application.DTOs.HR
{
    public class SalaryGradeDto
    {
        public int SalaryGradeId { get; set; }
        public string GradeName { get; set; } = null!;
        public int GradeLevel { get; set; }
        public decimal MinSalary { get; set; }
        public decimal MidSalary { get; set; }
        public decimal MaxSalary { get; set; }
        public bool IsActive { get; set; }
        public string? Description { get; set; }
        public DateTime EffectiveDate { get; set; }
    }

    public class CreateSalaryGradeDto
    {
        public string GradeName { get; set; } = null!;
        public int GradeLevel { get; set; }
        public decimal MinSalary { get; set; }
        public decimal MidSalary { get; set; }
        public decimal MaxSalary { get; set; }
        public string? Description { get; set; }
        public DateTime EffectiveDate { get; set; }
    }

    public class UpdateSalaryGradeDto
    {
        public string GradeName { get; set; } = null!;
        public int GradeLevel { get; set; }
        public decimal MinSalary { get; set; }
        public decimal MidSalary { get; set; }
        public decimal MaxSalary { get; set; }
        public bool IsActive { get; set; }
        public string? Description { get; set; }
    }
}
