using ERP.HRM.Domain.Entities;

namespace ERP.HRM.Domain.Entities
{
    /// <summary>
    /// Salary type enumeration
    /// </summary>
    public enum SalaryType
    {
        /// <summary>
        /// Monthly salary based on working days
        /// </summary>
        Monthly = 1,
        
        /// <summary>
        /// Production-based salary (unit price * quantity)
        /// </summary>
        Production = 2,
        
        /// <summary>
        /// Hourly salary
        /// </summary>
        Hourly = 3
    }
}
