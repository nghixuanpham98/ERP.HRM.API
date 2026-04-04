using System;

namespace ERP.HRM.Domain.Entities
{
    /// <summary>
    /// Represents employee records/profile details
    /// Quản lý hồ sơ cán bộ nhân viên
    /// </summary>
    public class EmployeeRecord : BaseEntity
    {
        public int EmployeeRecordId { get; set; }

        /// <summary>
        /// Reference to employee
        /// </summary>
        public int EmployeeId { get; set; }

        /// <summary>
        /// Record number/code
        /// </summary>
        public string RecordNumber { get; set; } = null!;

        /// <summary>
        /// File location/path
        /// </summary>
        public string FilePath { get; set; } = null!;

        /// <summary>
        /// Document type: Identity, Certificate, Contract, Medical, etc.
        /// </summary>
        public string DocumentType { get; set; } = null!;

        /// <summary>
        /// Issue date
        /// </summary>
        public DateTime IssueDate { get; set; }

        /// <summary>
        /// Expiry date (if applicable)
        /// </summary>
        public DateTime? ExpiryDate { get; set; }

        /// <summary>
        /// Issuing organization
        /// </summary>
        public string? IssuingOrganization { get; set; }

        /// <summary>
        /// Description/notes
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Status: Active, Expired, Archived
        /// </summary>
        public string Status { get; set; } = "Active";

        /// <summary>
        /// Upload date
        /// </summary>
        public DateTime UploadDate { get; set; }

        public virtual Employee Employee { get; set; } = null!;
    }
}
