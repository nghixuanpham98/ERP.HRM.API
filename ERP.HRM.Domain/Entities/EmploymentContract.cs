using System;

namespace ERP.HRM.Domain.Entities
{
    /// <summary>
    /// Represents an employment contract for an employee
    /// Tracks contract history and details
    /// </summary>
    public class EmploymentContract : BaseEntity
    {
        public int EmploymentContractId { get; set; }

        /// <summary>
        /// Reference to employee
        /// </summary>
        public int EmployeeId { get; set; }

        /// <summary>
        /// Unique contract number
        /// </summary>
        public string ContractNumber { get; set; } = null!;

        /// <summary>
        /// Type of contract: Full-time, Part-time, Contract, Seasonal, etc.
        /// </summary>
        public string ContractType { get; set; } = null!;

        /// <summary>
        /// Contract start date
        /// </summary>
        public DateOnly StartDate { get; set; }

        /// <summary>
        /// Contract end date (null for indefinite)
        /// </summary>
        public DateOnly? EndDate { get; set; }

        /// <summary>
        /// Probation end date
        /// </summary>
        public DateOnly? ProbationEndDate { get; set; }

        /// <summary>
        /// Whether this contract is currently active
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Reason for contract termination
        /// </summary>
        public string? TerminationReason { get; set; }

        /// <summary>
        /// When this record was created
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// When this record was last modified
        /// </summary>
        public DateTime? ModifiedAt { get; set; }

        public virtual Employee Employee { get; set; } = null!;
    }
}
