namespace ERP.HRM.Application.DTOs.HR
{
    public class PerformanceAppraisalDto
    {
        public int PerformanceAppraisalId { get; set; }
        public int EmployeeId { get; set; }
        public string AppraisalPeriod { get; set; } = null!;
        public Guid? AppraisedByUserId { get; set; }
        public DateTime AppraisalDate { get; set; }
        public decimal OverallRatingScore { get; set; }
        public int PerformanceRating { get; set; }
        public int CompetencyRating { get; set; }
        public int BehaviorRating { get; set; }
        public int CommunicationRating { get; set; }
        public int TeamworkRating { get; set; }
        public string? OverallComments { get; set; }
        public string? Strengths { get; set; }
        public string? AreasForImprovement { get; set; }
        public string? PromotionRecommendation { get; set; }
        public string Status { get; set; } = null!;
        public bool EmployeeAcknowledged { get; set; }
    }

    public class CreatePerformanceAppraisalDto
    {
        public int EmployeeId { get; set; }
        public string AppraisalPeriod { get; set; } = null!;
        public Guid? AppraisedByUserId { get; set; }
        public decimal OverallRatingScore { get; set; }
        public int PerformanceRating { get; set; }
        public int CompetencyRating { get; set; }
        public int BehaviorRating { get; set; }
        public int CommunicationRating { get; set; }
        public int TeamworkRating { get; set; }
        public string? OverallComments { get; set; }
        public string? Strengths { get; set; }
        public string? AreasForImprovement { get; set; }
        public string? PromotionRecommendation { get; set; }
        public string? Comments { get; set; }
        public string? GoalsAchieved { get; set; }
        public string? GoalsForNext { get; set; }
        public string? TrainingRecommendations { get; set; }
    }

    public class UpdatePerformanceAppraisalDto
    {
        public int PerformanceAppraisalId { get; set; }
        public decimal? OverallRatingScore { get; set; }
        public string? OverallComments { get; set; }
        public string? Comments { get; set; }
        public string? Status { get; set; }
    }
}
