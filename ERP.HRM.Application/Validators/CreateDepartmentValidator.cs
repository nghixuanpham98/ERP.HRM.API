using ERP.HRM.Application.DTOs.Department;
using FluentValidation;

namespace ERP.HRM.Application.Validators
{
    public class CreateDepartmentValidator : AbstractValidator<CreateDepartmentDto>
    {
        public CreateDepartmentValidator()
        {
            RuleFor(x => x.DepartmentName)
                .NotEmpty().WithMessage("Tên phòng ban không được để trống")
                .MaximumLength(100);

            RuleFor(x => x.DepartmentCode)
                .NotEmpty().WithMessage("Mã phòng ban không được để trống")
                .MaximumLength(20);

            RuleFor(x => x.ParentDepartmentId)
                .GreaterThan(0).When(x => x.ParentDepartmentId.HasValue)
                .WithMessage("Id phòng ban cha không hợp lệ");

            RuleFor(x => x.HeadOfDepartmentId)
                .GreaterThan(0).When(x => x.HeadOfDepartmentId.HasValue)
                .WithMessage("Id trưởng phòng không hợp lệ");
        }
    }
}
