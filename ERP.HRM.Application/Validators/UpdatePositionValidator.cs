using ERP.HRM.Application.DTOs.Position;
using FluentValidation;

namespace ERP.HRM.Application.Validators
{
    public class UpdatePositionValidator : AbstractValidator<UpdatePositionDto>
    {
        public UpdatePositionValidator()
        {
            RuleFor(x => x.PositionId)
                .GreaterThan(0).WithMessage("Id vị trí không hợp lệ");

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
