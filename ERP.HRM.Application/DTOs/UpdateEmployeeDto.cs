using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.HRM.Application.DTOs
{
    public class UpdateEmployeeDto
    {
        public string FullName { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public int? DepartmentId { get; set; }
        public int? PositionId { get; set; }
        public decimal? BaseSalary { get; set; }
        public DateOnly? HireDate { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public int? ManagerId { get; set; }
        public string? JobTitle { get; set; }
        public decimal? Allowance { get; set; }
        public string? ContractType { get; set; }
        public DateOnly? ContractStartDate { get; set; }
        public DateOnly? ContractEndDate { get; set; }
        public DateOnly? ProbationEndDate { get; set; }
        public string? Status { get; set; }
    }
}