using System;
using System.Collections.Generic;

namespace ERP.HRM.Domain.Entities
{
    /// <summary>
    /// Represents a salary grade/level for employees
    /// Defines salary bands for each grade
    /// </summary>
    public class SalaryGrade : BaseEntity
    {
        public int SalaryGradeId { get; set; }

        /// <summary>
        /// Grade name: "Level 1", "Level 2", "Senior", "Manager", etc.
        /// </summary>
        public string GradeName { get; set; } = null!;

        /// <summary>
        /// Grade level for sorting (1, 2, 3...)
        /// </summary>
        public int GradeLevel { get; set; }

        /// <summary>
        /// Minimum salary for this grade
        /// </summary>
        public decimal MinSalary { get; set; }

        /// <summary>
        /// Mid-point salary for this grade
        /// </summary>
        public decimal MidSalary { get; set; }

        /// <summary>
        /// Maximum salary for this grade
        /// </summary>
        public decimal MaxSalary { get; set; }

        /// <summary>
        /// Whether this grade is active
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Description of the grade
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Effective date for this grade definition
        /// </summary>
        public DateTime EffectiveDate { get; set; }

        public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}
