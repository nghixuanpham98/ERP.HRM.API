using ERP.HRM.Application.DTOs.HR;
using FluentValidation;

namespace ERP.HRM.Application.Validators.HR
{
    public class CreateEmployeeTrainingDtoValidator : AbstractValidator<CreateEmployeeTrainingDto>
    {
        public CreateEmployeeTrainingDtoValidator()
        {
            RuleFor(x => x.EmployeeId)
                .GreaterThan(0).WithMessage("Employee ID must be greater than 0");

            RuleFor(x => x.TrainingName)
                .NotEmpty().WithMessage("Training name is required")
                .MaximumLength(200).WithMessage("Training name cannot exceed 200 characters");

            RuleFor(x => x.Provider)
                .NotEmpty().WithMessage("Training provider is required")
                .MaximumLength(200).WithMessage("Provider name cannot exceed 200 characters");

            RuleFor(x => x.Category)
                .NotEmpty().WithMessage("Training category is required")
                .MaximumLength(100).WithMessage("Category cannot exceed 100 characters");

            RuleFor(x => x.DurationHours)
                .GreaterThan(0).WithMessage("Duration hours must be greater than 0");

            RuleFor(x => x.StartDate)
                .NotEmpty().WithMessage("Start date is required");

            When(x => x.CompletionDate.HasValue, () =>
            {
                RuleFor(x => x.CompletionDate)
                    .GreaterThanOrEqualTo(x => x.StartDate).WithMessage("Completion date must be on or after start date");
            });

            RuleFor(x => x.TrainingCost)
                .GreaterThanOrEqualTo(0).WithMessage("Training cost must be greater than or equal to 0");

            RuleFor(x => x.Status)
                .NotEmpty().WithMessage("Status is required")
                .Must(x => new[] { "Pending", "InProgress", "Completed", "Cancelled", "Failed" }.Contains(x))
                .WithMessage("Invalid status");
        }
    }

    public class UpdateEmployeeTrainingDtoValidator : AbstractValidator<UpdateEmployeeTrainingDto>
    {
        public UpdateEmployeeTrainingDtoValidator()
        {
            When(x => !string.IsNullOrEmpty(x.Status), () =>
            {
                RuleFor(x => x.Status)
                    .Must(x => new[] { "Pending", "InProgress", "Completed", "Cancelled", "Failed" }.Contains(x!))
                    .WithMessage("Invalid status");
            });

            When(x => x.CompletionDate.HasValue, () =>
            {
                RuleFor(x => x.CompletionDate)
                    .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow)).WithMessage("Completion date cannot be in the future");
            });
        }
    }
}
