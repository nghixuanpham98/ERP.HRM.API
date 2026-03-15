using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.HRM.Application.DTOs
{
    public class DepartmentDto
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }
        public string DepartmentCode { get; set; }
        public int? ParentDepartmentId { get; set; }
        public int? HeadOfDepartmentId { get; set; }

        // Thông tin tổng hợp
        public int EmployeeCount { get; set; }
    }
}
