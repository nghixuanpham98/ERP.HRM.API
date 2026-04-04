namespace ERP.HRM.Domain.Entities
{
    /// <summary>
    /// Represents a product for production-based salary calculation
    /// </summary>
    public class Product : BaseEntity
    {
        public int ProductId { get; set; }
        
        /// <summary>
        /// Product code (e.g., "PROD001")
        /// </summary>
        public string ProductCode { get; set; } = null!;
        
        /// <summary>
        /// Product name
        /// </summary>
        public string ProductName { get; set; } = null!;
        
        /// <summary>
        /// Product description
        /// </summary>
        public string? Description { get; set; }
        
        /// <summary>
        /// Unit (e.g., "cái", "bộ", "chiếc")
        /// </summary>
        public string Unit { get; set; } = "cái";
        
        /// <summary>
        /// Category (e.g., "Electronics", "Mechanical")
        /// </summary>
        public string? Category { get; set; }
        
        /// <summary>
        /// Status (Active, Inactive)
        /// </summary>
        public string Status { get; set; } = "Active";

        public virtual ICollection<ProductionOutput> ProductionOutputs { get; set; } = new List<ProductionOutput>();
    }
}
