namespace ERP.HRM.Domain.Entities
{
    /// <summary>
    /// Represents an employee's salary configuration
    /// Defines which salary type and calculation method applies to the employee
    /// </summary>
    public class SalaryConfiguration : BaseEntity
    {
        public int SalaryConfigurationId { get; set; }
        public int EmployeeId { get; set; }
        
        /// <summary>
        /// Type of salary calculation (Monthly, Production, Hourly)
        /// </summary>
        public SalaryType SalaryType { get; set; }
        
        /// <summary>
        /// Base salary for monthly employees (không bao gồm phụ cấp)
        /// </summary>
        public decimal BaseSalary { get; set; }
        
        /// <summary>
        /// Unit price per product for production workers (đơn giá sản phẩm)
        /// </summary>
        public decimal? UnitPrice { get; set; }
        
        /// <summary>
        /// Hourly rate for hourly employees
        /// </summary>
        public decimal? HourlyRate { get; set; }
        
        /// <summary>
        /// Allowance/bonus amount (phụ cấp)
        /// </summary>
        public decimal? Allowance { get; set; }
        
        /// <summary>
        /// Insurance contribution rate (%)
        /// </summary>
        public decimal? InsuranceRate { get; set; }
        
        /// <summary>
        /// Tax rate (%)
        /// </summary>
        public decimal? TaxRate { get; set; }
        
        /// <summary>
        /// Effective from date
        /// </summary>
        public DateTime EffectiveFrom { get; set; }
        
        /// <summary>
        /// Effective to date (null means ongoing)
        /// </summary>
        public DateTime? EffectiveTo { get; set; }
        
        /// <summary>
        /// Is this configuration currently active
        /// </summary>
        public bool IsActive { get; set; } = true;

        public virtual Employee Employee { get; set; } = null!;
    }
}
