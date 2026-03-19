using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.HRM.Application.DTOs.Department
{
    public class CreateDepartmentDto
    {
        public string DepartmentName { get; set; }
        public string? Description { get; set; }
        public string DepartmentCode { get; set; }
        public int? ParentDepartmentId { get; set; }
        public int? HeadOfDepartmentId { get; set; }
    }
}
