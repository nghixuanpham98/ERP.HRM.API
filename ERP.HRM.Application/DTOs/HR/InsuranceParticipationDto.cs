namespace ERP.HRM.Application.DTOs.HR
{
    public class InsuranceParticipationDto
    {
        public int InsuranceParticipationId { get; set; }
        public int EmployeeId { get; set; }
        public string InsuranceType { get; set; } = null!;
        public string InsuranceNumber { get; set; } = null!;
        public DateOnly StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public string Status { get; set; } = null!;
        public decimal ContributionBaseSalary { get; set; }
        public decimal EmployeeContributionRate { get; set; }
        public decimal EmployerContributionRate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }

    public class CreateInsuranceParticipationDto
    {
        public int EmployeeId { get; set; }
        public string InsuranceType { get; set; } = null!;
        public string InsuranceNumber { get; set; } = null!;
        public DateOnly StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public decimal ContributionBaseSalary { get; set; }
        public decimal EmployeeContributionRate { get; set; }
        public decimal EmployerContributionRate { get; set; }
    }

    public class UpdateInsuranceParticipationDto
    {
        public string InsuranceNumber { get; set; } = null!;
        public DateOnly StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public string Status { get; set; } = null!;
        public decimal ContributionBaseSalary { get; set; }
        public decimal EmployeeContributionRate { get; set; }
        public decimal EmployerContributionRate { get; set; }
    }
}
