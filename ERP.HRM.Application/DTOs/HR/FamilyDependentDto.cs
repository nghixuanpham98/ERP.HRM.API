using System;

namespace ERP.HRM.Application.DTOs.HR
{
    public class FamilyDependentDto
    {
        public int FamilyDependentId { get; set; }
        public int EmployeeId { get; set; }
        public string FullName { get; set; } = null!;
        public string Relationship { get; set; } = null!;
        public DateOnly DateOfBirth { get; set; }
        public bool IsQualified { get; set; }
        public DateOnly? QualificationStartDate { get; set; }
        public DateOnly? QualificationEndDate { get; set; }
        public string? NationalId { get; set; }
        public string? Notes { get; set; }
    }

    public class CreateFamilyDependentDto
    {
        public int EmployeeId { get; set; }
        public string FullName { get; set; } = null!;
        public string Relationship { get; set; } = null!;
        public DateOnly DateOfBirth { get; set; }
        public DateOnly? QualificationStartDate { get; set; }
        public DateOnly? QualificationEndDate { get; set; }
        public string? NationalId { get; set; }
        public string? Notes { get; set; }
    }

    public class UpdateFamilyDependentDto
    {
        public string FullName { get; set; } = null!;
        public string Relationship { get; set; } = null!;
        public DateOnly DateOfBirth { get; set; }
        public bool IsQualified { get; set; }
        public DateOnly? QualificationStartDate { get; set; }
        public DateOnly? QualificationEndDate { get; set; }
        public string? NationalId { get; set; }
        public string? Notes { get; set; }
    }
}
