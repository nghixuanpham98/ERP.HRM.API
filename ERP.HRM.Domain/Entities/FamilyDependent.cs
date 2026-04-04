using System;

namespace ERP.HRM.Domain.Entities
{
    /// <summary>
    /// Represents a family member/dependent of an employee
    /// Used for tax deduction calculations (giảm trừ gia cảnh)
    /// </summary>
    public class FamilyDependent : BaseEntity
    {
        public int FamilyDependentId { get; set; }

        /// <summary>
        /// Reference to employee
        /// </summary>
        public int EmployeeId { get; set; }

        /// <summary>
        /// Full name of dependent
        /// </summary>
        public string FullName { get; set; } = null!;

        /// <summary>
        /// Relationship to employee: Spouse, Child, Parent, etc.
        /// </summary>
        public string Relationship { get; set; } = null!;

        /// <summary>
        /// Date of birth of dependent
        /// </summary>
        public DateOnly DateOfBirth { get; set; }

        /// <summary>
        /// Whether this dependent qualifies for tax deduction
        /// </summary>
        public bool IsQualified { get; set; } = true;

        /// <summary>
        /// Date when qualification starts
        /// </summary>
        public DateOnly? QualificationStartDate { get; set; }

        /// <summary>
        /// Date when qualification ends (e.g., when child turns 18)
        /// </summary>
        public DateOnly? QualificationEndDate { get; set; }

        /// <summary>
        /// National ID of dependent
        /// </summary>
        public string? NationalId { get; set; }

        /// <summary>
        /// Additional notes
        /// </summary>
        public string? Notes { get; set; }

        public virtual Employee Employee { get; set; } = null!;
    }
}
