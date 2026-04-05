namespace ERP.HRM.Domain.Entities
{
    /// <summary>
    /// Tracks training programs completed by employees
    /// </summary>
    public class EmployeeTraining : BaseEntity
    {
        public int EmployeeTrainingId { get; set; }

        /// <summary>
        /// Reference to employee
        /// </summary>
        public int EmployeeId { get; set; }

        /// <summary>
        /// Training program name
        /// </summary>
        public string TrainingName { get; set; } = null!;

        /// <summary>
        /// Training provider/institution
        /// </summary>
        public string? Provider { get; set; }

        /// <summary>
        /// Training category (Technical, Soft Skills, Compliance, etc.)
        /// </summary>
        public string? Category { get; set; }

        /// <summary>
        /// Duration in hours
        /// </summary>
        public decimal DurationHours { get; set; }

        /// <summary>
        /// Training start date
        /// </summary>
        public DateOnly StartDate { get; set; }

        /// <summary>
        /// Training completion date
        /// </summary>
        public DateOnly? CompletionDate { get; set; }

        /// <summary>
        /// Grade/score obtained
        /// </summary>
        public decimal? GradeObtained { get; set; }

        /// <summary>
        /// Maximum possible grade/score
        /// </summary>
        public decimal? MaxGrade { get; set; }

        /// <summary>
        /// Certificate obtained (yes/no)
        /// </summary>
        public bool CertificateObtained { get; set; } = false;

        /// <summary>
        /// Certificate document URL
        /// </summary>
        public string? CertificateUrl { get; set; }

        /// <summary>
        /// Cost of training
        /// </summary>
        public decimal? TrainingCost { get; set; }

        /// <summary>
        /// Status: Scheduled, InProgress, Completed, Cancelled, Failed
        /// </summary>
        public string Status { get; set; } = "Scheduled";

        /// <summary>
        /// Notes/remarks
        /// </summary>
        public string? Notes { get; set; }

        public virtual Employee? Employee { get; set; }
    }
}
