using ERP.HRM.Application.DTOs.HR;
using FluentValidation;

namespace ERP.HRM.Application.Validators.HR
{
    public class CreateInsuranceParticipationValidator : AbstractValidator<CreateInsuranceParticipationDto>
    {
        public CreateInsuranceParticipationValidator()
        {
            RuleFor(x => x.EmployeeId)
                .GreaterThan(0)
                .WithMessage("Employee ID must be greater than 0");

            RuleFor(x => x.InsuranceType)
                .NotEmpty()
                .WithMessage("Insurance type is required")
                .Must(x => new[] { "Health", "Unemployment", "WorkInjury", "Social" }.Contains(x))
                .WithMessage("Insurance type must be one of: Health, Unemployment, WorkInjury, Social");

            RuleFor(x => x.InsuranceNumber)
                .NotEmpty()
                .WithMessage("Insurance number is required");

            RuleFor(x => x.StartDate)
                .NotEmpty()
                .WithMessage("Start date is required");

            RuleFor(x => x.EndDate)
                .GreaterThan(x => x.StartDate)
                .When(x => x.EndDate.HasValue)
                .WithMessage("End date must be after start date");

            RuleFor(x => x.ContributionBaseSalary)
                .GreaterThan(0)
                .WithMessage("Contribution base salary must be greater than 0");

            RuleFor(x => x.EmployeeContributionRate)
                .GreaterThanOrEqualTo(0)
                .LessThanOrEqualTo(100)
                .WithMessage("Employee contribution rate must be between 0 and 100");

            RuleFor(x => x.EmployerContributionRate)
                .GreaterThanOrEqualTo(0)
                .LessThanOrEqualTo(100)
                .WithMessage("Employer contribution rate must be between 0 and 100");
        }
    }

    public class UpdateInsuranceParticipationValidator : AbstractValidator<UpdateInsuranceParticipationDto>
    {
        public UpdateInsuranceParticipationValidator()
        {
            RuleFor(x => x.InsuranceNumber)
                .NotEmpty()
                .WithMessage("Insurance number is required");

            RuleFor(x => x.StartDate)
                .NotEmpty()
                .WithMessage("Start date is required");

            RuleFor(x => x.EndDate)
                .GreaterThan(x => x.StartDate)
                .When(x => x.EndDate.HasValue)
                .WithMessage("End date must be after start date");

            RuleFor(x => x.Status)
                .NotEmpty()
                .WithMessage("Status is required")
                .Must(x => new[] { "Active", "Suspended", "Terminated" }.Contains(x))
                .WithMessage("Status must be one of: Active, Suspended, Terminated");

            RuleFor(x => x.ContributionBaseSalary)
                .GreaterThan(0)
                .WithMessage("Contribution base salary must be greater than 0");

            RuleFor(x => x.EmployeeContributionRate)
                .GreaterThanOrEqualTo(0)
                .LessThanOrEqualTo(100)
                .WithMessage("Employee contribution rate must be between 0 and 100");

            RuleFor(x => x.EmployerContributionRate)
                .GreaterThanOrEqualTo(0)
                .LessThanOrEqualTo(100)
                .WithMessage("Employer contribution rate must be between 0 and 100");
        }
    }
}
