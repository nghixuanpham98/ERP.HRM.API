using ERP.HRM.Application.DTOs.HR;
using FluentValidation;

namespace ERP.HRM.Application.Validators.HR
{
    public class CreateSalaryGradeValidator : AbstractValidator<CreateSalaryGradeDto>
    {
        public CreateSalaryGradeValidator()
        {
            RuleFor(x => x.GradeName)
                .NotEmpty()
                .WithMessage("Grade name is required")
                .MinimumLength(2)
                .WithMessage("Grade name must be at least 2 characters");

            RuleFor(x => x.GradeLevel)
                .GreaterThan(0)
                .WithMessage("Grade level must be greater than 0");

            RuleFor(x => x.MinSalary)
                .GreaterThan(0)
                .WithMessage("Minimum salary must be greater than 0");

            RuleFor(x => x.MidSalary)
                .GreaterThan(x => x.MinSalary)
                .WithMessage("Mid salary must be greater than minimum salary");

            RuleFor(x => x.MaxSalary)
                .GreaterThan(x => x.MidSalary)
                .WithMessage("Maximum salary must be greater than mid salary");

            RuleFor(x => x.EffectiveDate)
                .NotEmpty()
                .WithMessage("Effective date is required");
        }
    }

    public class UpdateSalaryGradeValidator : AbstractValidator<UpdateSalaryGradeDto>
    {
        public UpdateSalaryGradeValidator()
        {
            RuleFor(x => x.GradeName)
                .NotEmpty()
                .WithMessage("Grade name is required");

            RuleFor(x => x.GradeLevel)
                .GreaterThan(0)
                .WithMessage("Grade level must be greater than 0");

            RuleFor(x => x.MinSalary)
                .GreaterThan(0)
                .WithMessage("Minimum salary must be greater than 0");

            RuleFor(x => x.MidSalary)
                .GreaterThan(x => x.MinSalary)
                .WithMessage("Mid salary must be greater than minimum salary");

            RuleFor(x => x.MaxSalary)
                .GreaterThan(x => x.MidSalary)
                .WithMessage("Maximum salary must be greater than mid salary");
        }
    }
}
