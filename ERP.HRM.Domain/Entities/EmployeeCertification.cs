namespace ERP.HRM.Domain.Entities
{
    /// <summary>
    /// Tracks certifications and qualifications held by employees
    /// </summary>
    public class EmployeeCertification : BaseEntity
    {
        public int EmployeeCertificationId { get; set; }

        /// <summary>
        /// Reference to employee
        /// </summary>
        public int EmployeeId { get; set; }

        /// <summary>
        /// Certification name
        /// </summary>
        public string CertificationName { get; set; } = null!;

        /// <summary>
        /// Certification code/reference
        /// </summary>
        public string? CertificationCode { get; set; }

        /// <summary>
        /// Organization that issued the certification
        /// </summary>
        public string? IssuedBy { get; set; }

        /// <summary>
        /// Issue date
        /// </summary>
        public DateOnly IssueDate { get; set; }

        /// <summary>
        /// Expiry date (null if no expiry)
        /// </summary>
        public DateOnly? ExpiryDate { get; set; }

        /// <summary>
        /// Certification document URL
        /// </summary>
        public string? DocumentUrl { get; set; }

        /// <summary>
        /// Status: Active, Expired, Revoked, Suspended
        /// </summary>
        public string Status { get; set; } = "Active";

        /// <summary>
        /// Is this certification required for current position
        /// </summary>
        public bool IsRequired { get; set; } = false;

        /// <summary>
        /// Renewal reminder date
        /// </summary>
        public DateTime? RenewalReminderDate { get; set; }

        /// <summary>
        /// Notes/remarks
        /// </summary>
        public string? Notes { get; set; }

        public virtual Employee? Employee { get; set; }
    }
}
