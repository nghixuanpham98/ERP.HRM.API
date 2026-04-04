namespace ERP.HRM.Domain.Entities
{
    /// <summary>
    /// Represents detailed deductions for a payroll record
    /// </summary>
    public class PayrollDeduction : BaseEntity
    {
        public int PayrollDeductionId { get; set; }
        
        /// <summary>
        /// Reference to payroll record
        /// </summary>
        public int PayrollRecordId { get; set; }
        
        /// <summary>
        /// Type of deduction (e.g., "BHXH", "Thuế", "Vay nhân viên", "Phạt")
        /// </summary>
        public string DeductionType { get; set; } = null!;
        
        /// <summary>
        /// Description of deduction
        /// </summary>
        public string Description { get; set; } = null!;
        
        /// <summary>
        /// Amount to deduct
        /// </summary>
        public decimal Amount { get; set; }
        
        /// <summary>
        /// Reason/notes for the deduction
        /// </summary>
        public string? Reason { get; set; }

        public virtual PayrollRecord PayrollRecord { get; set; } = null!;
    }
}
