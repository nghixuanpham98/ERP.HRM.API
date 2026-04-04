using ERP.HRM.Application.DTOs.HR;
using FluentValidation;

namespace ERP.HRM.Application.Validators.HR
{
    public class CreateEmployeeRecordValidator : AbstractValidator<CreateEmployeeRecordDto>
    {
        public CreateEmployeeRecordValidator()
        {
            RuleFor(x => x.EmployeeId)
                .GreaterThan(0)
                .WithMessage("Employee ID must be greater than 0");

            RuleFor(x => x.RecordNumber)
                .NotEmpty()
                .WithMessage("Record number is required")
                .MinimumLength(3)
                .WithMessage("Record number must be at least 3 characters");

            RuleFor(x => x.FilePath)
                .NotEmpty()
                .WithMessage("File path is required");

            RuleFor(x => x.DocumentType)
                .NotEmpty()
                .WithMessage("Document type is required")
                .Must(x => new[] { "Identity", "Certificate", "Contract", "Medical", "License", "Other" }.Contains(x))
                .WithMessage("Document type must be one of: Identity, Certificate, Contract, Medical, License, Other");

            RuleFor(x => x.IssueDate)
                .NotEmpty()
                .WithMessage("Issue date is required")
                .LessThanOrEqualTo(DateTime.Today)
                .WithMessage("Issue date cannot be in the future");

            RuleFor(x => x.ExpiryDate)
                .GreaterThan(x => x.IssueDate)
                .When(x => x.ExpiryDate.HasValue)
                .WithMessage("Expiry date must be after issue date");

            RuleFor(x => x.IssuingOrganization)
                .NotEmpty()
                .WithMessage("Issuing organization is required");
        }
    }

    public class UpdateEmployeeRecordValidator : AbstractValidator<UpdateEmployeeRecordDto>
    {
        public UpdateEmployeeRecordValidator()
        {
            RuleFor(x => x.RecordNumber)
                .NotEmpty()
                .WithMessage("Record number is required")
                .MinimumLength(3)
                .WithMessage("Record number must be at least 3 characters");

            RuleFor(x => x.FilePath)
                .NotEmpty()
                .WithMessage("File path is required");

            RuleFor(x => x.DocumentType)
                .NotEmpty()
                .WithMessage("Document type is required")
                .Must(x => new[] { "Identity", "Certificate", "Contract", "Medical", "License", "Other" }.Contains(x))
                .WithMessage("Document type must be one of: Identity, Certificate, Contract, Medical, License, Other");

            RuleFor(x => x.IssueDate)
                .NotEmpty()
                .WithMessage("Issue date is required");

            RuleFor(x => x.ExpiryDate)
                .GreaterThan(x => x.IssueDate)
                .When(x => x.ExpiryDate.HasValue)
                .WithMessage("Expiry date must be after issue date");

            RuleFor(x => x.IssuingOrganization)
                .NotEmpty()
                .WithMessage("Issuing organization is required");

            RuleFor(x => x.Status)
                .NotEmpty()
                .WithMessage("Status is required")
                .Must(x => new[] { "Active", "Expired", "Archived" }.Contains(x))
                .WithMessage("Status must be one of: Active, Expired, Archived");
        }
    }
}
