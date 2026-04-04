using FluentValidation;
using ERP.HRM.Application.DTOs.Payroll;

namespace ERP.HRM.Application.Validators.Payroll
{
    /// <summary>
    /// Validator for CreateSalaryComponentDto
    /// </summary>
    public class CreateSalaryComponentValidator : AbstractValidator<CreateSalaryComponentDto>
    {
        public CreateSalaryComponentValidator()
        {
            RuleFor(x => x.EmployeeId)
                .GreaterThan(0).WithMessage("Employee ID must be valid");

            RuleFor(x => x.ComponentType)
                .NotEmpty().WithMessage("Component type is required")
                .Must(x => new[] { "Allowance", "Bonus", "Fine", "Deduction", "Adjustment" }.Contains(x))
                .WithMessage("Invalid component type");

            RuleFor(x => x.ComponentName)
                .NotEmpty().WithMessage("Component name is required")
                .MaximumLength(255).WithMessage("Component name cannot exceed 255 characters");

            RuleFor(x => x.Amount)
                .NotEmpty().WithMessage("Amount is required");

            RuleFor(x => x.EffectiveStartDate)
                .NotEmpty().WithMessage("Effective start date is required")
                .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now))
                .WithMessage("Start date cannot be in the future");

            RuleFor(x => x.EffectiveEndDate)
                .GreaterThanOrEqualTo(x => x.EffectiveStartDate)
                .When(x => x.EffectiveEndDate.HasValue)
                .WithMessage("End date must be after start date");

            RuleFor(x => x.ApplicablePeriod)
                .Must(x => new[] { "Monthly", "Quarterly", "Annual", "Custom" }.Contains(x))
                .WithMessage("Invalid applicable period");

            RuleFor(x => x.Status)
                .Must(x => new[] { "Active", "Inactive", "Suspended" }.Contains(x))
                .WithMessage("Invalid status");
        }
    }
}
