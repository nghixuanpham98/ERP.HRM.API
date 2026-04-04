namespace ERP.HRM.Application.DTOs.HR
{
    public class ResignationDecisionDto
    {
        public int ResignationDecisionId { get; set; }
        public int EmployeeId { get; set; }
        public string ResignationType { get; set; } = null!;
        public DateOnly NoticeDate { get; set; }
        public DateOnly EffectiveDate { get; set; }
        public string Reason { get; set; } = null!;
        public string? DetailedReason { get; set; }
        public string Status { get; set; } = null!;
        public decimal? SettlementAmount { get; set; }
        public DateOnly? FinalPaymentDate { get; set; }
        public int? ApprovedByUserId { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }

    public class CreateResignationDecisionDto
    {
        public int EmployeeId { get; set; }
        public string ResignationType { get; set; } = null!;
        public DateOnly NoticeDate { get; set; }
        public DateOnly EffectiveDate { get; set; }
        public string Reason { get; set; } = null!;
        public string? DetailedReason { get; set; }
        public decimal? SettlementAmount { get; set; }
    }

    public class UpdateResignationDecisionDto
    {
        public string ResignationType { get; set; } = null!;
        public DateOnly NoticeDate { get; set; }
        public DateOnly EffectiveDate { get; set; }
        public string Reason { get; set; } = null!;
        public string? DetailedReason { get; set; }
        public string Status { get; set; } = null!;
        public decimal? SettlementAmount { get; set; }
        public DateOnly? FinalPaymentDate { get; set; }
    }
}
