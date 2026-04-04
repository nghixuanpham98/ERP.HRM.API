namespace ERP.HRM.Domain.Entities
{
    /// <summary>
    /// Represents production output for a worker
    /// Used for production-based salary calculation
    /// </summary>
    public class ProductionOutput : BaseEntity
    {
        public int ProductionOutputId { get; set; }
        
        /// <summary>
        /// Reference to employee (worker)
        /// </summary>
        public int EmployeeId { get; set; }
        
        /// <summary>
        /// Reference to payroll period
        /// </summary>
        public int PayrollPeriodId { get; set; }
        
        /// <summary>
        /// Reference to product
        /// </summary>
        public int ProductId { get; set; }
        
        /// <summary>
        /// Quantity produced (sản lượng)
        /// </summary>
        public decimal Quantity { get; set; }
        
        /// <summary>
        /// Unit price at time of production (đơn giá)
        /// </summary>
        public decimal UnitPrice { get; set; }
        
        /// <summary>
        /// Production date
        /// </summary>
        public DateTime ProductionDate { get; set; }
        
        /// <summary>
        /// Quality status (e.g., "OK", "Defective", "Rework")
        /// </summary>
        public string QualityStatus { get; set; } = "OK";
        
        /// <summary>
        /// Notes about the production
        /// </summary>
        public string? Notes { get; set; }
        
        /// <summary>
        /// Calculated amount (Quantity * UnitPrice)
        /// </summary>
        public decimal Amount { get; set; }

        public virtual Employee Employee { get; set; } = null!;
        public virtual PayrollPeriod PayrollPeriod { get; set; } = null!;
        public virtual Product Product { get; set; } = null!;
    }
}
