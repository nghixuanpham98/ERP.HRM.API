using ERP.HRM.Application.DTOs.HR;
using FluentValidation;

namespace ERP.HRM.Application.Validators.HR
{
    public class CreateResignationDecisionValidator : AbstractValidator<CreateResignationDecisionDto>
    {
        public CreateResignationDecisionValidator()
        {
            RuleFor(x => x.EmployeeId)
                .GreaterThan(0)
                .WithMessage("Employee ID must be greater than 0");

            RuleFor(x => x.ResignationType)
                .NotEmpty()
                .WithMessage("Resignation type is required")
                .Must(x => new[] { "Resignation", "Termination", "Layoff", "ContractEnd" }.Contains(x))
                .WithMessage("Resignation type must be one of: Resignation, Termination, Layoff, ContractEnd");

            RuleFor(x => x.NoticeDate)
                .NotEmpty()
                .WithMessage("Notice date is required");

            RuleFor(x => x.EffectiveDate)
                .GreaterThanOrEqualTo(x => x.NoticeDate)
                .WithMessage("Effective date must be on or after notice date");

            RuleFor(x => x.Reason)
                .NotEmpty()
                .WithMessage("Reason is required");

            RuleFor(x => x.SettlementAmount)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Settlement amount must be greater than or equal to 0");
        }
    }

    public class UpdateResignationDecisionValidator : AbstractValidator<UpdateResignationDecisionDto>
    {
        public UpdateResignationDecisionValidator()
        {
            RuleFor(x => x.ResignationType)
                .NotEmpty()
                .WithMessage("Resignation type is required")
                .Must(x => new[] { "Resignation", "Termination", "Layoff", "ContractEnd" }.Contains(x))
                .WithMessage("Resignation type must be one of: Resignation, Termination, Layoff, ContractEnd");

            RuleFor(x => x.NoticeDate)
                .NotEmpty()
                .WithMessage("Notice date is required");

            RuleFor(x => x.EffectiveDate)
                .GreaterThanOrEqualTo(x => x.NoticeDate)
                .WithMessage("Effective date must be on or after notice date");

            RuleFor(x => x.Reason)
                .NotEmpty()
                .WithMessage("Reason is required");

            RuleFor(x => x.Status)
                .NotEmpty()
                .WithMessage("Status is required")
                .Must(x => new[] { "Pending", "Approved", "Rejected", "Completed" }.Contains(x))
                .WithMessage("Status must be one of: Pending, Approved, Rejected, Completed");

            RuleFor(x => x.SettlementAmount)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Settlement amount must be greater than or equal to 0");

            RuleFor(x => x.FinalPaymentDate)
                .GreaterThanOrEqualTo(x => x.EffectiveDate)
                .When(x => x.FinalPaymentDate.HasValue)
                .WithMessage("Final payment date must be on or after effective date");
        }
    }
}
