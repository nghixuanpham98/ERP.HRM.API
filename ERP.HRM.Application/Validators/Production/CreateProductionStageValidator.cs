using FluentValidation;
using ERP.HRM.Application.DTOs.Production;

namespace ERP.HRM.Application.Validators.Production
{
    /// <summary>
    /// Validator for CreateProductionStageDto
    /// </summary>
    public class CreateProductionStageValidator : AbstractValidator<CreateProductionStageDto>
    {
        public CreateProductionStageValidator()
        {
            RuleFor(x => x.StageName)
                .NotEmpty().WithMessage("Stage name is required")
                .MaximumLength(255).WithMessage("Stage name cannot exceed 255 characters");

            RuleFor(x => x.StageCode)
                .NotEmpty().WithMessage("Stage code is required")
                .MaximumLength(50).WithMessage("Stage code cannot exceed 50 characters")
                .Matches(@"^[A-Z0-9\-]+$").WithMessage("Stage code must contain only uppercase letters, numbers, and hyphens");

            RuleFor(x => x.SequenceOrder)
                .GreaterThan(0).WithMessage("Sequence order must be greater than 0");

            RuleFor(x => x.EstimatedHours)
                .GreaterThan(0).When(x => x.EstimatedHours.HasValue)
                .WithMessage("Estimated hours must be greater than 0");

            RuleFor(x => x.Status)
                .Must(x => new[] { "Active", "Inactive", "Paused" }.Contains(x))
                .WithMessage("Invalid status");
        }
    }
}
