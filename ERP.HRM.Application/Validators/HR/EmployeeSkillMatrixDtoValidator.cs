using ERP.HRM.Application.DTOs.HR;
using FluentValidation;

namespace ERP.HRM.Application.Validators.HR
{
    public class CreateEmployeeSkillMatrixDtoValidator : AbstractValidator<CreateEmployeeSkillMatrixDto>
    {
        public CreateEmployeeSkillMatrixDtoValidator()
        {
            RuleFor(x => x.EmployeeId)
                .GreaterThan(0).WithMessage("Employee ID must be greater than 0");

            RuleFor(x => x.SkillName)
                .NotEmpty().WithMessage("Skill name is required")
                .MaximumLength(100).WithMessage("Skill name cannot exceed 100 characters");

            RuleFor(x => x.SkillCategory)
                .NotEmpty().WithMessage("Skill category is required")
                .Must(x => new[] { "Technical", "Language", "Soft Skills" }.Contains(x))
                .WithMessage("Invalid skill category");

            RuleFor(x => x.Level)
                .InclusiveBetween(1, 5).WithMessage("Level must be between 1 (Beginner) and 5 (Master)");

            RuleFor(x => x.YearsOfExperience)
                .GreaterThanOrEqualTo(0).WithMessage("Years of experience must be greater than or equal to 0")
                .LessThanOrEqualTo(70).WithMessage("Years of experience cannot exceed 70");

            When(x => !string.IsNullOrEmpty(x.LastAssessmentDate.ToString()), () =>
            {
                RuleFor(x => x.LastAssessmentDate)
                    .LessThanOrEqualTo(DateTime.UtcNow.Date).WithMessage("Last assessment date cannot be in the future");
            });
        }
    }

    public class UpdateEmployeeSkillMatrixDtoValidator : AbstractValidator<UpdateEmployeeSkillMatrixDto>
    {
        public UpdateEmployeeSkillMatrixDtoValidator()
        {
            RuleFor(x => x.Level)
                .InclusiveBetween(1, 5).WithMessage("Level must be between 1 (Beginner) and 5 (Master)");

            RuleFor(x => x.YearsOfExperience)
                .GreaterThanOrEqualTo(0).WithMessage("Years of experience must be greater than or equal to 0");

            When(x => x.AssessmentScore.HasValue, () =>
            {
                RuleFor(x => x.AssessmentScore)
                    .InclusiveBetween(0, 100).WithMessage("Assessment score must be between 0 and 100");
            });
        }
    }
}
