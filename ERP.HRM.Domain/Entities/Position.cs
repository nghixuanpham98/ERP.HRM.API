using System;
using System.Collections.Generic;

namespace ERP.HRM.Domain.Entities
{
    public partial class Position : BaseEntity
    {
        public int PositionId { get; set; }

        public string PositionName { get; set; } = null!;

        public decimal? Allowance { get; set; }

        public string? Status { get; set; }

        public string PositionCode { get; set; } = null!;

        public int? Level { get; set; }

        public string? Description { get; set; }

        public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}
