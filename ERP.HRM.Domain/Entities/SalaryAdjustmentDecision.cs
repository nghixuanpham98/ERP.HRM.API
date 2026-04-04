using System;

namespace ERP.HRM.Domain.Entities
{
    /// <summary>
    /// Represents a salary adjustment decision for an employee
    /// Tracks history of salary changes with approval workflow
    /// </summary>
    public class SalaryAdjustmentDecision : BaseEntity
    {
        public int SalaryAdjustmentDecisionId { get; set; }

        /// <summary>
        /// Reference to employee
        /// </summary>
        public int EmployeeId { get; set; }

        /// <summary>
        /// User who created this decision
        /// </summary>
        public int CreatedByUserId { get; set; }

        /// <summary>
        /// User who approved this decision (null if not approved)
        /// </summary>
        public int? ApprovedByUserId { get; set; }

        /// <summary>
        /// Type of decision: "Increase", "Decrease", "Promotion", "Demotion"
        /// </summary>
        public string DecisionType { get; set; } = null!;

        /// <summary>
        /// Old base salary amount
        /// </summary>
        public decimal OldBaseSalary { get; set; }

        /// <summary>
        /// New base salary amount
        /// </summary>
        public decimal NewBaseSalary { get; set; }

        /// <summary>
        /// Change amount (NewBaseSalary - OldBaseSalary)
        /// </summary>
        public decimal SalaryChange { get; set; }

        /// <summary>
        /// Effective date for the new salary
        /// </summary>
        public DateOnly EffectiveDate { get; set; }

        /// <summary>
        /// Reason for the adjustment
        /// </summary>
        public string Reason { get; set; } = null!;

        /// <summary>
        /// Status: "Pending", "Approved", "Rejected", "Applied"
        /// </summary>
        public string Status { get; set; } = "Pending";

        /// <summary>
        /// When the decision was made
        /// </summary>
        public DateTime DecisionDate { get; set; }

        /// <summary>
        /// When the decision was approved (if applicable)
        /// </summary>
        public DateTime? ApprovedDate { get; set; }

        /// <summary>
        /// When the decision was effectively implemented
        /// </summary>
        public DateTime? EffectiveImplementationDate { get; set; }

        /// <summary>
        /// Approver's notes
        /// </summary>
        public string? ApprovalNotes { get; set; }

        public virtual Employee Employee { get; set; } = null!;
    }
}
