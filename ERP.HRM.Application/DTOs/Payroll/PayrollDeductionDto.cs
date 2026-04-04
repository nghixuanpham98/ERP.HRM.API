namespace ERP.HRM.Application.DTOs.Payroll
{
    /// <summary>
    /// DTO for PayrollDeduction entity
    /// </summary>
    public class PayrollDeductionDto
    {
        /// <summary>
        /// Unique identifier for the deduction
        /// </summary>
        public int PayrollDeductionId { get; set; }

        /// <summary>
        /// Reference to the payroll record
        /// </summary>
        public int PayrollRecordId { get; set; }

        /// <summary>
        /// Type of deduction (BHXH, Thuế, Vay nhân viên, Phạt, etc.)
        /// </summary>
        public string DeductionType { get; set; }

        /// <summary>
        /// Description of the deduction
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Amount of the deduction
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Reason for the deduction
        /// </summary>
        public string? Reason { get; set; }
    }
}
