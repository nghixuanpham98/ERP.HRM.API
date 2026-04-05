namespace ERP.HRM.Domain.Entities
{
    /// <summary>
    /// Maintains skills and competency matrix for employees
    /// Tracks skill levels and assessment dates
    /// </summary>
    public class EmployeeSkillMatrix : BaseEntity
    {
        public int EmployeeSkillMatrixId { get; set; }

        /// <summary>
        /// Reference to employee
        /// </summary>
        public int EmployeeId { get; set; }

        /// <summary>
        /// Skill name
        /// </summary>
        public string SkillName { get; set; } = null!;

        /// <summary>
        /// Skill category (Technical, Language, Soft Skills, etc.)
        /// </summary>
        public string? SkillCategory { get; set; }

        /// <summary>
        /// Proficiency level: 1 (Beginner), 2 (Intermediate), 3 (Advanced), 4 (Expert), 5 (Master)
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// Years of experience with this skill
        /// </summary>
        public decimal YearsOfExperience { get; set; }

        /// <summary>
        /// Is this skill required for the current position
        /// </summary>
        public bool IsRequired { get; set; } = false;

        /// <summary>
        /// Last assessment date
        /// </summary>
        public DateTime LastAssessmentDate { get; set; }

        /// <summary>
        /// Next assessment due date
        /// </summary>
        public DateTime? NextAssessmentDueDate { get; set; }

        /// <summary>
        /// Assessor/Evaluator name
        /// </summary>
        public string? AssessorName { get; set; }

        /// <summary>
        /// Assessment score/rating
        /// </summary>
        public decimal? AssessmentScore { get; set; }

        /// <summary>
        /// Notes on skill assessment
        /// </summary>
        public string? AssessmentNotes { get; set; }

        /// <summary>
        /// Recommendation for improvement
        /// </summary>
        public string? Recommendation { get; set; }

        /// <summary>
        /// Is this skill verified/certified
        /// </summary>
        public bool IsVerified { get; set; } = false;

        /// <summary>
        /// Related certification if any
        /// </summary>
        public int? CertificationId { get; set; }

        public virtual Employee? Employee { get; set; }
        public virtual EmployeeCertification? Certification { get; set; }
    }
}
