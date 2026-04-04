using ERP.HRM.Application.DTOs.HR;
using FluentValidation;

namespace ERP.HRM.Application.Validators.HR
{
    public class CreateFamilyDependentValidator : AbstractValidator<CreateFamilyDependentDto>
    {
        public CreateFamilyDependentValidator()
        {
            RuleFor(x => x.EmployeeId)
                .GreaterThan(0)
                .WithMessage("Employee ID must be greater than 0");

            RuleFor(x => x.FullName)
                .NotEmpty()
                .WithMessage("Full name is required")
                .MinimumLength(2)
                .WithMessage("Full name must be at least 2 characters");

            RuleFor(x => x.Relationship)
                .NotEmpty()
                .WithMessage("Relationship is required")
                .Must(x => new[] { "Spouse", "Child", "Parent", "Sibling", "Other" }.Contains(x))
                .WithMessage("Relationship must be one of: Spouse, Child, Parent, Sibling, Other");

            RuleFor(x => x.DateOfBirth)
                .NotEmpty()
                .WithMessage("Date of birth is required");

            RuleFor(x => x.QualificationEndDate)
                .GreaterThan(x => x.QualificationStartDate)
                .When(x => x.QualificationStartDate.HasValue && x.QualificationEndDate.HasValue)
                .WithMessage("Qualification end date must be after qualification start date");
        }
    }

    public class UpdateFamilyDependentValidator : AbstractValidator<UpdateFamilyDependentDto>
    {
        public UpdateFamilyDependentValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty()
                .WithMessage("Full name is required");

            RuleFor(x => x.Relationship)
                .NotEmpty()
                .WithMessage("Relationship is required");

            RuleFor(x => x.DateOfBirth)
                .NotEmpty()
                .WithMessage("Date of birth is required");
        }
    }
}
