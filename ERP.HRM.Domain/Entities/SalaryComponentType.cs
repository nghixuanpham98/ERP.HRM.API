namespace ERP.HRM.Domain.Entities
{
    /// <summary>
    /// Defines salary component types and their properties
    /// Examples: BaseSalary, HousingAllowance, TransportAllowance, etc.
    /// </summary>
    public class SalaryComponentType : BaseEntity
    {
        public int SalaryComponentTypeId { get; set; }

        /// <summary>
        /// Component name
        /// </summary>
        public string ComponentName { get; set; } = null!;

        /// <summary>
        /// Component code (e.g., "BASE", "HOUSING", "TRANSPORT")
        /// </summary>
        public string ComponentCode { get; set; } = null!;

        /// <summary>
        /// Description of the component
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Type: Income, Allowance, Bonus, Deduction
        /// </summary>
        public string ComponentType { get; set; } = null!;

        /// <summary>
        /// Is this a fixed amount or percentage-based
        /// </summary>
        public bool IsFixed { get; set; } = true;

        /// <summary>
        /// Is this amount included in taxable income
        /// </summary>
        public bool IsTaxableIncome { get; set; } = true;

        /// <summary>
        /// Is this included in insurance calculation
        /// </summary>
        public bool IsIncludedInInsurance { get; set; } = true;

        /// <summary>
        /// Display order in payslip
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// Is this component active
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Configuration JSON (for complex rules)
        /// </summary>
        public string? ConfigurationJson { get; set; }

        public virtual ICollection<SalaryComponentValue> SalaryComponentValues { get; set; } = new List<SalaryComponentValue>();
    }
}
