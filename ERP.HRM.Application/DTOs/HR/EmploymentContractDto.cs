using System;

namespace ERP.HRM.Application.DTOs.HR
{
    public class EmploymentContractDto
    {
        public int EmploymentContractId { get; set; }
        public int EmployeeId { get; set; }
        public string ContractNumber { get; set; } = null!;
        public string ContractType { get; set; } = null!;
        public DateOnly StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public DateOnly? ProbationEndDate { get; set; }
        public bool IsActive { get; set; }
        public string? TerminationReason { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }

    public class CreateEmploymentContractDto
    {
        public int EmployeeId { get; set; }
        public string ContractNumber { get; set; } = null!;
        public string ContractType { get; set; } = null!;
        public DateOnly StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public DateOnly? ProbationEndDate { get; set; }
    }

    public class UpdateEmploymentContractDto
    {
        public string ContractNumber { get; set; } = null!;
        public string ContractType { get; set; } = null!;
        public DateOnly StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public DateOnly? ProbationEndDate { get; set; }
        public bool IsActive { get; set; }
        public string? TerminationReason { get; set; }
    }
}
