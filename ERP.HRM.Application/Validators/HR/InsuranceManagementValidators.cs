using ERP.HRM.Application.DTOs.HR;
using FluentValidation;

namespace ERP.HRM.Application.Validators.HR
{
    public class CreateInsuranceParticipationDtoValidator : AbstractValidator<CreateInsuranceParticipationDto>
    {
        public CreateInsuranceParticipationDtoValidator()
        {
            RuleFor(x => x.EmployeeId)
                .GreaterThan(0).WithMessage("Employee ID must be greater than 0");

            RuleFor(x => x.InsuranceType)
                .NotEmpty().WithMessage("Insurance type is required")
                .Must(x => new[] { "Health", "Unemployment", "WorkInjury" }.Contains(x))
                .WithMessage("Insurance type must be one of: Health, Unemployment, WorkInjury");

            RuleFor(x => x.InsuranceNumber)
                .NotEmpty().WithMessage("Insurance number is required")
                .Length(5, 50).WithMessage("Insurance number must be between 5 and 50 characters");

            RuleFor(x => x.StartDate)
                .NotEmpty().WithMessage("Start date is required");

            RuleFor(x => x.EndDate)
                .GreaterThan(x => x.StartDate).When(x => x.EndDate.HasValue)
                .WithMessage("End date must be after start date");

            RuleFor(x => x.ContributionBaseSalary)
                .GreaterThan(0).WithMessage("Contribution base salary must be greater than 0");

            RuleFor(x => x.EmployeeContributionRate)
                .GreaterThanOrEqualTo(0).WithMessage("Employee contribution rate cannot be negative")
                .LessThanOrEqualTo(100).WithMessage("Employee contribution rate cannot exceed 100%");

            RuleFor(x => x.EmployerContributionRate)
                .GreaterThanOrEqualTo(0).WithMessage("Employer contribution rate cannot be negative")
                .LessThanOrEqualTo(100).WithMessage("Employer contribution rate cannot exceed 100%");
        }
    }

    public class UpdateInsuranceParticipationDtoValidator : AbstractValidator<UpdateInsuranceParticipationDto>
    {
        public UpdateInsuranceParticipationDtoValidator()
        {
            RuleFor(x => x.InsuranceNumber)
                .NotEmpty().WithMessage("Insurance number is required")
                .Length(5, 50).WithMessage("Insurance number must be between 5 and 50 characters");

            RuleFor(x => x.StartDate)
                .NotEmpty().WithMessage("Start date is required");

            RuleFor(x => x.EndDate)
                .GreaterThan(x => x.StartDate).When(x => x.EndDate.HasValue)
                .WithMessage("End date must be after start date");

            RuleFor(x => x.Status)
                .NotEmpty().WithMessage("Status is required")
                .Must(x => new[] { "Active", "Suspended", "Terminated" }.Contains(x))
                .WithMessage("Status must be one of: Active, Suspended, Terminated");

            RuleFor(x => x.ContributionBaseSalary)
                .GreaterThan(0).WithMessage("Contribution base salary must be greater than 0");

            RuleFor(x => x.EmployeeContributionRate)
                .GreaterThanOrEqualTo(0).WithMessage("Employee contribution rate cannot be negative")
                .LessThanOrEqualTo(100).WithMessage("Employee contribution rate cannot exceed 100%");

            RuleFor(x => x.EmployerContributionRate)
                .GreaterThanOrEqualTo(0).WithMessage("Employer contribution rate cannot be negative")
                .LessThanOrEqualTo(100).WithMessage("Employer contribution rate cannot exceed 100%");
        }
    }

    public class CreateInsuranceTierDtoValidator : AbstractValidator<CreateInsuranceTierDto>
    {
        public CreateInsuranceTierDtoValidator()
        {
            RuleFor(x => x.TierName)
                .NotEmpty().WithMessage("Tier name is required")
                .Length(2, 50).WithMessage("Tier name must be between 2 and 50 characters");

            RuleFor(x => x.InsuranceType)
                .NotEmpty().WithMessage("Insurance type is required")
                .Must(x => new[] { "Health", "Unemployment", "WorkInjury" }.Contains(x))
                .WithMessage("Insurance type must be one of: Health, Unemployment, WorkInjury");

            RuleFor(x => x.MinSalary)
                .GreaterThanOrEqualTo(0).WithMessage("Minimum salary cannot be negative");

            RuleFor(x => x.MaxSalary)
                .GreaterThan(x => x.MinSalary).WithMessage("Maximum salary must be greater than minimum salary");

            RuleFor(x => x.EmployeeRate)
                .GreaterThanOrEqualTo(0).WithMessage("Employee rate cannot be negative")
                .LessThanOrEqualTo(100).WithMessage("Employee rate cannot exceed 100%");

            RuleFor(x => x.EmployerRate)
                .GreaterThanOrEqualTo(0).WithMessage("Employer rate cannot be negative")
                .LessThanOrEqualTo(100).WithMessage("Employer rate cannot exceed 100%");

            RuleFor(x => x.EffectiveDate)
                .NotEmpty().WithMessage("Effective date is required");
        }
    }

    public class UpdateInsuranceTierDtoValidator : AbstractValidator<UpdateInsuranceTierDto>
    {
        public UpdateInsuranceTierDtoValidator()
        {
            RuleFor(x => x.TierName)
                .NotEmpty().WithMessage("Tier name is required")
                .Length(2, 50).WithMessage("Tier name must be between 2 and 50 characters");

            RuleFor(x => x.MinSalary)
                .GreaterThanOrEqualTo(0).WithMessage("Minimum salary cannot be negative");

            RuleFor(x => x.MaxSalary)
                .GreaterThan(x => x.MinSalary).WithMessage("Maximum salary must be greater than minimum salary");

            RuleFor(x => x.EmployeeRate)
                .GreaterThanOrEqualTo(0).WithMessage("Employee rate cannot be negative")
                .LessThanOrEqualTo(100).WithMessage("Employee rate cannot exceed 100%");

            RuleFor(x => x.EmployerRate)
                .GreaterThanOrEqualTo(0).WithMessage("Employer rate cannot be negative")
                .LessThanOrEqualTo(100).WithMessage("Employer rate cannot exceed 100%");
        }
    }
}
