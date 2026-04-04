using System;
using System.Collections.Generic;

namespace ERP.HRM.Domain.Entities
{
    public partial class Employee : BaseEntity
    {
        public int EmployeeId { get; set; }

        public string EmployeeCode { get; set; } = null!;

        public string FullName { get; set; } = null!;

        public DateOnly? DateOfBirth { get; set; }

        public string? Gender { get; set; }

        public int? DepartmentId { get; set; }

        public int? PositionId { get; set; }

        public decimal? BaseSalary { get; set; }

        public DateOnly? HireDate { get; set; }

        public string? Status { get; set; }

        public string? NationalId { get; set; }

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

        public DateTime? LastLoginDate { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public virtual Department? Department { get; set; }

        public virtual ICollection<Employee> InverseManager { get; set; } = new List<Employee>();

        public virtual Employee? Manager { get; set; }

        public virtual Position? Position { get; set; }

        public virtual SalaryGrade? SalaryGrade { get; set; }

        public virtual ICollection<EmploymentContract> EmploymentContracts { get; set; } = new List<EmploymentContract>();

        public virtual ICollection<FamilyDependent> FamilyDependents { get; set; } = new List<FamilyDependent>();

        public virtual ICollection<SalaryAdjustmentDecision> SalaryAdjustmentDecisions { get; set; } = new List<SalaryAdjustmentDecision>();
    }
}
