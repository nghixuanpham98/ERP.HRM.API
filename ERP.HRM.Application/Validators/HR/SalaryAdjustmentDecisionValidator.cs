using ERP.HRM.Application.DTOs.HR;
using FluentValidation;

namespace ERP.HRM.Application.Validators.HR
{
    public class CreateSalaryAdjustmentDecisionValidator : AbstractValidator<CreateSalaryAdjustmentDecisionDto>
    {
        public CreateSalaryAdjustmentDecisionValidator()
        {
            RuleFor(x => x.EmployeeId)
                .GreaterThan(0)
                .WithMessage("Employee ID must be greater than 0");

            RuleFor(x => x.DecisionType)
                .NotEmpty()
                .WithMessage("Decision type is required")
                .Must(x => new[] { "Increase", "Decrease", "Promotion", "Demotion" }.Contains(x))
                .WithMessage("Decision type must be one of: Increase, Decrease, Promotion, Demotion");

            RuleFor(x => x.OldBaseSalary)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Old base salary must be greater than or equal to 0");

            RuleFor(x => x.NewBaseSalary)
                .GreaterThan(0)
                .WithMessage("New base salary must be greater than 0");

            RuleFor(x => x.EffectiveDate)
                .NotEmpty()
                .WithMessage("Effective date is required");

            RuleFor(x => x.Reason)
                .NotEmpty()
                .WithMessage("Reason is required")
                .MinimumLength(5)
                .WithMessage("Reason must be at least 5 characters");
        }
    }

    public class UpdateSalaryAdjustmentDecisionValidator : AbstractValidator<UpdateSalaryAdjustmentDecisionDto>
    {
        public UpdateSalaryAdjustmentDecisionValidator()
        {
            RuleFor(x => x.DecisionType)
                .NotEmpty()
                .WithMessage("Decision type is required");

            RuleFor(x => x.OldBaseSalary)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Old base salary must be greater than or equal to 0");

            RuleFor(x => x.NewBaseSalary)
                .GreaterThan(0)
                .WithMessage("New base salary must be greater than 0");

            RuleFor(x => x.Reason)
                .NotEmpty()
                .WithMessage("Reason is required");
        }
    }
}
