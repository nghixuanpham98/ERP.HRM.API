namespace ERP.HRM.Domain.Entities
{
    /// <summary>
    /// Records performance appraisal/evaluation for employees
    /// Tracks goals, achievements, and development areas
    /// </summary>
    public class PerformanceAppraisal : BaseEntity
    {
        public int PerformanceAppraisalId { get; set; }

        /// <summary>
        /// Reference to employee
        /// </summary>
        public int EmployeeId { get; set; }

        /// <summary>
        /// Appraisal period (e.g., "Q1-2024", "H1-2024", "2024")
        /// </summary>
        public string AppraisalPeriod { get; set; } = null!;

        /// <summary>
        /// User ID who performed the appraisal (Manager)
        /// </summary>
        public Guid? AppraisedByUserId { get; set; }

        /// <summary>
        /// Appraisal date
        /// </summary>
        public DateTime AppraisalDate { get; set; }

        /// <summary>
        /// Overall rating score (1-5)
        /// </summary>
        public decimal OverallRatingScore { get; set; }

        /// <summary>
        /// Performance rating (1-5): Far Below Expectations, Below Expectations, Meets Expectations, Exceeds Expectations, Far Exceeds
        /// </summary>
        public int PerformanceRating { get; set; }

        /// <summary>
        /// Competency rating (1-5)
        /// </summary>
        public int CompetencyRating { get; set; }

        /// <summary>
        /// Behavior/Attitude rating (1-5)
        /// </summary>
        public int BehaviorRating { get; set; }

        /// <summary>
        /// Communication rating (1-5)
        /// </summary>
        public int CommunicationRating { get; set; }

        /// <summary>
        /// Teamwork rating (1-5)
        /// </summary>
        public int TeamworkRating { get; set; }

        /// <summary>
        /// Overall comments from appraiser
        /// </summary>
        public string? OverallComments { get; set; }

        /// <summary>
        /// Key strengths (multiline)
        /// </summary>
        public string? Strengths { get; set; }

        /// <summary>
        /// Areas for improvement (multiline)
        /// </summary>
        public string? AreasForImprovement { get; set; }

        /// <summary>
        /// Development plans/recommendations (multiline)
        /// </summary>
        public string? DevelopmentPlans { get; set; }

        /// <summary>
        /// Goals achieved in this period
        /// </summary>
        public string? GoalsAchieved { get; set; }

        /// <summary>
        /// Goals for next period
        /// </summary>
        public string? GoalsForNext { get; set; }

        /// <summary>
        /// Training recommendations
        /// </summary>
        public string? TrainingRecommendations { get; set; }

        /// <summary>
        /// Promotion recommendation (Yes/No/Maybe)
        /// </summary>
        public string? PromotionRecommendation { get; set; }

        /// <summary>
        /// Status: Draft, Submitted, Reviewed, Approved, Completed, Cancelled
        /// </summary>
        public string Status { get; set; } = "Draft";

        /// <summary>
        /// Employee self-review (employee's comments)
        /// </summary>
        public string? EmployeeSelfReview { get; set; }

        /// <summary>
        /// Acknowledgment by employee (signed off)
        /// </summary>
        public bool EmployeeAcknowledged { get; set; } = false;

        /// <summary>
        /// Employee acknowledgment date
        /// </summary>
        public DateTime? EmployeeAcknowledgmentDate { get; set; }

        /// <summary>
        /// Follow-up appraisal date
        /// </summary>
        public DateTime? FollowUpDate { get; set; }

        public virtual Employee? Employee { get; set; }
    }
}
