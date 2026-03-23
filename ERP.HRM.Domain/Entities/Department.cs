using System;
using System.Collections.Generic;

namespace ERP.HRM.Domain.Entities 
{
    public partial class Department
{
    public int DepartmentId { get; set; }

    public string DepartmentName { get; set; } = null!;

    public string? Description { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public string? ModifiedBy { get; set; }

    public string DepartmentCode { get; set; } = null!;

    public int? ParentDepartmentId { get; set; }

    public int? HeadOfDepartmentId { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
}
