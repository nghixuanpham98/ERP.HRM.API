using System;

namespace ERP.HRM.Application.DTOs.HR
{
    public class SalaryAdjustmentDecisionDto
    {
        public int SalaryAdjustmentDecisionId { get; set; }
        public int EmployeeId { get; set; }
        public int CreatedByUserId { get; set; }
        public int? ApprovedByUserId { get; set; }
        public string DecisionType { get; set; } = null!;
        public decimal OldBaseSalary { get; set; }
        public decimal NewBaseSalary { get; set; }
        public decimal SalaryChange { get; set; }
        public DateOnly EffectiveDate { get; set; }
        public string Reason { get; set; } = null!;
        public string Status { get; set; } = null!;
        public DateTime DecisionDate { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public DateTime? EffectiveImplementationDate { get; set; }
        public string? ApprovalNotes { get; set; }
    }

    public class CreateSalaryAdjustmentDecisionDto
    {
        public int EmployeeId { get; set; }
        public string DecisionType { get; set; } = null!;
        public decimal OldBaseSalary { get; set; }
        public decimal NewBaseSalary { get; set; }
        public DateOnly EffectiveDate { get; set; }
        public string Reason { get; set; } = null!;
    }

    public class ApproveSalaryAdjustmentDecisionDto
    {
        public int ApprovedByUserId { get; set; }
        public string Status { get; set; } = null!; // "Approved" or "Rejected"
        public string? ApprovalNotes { get; set; }
    }

    public class UpdateSalaryAdjustmentDecisionDto
    {
        public string DecisionType { get; set; } = null!;
        public decimal OldBaseSalary { get; set; }
        public decimal NewBaseSalary { get; set; }
        public DateOnly EffectiveDate { get; set; }
        public string Reason { get; set; } = null!;
    }
}
