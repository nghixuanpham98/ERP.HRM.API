using FluentValidation;
using ERP.HRM.Application.DTOs.Production;

namespace ERP.HRM.Application.Validators.Production
{
    /// <summary>
    /// Validator for CreateProductionJobDto
    /// </summary>
    public class CreateProductionJobValidator : AbstractValidator<CreateProductionJobDto>
    {
        public CreateProductionJobValidator()
        {
            RuleFor(x => x.ProductionStageId)
                .GreaterThan(0).WithMessage("Production stage ID must be valid");

            RuleFor(x => x.JobName)
                .NotEmpty().WithMessage("Job name is required")
                .MaximumLength(255).WithMessage("Job name cannot exceed 255 characters");

            RuleFor(x => x.JobCode)
                .NotEmpty().WithMessage("Job code is required")
                .MaximumLength(50).WithMessage("Job code cannot exceed 50 characters")
                .Matches(@"^[A-Z0-9\-]+$").WithMessage("Job code must contain only uppercase letters, numbers, and hyphens");

            RuleFor(x => x.ComplexityLevel)
                .Must(x => new[] { "Simple", "Medium", "Complex", "Expert" }.Contains(x))
                .WithMessage("Invalid complexity level");

            RuleFor(x => x.ComplexityMultiplier)
                .GreaterThan(0).WithMessage("Complexity multiplier must be greater than 0");

            RuleFor(x => x.EstimatedTimePerUnit)
                .GreaterThan(0).When(x => x.EstimatedTimePerUnit.HasValue)
                .WithMessage("Estimated time per unit must be greater than 0");

            RuleFor(x => x.Status)
                .Must(x => new[] { "Active", "Inactive", "OnHold" }.Contains(x))
                .WithMessage("Invalid status");
        }
    }
}
