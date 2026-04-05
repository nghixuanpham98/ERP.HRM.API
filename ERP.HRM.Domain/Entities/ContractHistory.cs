namespace ERP.HRM.Domain.Entities
{
    /// <summary>
    /// Tracks contract history and status changes
    /// Audit trail for contract modifications
    /// </summary>
    public class ContractHistory : BaseEntity
    {
        public int ContractHistoryId { get; set; }

        /// <summary>
        /// Reference to employment contract
        /// </summary>
        public int EmploymentContractId { get; set; }

        /// <summary>
        /// Field name that was changed
        /// </summary>
        public string FieldName { get; set; } = null!;

        /// <summary>
        /// Old value before change
        /// </summary>
        public string? OldValue { get; set; }

        /// <summary>
        /// New value after change
        /// </summary>
        public string? NewValue { get; set; }

        /// <summary>
        /// User ID who made the change
        /// </summary>
        public Guid? ModifiedByUserId { get; set; }

        /// <summary>
        /// Timestamp of change
        /// </summary>
        public DateTime ModifiedDate { get; set; }

        /// <summary>
        /// Action performed: Created, Updated, StatusChanged, Approved, Rejected
        /// </summary>
        public string Action { get; set; } = null!;

        /// <summary>
        /// Additional notes/context
        /// </summary>
        public string? Notes { get; set; }

        public virtual EmploymentContract? EmploymentContract { get; set; }
    }
}
