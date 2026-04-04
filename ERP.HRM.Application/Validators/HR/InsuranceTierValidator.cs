using ERP.HRM.Application.DTOs.HR;
using FluentValidation;

namespace ERP.HRM.Application.Validators.HR
{
    public class CreateInsuranceTierValidator : AbstractValidator<CreateInsuranceTierDto>
    {
        public CreateInsuranceTierValidator()
        {
            RuleFor(x => x.TierName)
                .NotEmpty()
                .WithMessage("Tier name is required");

            RuleFor(x => x.InsuranceType)
                .NotEmpty()
                .WithMessage("Insurance type is required")
                .Must(x => new[] { "Health", "Unemployment", "WorkInjury" }.Contains(x))
                .WithMessage("Insurance type must be one of: Health, Unemployment, WorkInjury");

            RuleFor(x => x.MinSalary)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Minimum salary must be greater than or equal to 0");

            RuleFor(x => x.MaxSalary)
                .GreaterThan(x => x.MinSalary)
                .WithMessage("Maximum salary must be greater than minimum salary");

            RuleFor(x => x.EmployeeRate)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Employee rate must be greater than or equal to 0")
                .LessThanOrEqualTo(100)
                .WithMessage("Employee rate must be less than or equal to 100");

            RuleFor(x => x.EmployerRate)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Employer rate must be greater than or equal to 0")
                .LessThanOrEqualTo(100)
                .WithMessage("Employer rate must be less than or equal to 100");

            RuleFor(x => x.EffectiveDate)
                .NotEmpty()
                .WithMessage("Effective date is required");
        }
    }

    public class UpdateInsuranceTierValidator : AbstractValidator<UpdateInsuranceTierDto>
    {
        public UpdateInsuranceTierValidator()
        {
            RuleFor(x => x.TierName)
                .NotEmpty()
                .WithMessage("Tier name is required");

            RuleFor(x => x.InsuranceType)
                .NotEmpty()
                .WithMessage("Insurance type is required");

            RuleFor(x => x.MinSalary)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Minimum salary must be greater than or equal to 0");

            RuleFor(x => x.MaxSalary)
                .GreaterThan(x => x.MinSalary)
                .WithMessage("Maximum salary must be greater than minimum salary");

            RuleFor(x => x.EmployeeRate)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Employee rate must be greater than or equal to 0")
                .LessThanOrEqualTo(100)
                .WithMessage("Employee rate must be less than or equal to 100");

            RuleFor(x => x.EmployerRate)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Employer rate must be greater than or equal to 0")
                .LessThanOrEqualTo(100)
                .WithMessage("Employer rate must be less than or equal to 100");
        }
    }
}
