using ERP.HRM.Application.DTOs.Employee;
using MediatR;

namespace ERP.HRM.Application.Features.Employees.Commands
{
    /// <summary>
    /// Command to update an existing employee
    /// </summary>
    public class UpdateEmployeeCommand : IRequest<EmployeeDto>
    {
        public int EmployeeId { get; set; }
        public string EmployeeCode { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public int? DepartmentId { get; set; }
        public int? PositionId { get; set; }
        public decimal? BaseSalary { get; set; }
        public DateTime? HireDate { get; set; }
        public string? Status { get; set; }
        public string? NationalId { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public int? ManagerId { get; set; }
        public string? JobTitle { get; set; }
        public decimal? Allowance { get; set; }
        public string? ContractType { get; set; }
        public DateTime? ContractStartDate { get; set; }
        public DateTime? ContractEndDate { get; set; }
        public DateTime? ProbationEndDate { get; set; }
    }
}
