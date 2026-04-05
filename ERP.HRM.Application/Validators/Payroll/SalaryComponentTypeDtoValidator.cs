using ERP.HRM.Application.DTOs.Payroll;
using FluentValidation;

namespace ERP.HRM.Application.Validators.Payroll
{
    public class CreateSalaryComponentTypeDtoValidator : AbstractValidator<CreateSalaryComponentTypeDto>
    {
        public CreateSalaryComponentTypeDtoValidator()
        {
            RuleFor(x => x.ComponentCode)
                .NotEmpty().WithMessage("Component code is required")
                .MaximumLength(50).WithMessage("Component code cannot exceed 50 characters")
                .Matches(@"^[A-Z0-9_]+$").WithMessage("Component code must contain only uppercase letters, numbers, and underscores");

            RuleFor(x => x.ComponentName)
                .NotEmpty().WithMessage("Component name is required")
                .MaximumLength(100).WithMessage("Component name cannot exceed 100 characters");

            RuleFor(x => x.ComponentType)
                .NotEmpty().WithMessage("Component type is required")
                .Must(x => new[] { "Income", "Allowance", "Bonus", "Deduction" }.Contains(x))
                .WithMessage("Invalid component type. Must be one of: Income, Allowance, Bonus, Deduction");

            RuleFor(x => x.DisplayOrder)
                .GreaterThanOrEqualTo(0).WithMessage("Display order must be greater than or equal to 0");
        }
    }

    public class UpdateSalaryComponentTypeDtoValidator : AbstractValidator<UpdateSalaryComponentTypeDto>
    {
        public UpdateSalaryComponentTypeDtoValidator()
        {
            RuleFor(x => x.ComponentName)
                .MaximumLength(100).WithMessage("Component name cannot exceed 100 characters");

            RuleFor(x => x.DisplayOrder)
                .GreaterThanOrEqualTo(0).WithMessage("Display order must be greater than or equal to 0");
        }
    }
}
