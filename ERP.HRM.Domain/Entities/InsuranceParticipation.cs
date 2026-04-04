using System;

namespace ERP.HRM.Domain.Entities
{
    /// <summary>
    /// Represents insurance participation history
    /// Quản lý quá trình tham gia bảo hiểm
    /// </summary>
    public class InsuranceParticipation : BaseEntity
    {
        public int InsuranceParticipationId { get; set; }

        /// <summary>
        /// Reference to employee
        /// </summary>
        public int EmployeeId { get; set; }

        /// <summary>
        /// Insurance type: Health, Unemployment, WorkInjury
        /// </summary>
        public string InsuranceType { get; set; } = null!;

        /// <summary>
        /// Insurance participation number
        /// </summary>
        public string InsuranceNumber { get; set; } = null!;

        /// <summary>
        /// Start date of participation
        /// </summary>
        public DateOnly StartDate { get; set; }

        /// <summary>
        /// End date of participation (if applicable)
        /// </summary>
        public DateOnly? EndDate { get; set; }

        /// <summary>
        /// Participation status: Active, Suspended, Terminated
        /// </summary>
        public string Status { get; set; } = "Active";

        /// <summary>
        /// Contribution base salary
        /// </summary>
        public decimal ContributionBaseSalary { get; set; }

        /// <summary>
        /// Employee contribution rate (%)
        /// </summary>
        public decimal EmployeeContributionRate { get; set; }

        /// <summary>
        /// Employer contribution rate (%)
        /// </summary>
        public decimal EmployerContributionRate { get; set; }

        /// <summary>
        /// Remarks
        /// </summary>
        public string? Remarks { get; set; }

        public virtual Employee Employee { get; set; } = null!;
    }
}
