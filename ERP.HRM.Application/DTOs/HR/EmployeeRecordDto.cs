namespace ERP.HRM.Application.DTOs.HR
{
    public class EmployeeRecordDto
    {
        public int EmployeeRecordId { get; set; }
        public int EmployeeId { get; set; }
        public string RecordNumber { get; set; } = null!;
        public string FilePath { get; set; } = null!;
        public string DocumentType { get; set; } = null!;
        public DateTime IssueDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string IssuingOrganization { get; set; } = null!;
        public string Status { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }

    public class CreateEmployeeRecordDto
    {
        public int EmployeeId { get; set; }
        public string RecordNumber { get; set; } = null!;
        public string FilePath { get; set; } = null!;
        public string DocumentType { get; set; } = null!;
        public DateTime IssueDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string IssuingOrganization { get; set; } = null!;
    }

    public class UpdateEmployeeRecordDto
    {
        public string RecordNumber { get; set; } = null!;
        public string FilePath { get; set; } = null!;
        public string DocumentType { get; set; } = null!;
        public DateTime IssueDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string IssuingOrganization { get; set; } = null!;
        public string Status { get; set; } = null!;
    }
}
