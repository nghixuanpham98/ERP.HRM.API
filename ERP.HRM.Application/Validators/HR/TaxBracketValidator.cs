using ERP.HRM.Application.DTOs.HR;
using FluentValidation;

namespace ERP.HRM.Application.Validators.HR
{
    public class CreateTaxBracketValidator : AbstractValidator<CreateTaxBracketDto>
    {
        public CreateTaxBracketValidator()
        {
            RuleFor(x => x.BracketName)
                .NotEmpty()
                .WithMessage("Bracket name is required");

            RuleFor(x => x.MinIncome)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Minimum income must be greater than or equal to 0");

            RuleFor(x => x.MaxIncome)
                .GreaterThan(x => x.MinIncome)
                .WithMessage("Maximum income must be greater than minimum income");

            RuleFor(x => x.TaxRate)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Tax rate must be greater than or equal to 0")
                .LessThanOrEqualTo(100)
                .WithMessage("Tax rate must be less than or equal to 100");

            RuleFor(x => x.EffectiveDate)
                .NotEmpty()
                .WithMessage("Effective date is required");
        }
    }

    public class UpdateTaxBracketValidator : AbstractValidator<UpdateTaxBracketDto>
    {
        public UpdateTaxBracketValidator()
        {
            RuleFor(x => x.BracketName)
                .NotEmpty()
                .WithMessage("Bracket name is required");

            RuleFor(x => x.MinIncome)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Minimum income must be greater than or equal to 0");

            RuleFor(x => x.MaxIncome)
                .GreaterThan(x => x.MinIncome)
                .WithMessage("Maximum income must be greater than minimum income");

            RuleFor(x => x.TaxRate)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Tax rate must be greater than or equal to 0")
                .LessThanOrEqualTo(100)
                .WithMessage("Tax rate must be less than or equal to 100");
        }
    }
}
