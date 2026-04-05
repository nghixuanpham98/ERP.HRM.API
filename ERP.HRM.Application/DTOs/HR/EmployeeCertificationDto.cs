namespace ERP.HRM.Application.DTOs.HR
{
    public class EmployeeCertificationDto
    {
        public int EmployeeCertificationId { get; set; }
        public int EmployeeId { get; set; }
        public string CertificationName { get; set; } = null!;
        public string? CertificationCode { get; set; }
        public string? IssuedBy { get; set; }
        public DateOnly IssueDate { get; set; }
        public DateOnly? ExpiryDate { get; set; }
        public string? DocumentUrl { get; set; }
        public string Status { get; set; } = null!;
        public bool IsRequired { get; set; }
        public DateTime? RenewalReminderDate { get; set; }
    }

    public class CreateEmployeeCertificationDto
    {
        public int EmployeeId { get; set; }
        public string CertificationName { get; set; } = null!;
        public string? CertificationCode { get; set; }
        public string? IssuedBy { get; set; }
        public DateOnly IssueDate { get; set; }
        public DateOnly? ExpiryDate { get; set; }
        public string? DocumentUrl { get; set; }
        public string Status { get; set; } = "Active";
        public bool IsRequired { get; set; }
    }

    public class UpdateEmployeeCertificationDto
    {
        public int EmployeeCertificationId { get; set; }
        public string? CertificationName { get; set; }
        public string? Status { get; set; }
        public DateOnly? ExpiryDate { get; set; }
    }
}
