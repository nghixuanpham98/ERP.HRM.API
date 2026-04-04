using ERP.HRM.Application.DTOs.HR;
using FluentValidation;

namespace ERP.HRM.Application.Validators.HR
{
    public class CreateEmploymentContractValidator : AbstractValidator<CreateEmploymentContractDto>
    {
        public CreateEmploymentContractValidator()
        {
            RuleFor(x => x.EmployeeId)
                .GreaterThan(0)
                .WithMessage("Employee ID must be greater than 0");

            RuleFor(x => x.ContractNumber)
                .NotEmpty()
                .WithMessage("Contract number is required")
                .MinimumLength(3)
                .WithMessage("Contract number must be at least 3 characters");

            RuleFor(x => x.ContractType)
                .NotEmpty()
                .WithMessage("Contract type is required")
                .Must(x => new[] { "Full-time", "Part-time", "Contract", "Seasonal" }.Contains(x))
                .WithMessage("Contract type must be one of: Full-time, Part-time, Contract, Seasonal");

            RuleFor(x => x.StartDate)
                .NotEmpty()
                .WithMessage("Start date is required");

            RuleFor(x => x.EndDate)
                .GreaterThan(x => x.StartDate)
                .When(x => x.EndDate.HasValue)
                .WithMessage("End date must be after start date");

            RuleFor(x => x.ProbationEndDate)
                .GreaterThanOrEqualTo(x => x.StartDate)
                .When(x => x.ProbationEndDate.HasValue)
                .WithMessage("Probation end date must be on or after start date");
        }
    }

    public class UpdateEmploymentContractValidator : AbstractValidator<UpdateEmploymentContractDto>
    {
        public UpdateEmploymentContractValidator()
        {
            RuleFor(x => x.ContractNumber)
                .NotEmpty()
                .WithMessage("Contract number is required")
                .MinimumLength(3)
                .WithMessage("Contract number must be at least 3 characters");

            RuleFor(x => x.ContractType)
                .NotEmpty()
                .WithMessage("Contract type is required");

            RuleFor(x => x.EndDate)
                .GreaterThan(x => x.StartDate)
                .When(x => x.EndDate.HasValue)
                .WithMessage("End date must be after start date");
        }
    }
}
