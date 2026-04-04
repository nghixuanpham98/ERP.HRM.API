using ERP.HRM.Application.DTOs.HR;
using FluentValidation;

namespace ERP.HRM.Application.Validators.HR
{
    public class CreatePersonnelTransferValidator : AbstractValidator<CreatePersonnelTransferDto>
    {
        public CreatePersonnelTransferValidator()
        {
            RuleFor(x => x.EmployeeId)
                .GreaterThan(0)
                .WithMessage("Employee ID must be greater than 0");

            RuleFor(x => x.FromDepartmentId)
                .GreaterThan(0)
                .WithMessage("From Department ID must be greater than 0");

            RuleFor(x => x.ToDepartmentId)
                .GreaterThan(0)
                .WithMessage("To Department ID must be greater than 0");

            RuleFor(x => x.FromPositionId)
                .GreaterThan(0)
                .WithMessage("From Position ID must be greater than 0");

            RuleFor(x => x.ToPositionId)
                .GreaterThan(0)
                .WithMessage("To Position ID must be greater than 0");

            RuleFor(x => x.TransferType)
                .NotEmpty()
                .WithMessage("Transfer type is required")
                .Must(x => new[] { "Transfer", "Promotion", "Demotion", "Assignment" }.Contains(x))
                .WithMessage("Transfer type must be one of: Transfer, Promotion, Demotion, Assignment");

            RuleFor(x => x.OldSalary)
                .GreaterThan(0)
                .WithMessage("Old salary must be greater than 0");

            RuleFor(x => x.NewSalary)
                .GreaterThan(0)
                .WithMessage("New salary must be greater than 0");

            RuleFor(x => x.EffectiveDate)
                .NotEmpty()
                .WithMessage("Effective date is required");
        }
    }

    public class UpdatePersonnelTransferValidator : AbstractValidator<UpdatePersonnelTransferDto>
    {
        public UpdatePersonnelTransferValidator()
        {
            RuleFor(x => x.FromDepartmentId)
                .GreaterThan(0)
                .WithMessage("From Department ID must be greater than 0");

            RuleFor(x => x.ToDepartmentId)
                .GreaterThan(0)
                .WithMessage("To Department ID must be greater than 0");

            RuleFor(x => x.FromPositionId)
                .GreaterThan(0)
                .WithMessage("From Position ID must be greater than 0");

            RuleFor(x => x.ToPositionId)
                .GreaterThan(0)
                .WithMessage("To Position ID must be greater than 0");

            RuleFor(x => x.TransferType)
                .NotEmpty()
                .WithMessage("Transfer type is required")
                .Must(x => new[] { "Transfer", "Promotion", "Demotion", "Assignment" }.Contains(x))
                .WithMessage("Transfer type must be one of: Transfer, Promotion, Demotion, Assignment");

            RuleFor(x => x.OldSalary)
                .GreaterThan(0)
                .WithMessage("Old salary must be greater than 0");

            RuleFor(x => x.NewSalary)
                .GreaterThan(0)
                .WithMessage("New salary must be greater than 0");

            RuleFor(x => x.EffectiveDate)
                .NotEmpty()
                .WithMessage("Effective date is required");

            RuleFor(x => x.ApprovalStatus)
                .NotEmpty()
                .WithMessage("Approval status is required")
                .Must(x => new[] { "Pending", "Approved", "Rejected" }.Contains(x))
                .WithMessage("Approval status must be one of: Pending, Approved, Rejected");
        }
    }
}
