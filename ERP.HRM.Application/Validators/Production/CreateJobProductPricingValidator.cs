using FluentValidation;
using ERP.HRM.Application.DTOs.Production;

namespace ERP.HRM.Application.Validators.Production
{
    /// <summary>
    /// Validator for CreateJobProductPricingDto
    /// </summary>
    public class CreateJobProductPricingValidator : AbstractValidator<CreateJobProductPricingDto>
    {
        public CreateJobProductPricingValidator()
        {
            RuleFor(x => x.ProductionJobId)
                .GreaterThan(0).WithMessage("Production job ID must be valid");

            RuleFor(x => x.ProductId)
                .GreaterThan(0).WithMessage("Product ID must be valid");

            RuleFor(x => x.BaseUnitPrice)
                .GreaterThan(0).WithMessage("Base unit price must be greater than 0");

            RuleFor(x => x.EffectiveStartDate)
                .NotEmpty().WithMessage("Effective start date is required")
                .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now))
                .WithMessage("Start date cannot be in the future");

            RuleFor(x => x.EffectiveEndDate)
                .GreaterThanOrEqualTo(x => x.EffectiveStartDate)
                .When(x => x.EffectiveEndDate.HasValue)
                .WithMessage("End date must be after start date");

            RuleFor(x => x.Status)
                .Must(x => new[] { "Active", "Inactive", "Deprecated" }.Contains(x))
                .WithMessage("Invalid status");
        }
    }
}
