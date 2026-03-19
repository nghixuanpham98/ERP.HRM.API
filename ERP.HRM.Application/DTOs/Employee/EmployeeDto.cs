using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.HRM.Application.DTOs.Employee
{
    public class EmployeeDto
    {
        public int EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string FullName { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public DateOnly? HireDate { get; set; }
        public decimal? BaseSalary { get; set; }
        public decimal? Allowance { get; set; }
        public string? JobTitle { get; set; }
        public string? ContractType { get; set; }
        public DateOnly? ContractStartDate { get; set; }
        public DateOnly? ContractEndDate { get; set; }
        public string? Status { get; set; }

        // Thông tin liên quan
        public string? DepartmentName { get; set; }
        public string? PositionName { get; set; }
        public string? ManagerName { get; set; }
    }
}