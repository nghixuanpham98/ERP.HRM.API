using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.HRM.Application.DTOs
{
    public class UpdateEmployeeDto
    {
        public int EmployeeId { get; set; }
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public int PositionId { get; set; }
        public int DepartmentId { get; set; }
        public DateOnly? HireDate { get; set; }
        public decimal? Salary { get; set; }
    }
}