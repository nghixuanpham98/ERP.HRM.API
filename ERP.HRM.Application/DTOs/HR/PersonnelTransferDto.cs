namespace ERP.HRM.Application.DTOs.HR
{
    public class PersonnelTransferDto
    {
        public int PersonnelTransferId { get; set; }
        public int EmployeeId { get; set; }
        public int? FromDepartmentId { get; set; }
        public int? ToDepartmentId { get; set; }
        public int? FromPositionId { get; set; }
        public int? ToPositionId { get; set; }
        public string TransferType { get; set; } = null!;
        public decimal? OldSalary { get; set; }
        public decimal? NewSalary { get; set; }
        public DateOnly EffectiveDate { get; set; }
        public string ApprovalStatus { get; set; } = null!;
        public int? ApprovedByUserId { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }

    public class CreatePersonnelTransferDto
    {
        public int EmployeeId { get; set; }
        public int? FromDepartmentId { get; set; }
        public int? ToDepartmentId { get; set; }
        public int? FromPositionId { get; set; }
        public int? ToPositionId { get; set; }
        public string TransferType { get; set; } = null!;
        public decimal? OldSalary { get; set; }
        public decimal? NewSalary { get; set; }
        public DateOnly EffectiveDate { get; set; }
    }

    public class UpdatePersonnelTransferDto
    {
        public int? FromDepartmentId { get; set; }
        public int? ToDepartmentId { get; set; }
        public int? FromPositionId { get; set; }
        public int? ToPositionId { get; set; }
        public string TransferType { get; set; } = null!;
        public decimal? OldSalary { get; set; }
        public decimal? NewSalary { get; set; }
        public DateOnly EffectiveDate { get; set; }
        public string ApprovalStatus { get; set; } = null!;
    }
}
