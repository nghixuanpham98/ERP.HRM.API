using ERP.HRM.Application.DTOs.Position;
using FluentValidation;

namespace ERP.HRM.Application.Validators
{
    public class CreatePositionValidator : AbstractValidator<CreatePositionDto>
    {
        public CreatePositionValidator()
        {
            RuleFor(x => x.PositionName)
                .NotEmpty().WithMessage("Tên vị trí không được để trống")
                .MaximumLength(100);

            RuleFor(x => x.PositionCode)
                .NotEmpty().WithMessage("Mã vị trí không được để trống")
                .MaximumLength(20);

            RuleFor(x => x.Allowance)
                .GreaterThan(0).When(x => x.Allowance.HasValue)
                .WithMessage("Phụ cấp phải lớn hơn 0");

            RuleFor(x => x.Level)
                .GreaterThan(0).When(x => x.Level.HasValue)
                .WithMessage("Cấp bậc phải lớn hơn 0");
        }
    }
}
