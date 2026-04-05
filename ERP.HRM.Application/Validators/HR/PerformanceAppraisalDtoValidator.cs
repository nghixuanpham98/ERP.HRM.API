using ERP.HRM.Application.DTOs.HR;
using FluentValidation;

namespace ERP.HRM.Application.Validators.HR
{
    public class CreatePerformanceAppraisalDtoValidator : AbstractValidator<CreatePerformanceAppraisalDto>
    {
        public CreatePerformanceAppraisalDtoValidator()
        {
            RuleFor(x => x.EmployeeId)
                .GreaterThan(0).WithMessage("Employee ID must be greater than 0");

            RuleFor(x => x.AppraisalPeriod)
                .NotEmpty().WithMessage("Appraisal period is required")
                .MaximumLength(50).WithMessage("Appraisal period cannot exceed 50 characters");

            RuleFor(x => x.AppraisedByUserId)
                .NotEmpty().WithMessage("Appraiser user ID is required");

            RuleFor(x => x.OverallRatingScore)
                .InclusiveBetween(1m, 5m).WithMessage("Overall rating score must be between 1 and 5");

            RuleFor(x => x.PerformanceRating)
                .InclusiveBetween(1, 5).WithMessage("Performance rating must be between 1 and 5");

            RuleFor(x => x.CompetencyRating)
                .InclusiveBetween(1, 5).WithMessage("Competency rating must be between 1 and 5");

            RuleFor(x => x.BehaviorRating)
                .InclusiveBetween(1, 5).WithMessage("Behavior rating must be between 1 and 5");

            RuleFor(x => x.CommunicationRating)
                .InclusiveBetween(1, 5).WithMessage("Communication rating must be between 1 and 5");

            RuleFor(x => x.TeamworkRating)
                .InclusiveBetween(1, 5).WithMessage("Teamwork rating must be between 1 and 5");

            When(x => !string.IsNullOrEmpty(x.PromotionRecommendation), () =>
            {
                RuleFor(x => x.PromotionRecommendation)
                    .Must(x => new[] { "Yes", "No", "Maybe" }.Contains(x!))
                    .WithMessage("Promotion recommendation must be: Yes, No, or Maybe");
            });

            When(x => !string.IsNullOrEmpty(x.Comments), () =>
            {
                RuleFor(x => x.Comments)
                    .MaximumLength(2000).WithMessage("Comments cannot exceed 2000 characters");
            });
        }
    }

    public class UpdatePerformanceAppraisalDtoValidator : AbstractValidator<UpdatePerformanceAppraisalDto>
    {
        public UpdatePerformanceAppraisalDtoValidator()
        {
            When(x => x.OverallRatingScore.HasValue, () =>
            {
                RuleFor(x => x.OverallRatingScore)
                    .InclusiveBetween(1m, 5m).WithMessage("Overall rating score must be between 1 and 5");
            });

            When(x => !string.IsNullOrEmpty(x.Status), () =>
            {
                RuleFor(x => x.Status)
                    .Must(x => new[] { "Draft", "Submitted", "Reviewed", "Approved", "Completed" }.Contains(x!))
                    .WithMessage("Invalid status");
            });

            When(x => !string.IsNullOrEmpty(x.Comments), () =>
            {
                RuleFor(x => x.Comments)
                    .MaximumLength(2000).WithMessage("Comments cannot exceed 2000 characters");
            });
        }
    }
}
