using ERP.HRM.Application.DTOs.HR;
using FluentValidation;

namespace ERP.HRM.Application.Validators.HR
{
    public class CreateEmployeeCertificationDtoValidator : AbstractValidator<CreateEmployeeCertificationDto>
    {
        public CreateEmployeeCertificationDtoValidator()
        {
            RuleFor(x => x.EmployeeId)
                .GreaterThan(0).WithMessage("Employee ID must be greater than 0");

            RuleFor(x => x.CertificationCode)
                .NotEmpty().WithMessage("Certification code is required")
                .MaximumLength(50).WithMessage("Certification code cannot exceed 50 characters");

            RuleFor(x => x.CertificationName)
                .NotEmpty().WithMessage("Certification name is required")
                .MaximumLength(200).WithMessage("Certification name cannot exceed 200 characters");

            RuleFor(x => x.IssueDate)
                .NotEmpty().WithMessage("Issue date is required")
                .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow)).WithMessage("Issue date cannot be in the future");

            When(x => x.ExpiryDate.HasValue, () =>
            {
                RuleFor(x => x.ExpiryDate)
                    .GreaterThan(x => x.IssueDate).WithMessage("Expiry date must be after issue date");
            });

            RuleFor(x => x.Status)
                .NotEmpty().WithMessage("Status is required")
                .Must(x => new[] { "Active", "Expired", "Revoked", "Suspended" }.Contains(x))
                .WithMessage("Invalid status. Must be one of: Active, Expired, Revoked, Suspended");
        }
    }

    public class UpdateEmployeeCertificationDtoValidator : AbstractValidator<UpdateEmployeeCertificationDto>
    {
        public UpdateEmployeeCertificationDtoValidator()
        {
            When(x => !string.IsNullOrEmpty(x.Status), () =>
            {
                RuleFor(x => x.Status)
                    .Must(x => new[] { "Active", "Expired", "Revoked", "Suspended" }.Contains(x!))
                    .WithMessage("Invalid status. Must be one of: Active, Expired, Revoked, Suspended");
            });

            When(x => x.ExpiryDate.HasValue, () =>
            {
                RuleFor(x => x.ExpiryDate)
                    .GreaterThan(DateOnly.FromDateTime(DateTime.UtcNow)).WithMessage("Expiry date must be in the future");
            });
        }
    }
}
