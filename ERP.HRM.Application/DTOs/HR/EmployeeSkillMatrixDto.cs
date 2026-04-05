namespace ERP.HRM.Application.DTOs.HR
{
    public class EmployeeSkillMatrixDto
    {
        public int EmployeeSkillMatrixId { get; set; }
        public int EmployeeId { get; set; }
        public string SkillName { get; set; } = null!;
        public string? SkillCategory { get; set; }
        public int Level { get; set; }
        public decimal YearsOfExperience { get; set; }
        public bool IsRequired { get; set; }
        public DateTime LastAssessmentDate { get; set; }
        public DateTime? NextAssessmentDueDate { get; set; }
        public string? AssessorName { get; set; }
        public decimal? AssessmentScore { get; set; }
        public bool IsVerified { get; set; }
    }

    public class CreateEmployeeSkillMatrixDto
    {
        public int EmployeeId { get; set; }
        public string SkillName { get; set; } = null!;
        public string? SkillCategory { get; set; }
        public int Level { get; set; }
        public decimal YearsOfExperience { get; set; }
        public bool IsRequired { get; set; }
        public DateTime? LastAssessmentDate { get; set; }
        public DateTime? NextAssessmentDueDate { get; set; }
        public string? AssessorName { get; set; }
        public decimal? AssessmentScore { get; set; }
    }

    public class UpdateEmployeeSkillMatrixDto
    {
        public int EmployeeSkillMatrixId { get; set; }
        public int? Level { get; set; }
        public decimal? YearsOfExperience { get; set; }
        public DateTime? LastAssessmentDate { get; set; }
        public DateTime? NextAssessmentDueDate { get; set; }
        public string? AssessorName { get; set; }
        public decimal? AssessmentScore { get; set; }
        public bool? IsVerified { get; set; }
    }
}
