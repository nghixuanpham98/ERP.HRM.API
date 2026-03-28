using ERP.HRM.Application.DTOs.Employee;
using FluentValidation;
using System;

namespace ERP.HRM.Application.Validators
{
    public class UpdateEmployeeValidator : AbstractValidator<UpdateEmployeeDto>
    {
        public UpdateEmployeeValidator()
        {
            RuleFor(x => x.EmployeeId)
                .GreaterThan(0).WithMessage("Id nhân viên không hợp lệ");

            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Tên không được để trống")
                .MaximumLength(150).WithMessage("Tên không được vượt quá 150 ký tự");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email không được để trống")
                .EmailAddress().WithMessage("Email không hợp lệ")
                .MaximumLength(100).WithMessage("Email không được vượt quá 100 ký tự");

            RuleFor(x => x.PhoneNumber)
                .MaximumLength(20).WithMessage("Số điện thoại không được vượt quá 20 ký tự")
                .Matches(@"^\d{10,11}$").When(x => !string.IsNullOrEmpty(x.PhoneNumber))
                .WithMessage("Số điện thoại phải có 10 hoặc 11 chữ số");

            RuleFor(x => x.DepartmentId)
                .GreaterThan(0).WithMessage("Phòng ban không hợp lệ");

            RuleFor(x => x.PositionId)
                .GreaterThan(0).WithMessage("Vị trí không hợp lệ");

            RuleFor(x => x.BaseSalary)
                .GreaterThanOrEqualTo(0).When(x => x.BaseSalary.HasValue)
                .WithMessage("Lương cơ bản phải lớn hơn hoặc bằng 0");

            RuleFor(x => x.Allowance)
                .GreaterThanOrEqualTo(0).When(x => x.Allowance.HasValue)
                .WithMessage("Phụ cấp phải lớn hơn hoặc bằng 0");

            RuleFor(x => x.HireDate)
                .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow))
                .WithMessage("Ngày tuyển dụng không được trong tương lai");

            RuleFor(x => x.DateOfBirth)
                .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-18)))
                .When(x => x.DateOfBirth.HasValue)
                .WithMessage("Nhân viên phải từ 18 tuổi trở lên");

            RuleFor(x => x.Gender)
                .Must(x => string.IsNullOrEmpty(x) || new[] { "Nam", "Nữ", "Khác" }.Contains(x))
                .When(x => !string.IsNullOrEmpty(x.Gender))
                .WithMessage("Giới tính phải là 'Nam', 'Nữ' hoặc 'Khác'");

            RuleFor(x => x.ContractType)
                .Must(x => string.IsNullOrEmpty(x) || new[] { "Chính thức", "Hợp đồng", "Thử việc" }.Contains(x))
                .When(x => !string.IsNullOrEmpty(x.ContractType))
                .WithMessage("Loại hợp đồng không hợp lệ");

            RuleFor(x => x.ContractStartDate)
                .NotEmpty().When(x => !string.IsNullOrEmpty(x.ContractType))
                .WithMessage("Ngày bắt đầu hợp đồng không được để trống khi có loại hợp đồng")
                .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow))
                .When(x => x.ContractStartDate.HasValue)
                .WithMessage("Ngày bắt đầu hợp đồng không được trong tương lai");

            RuleFor(x => x.ContractEndDate)
                .GreaterThan(x => x.ContractStartDate)
                .When(x => x.ContractStartDate.HasValue && x.ContractEndDate.HasValue)
                .WithMessage("Ngày kết thúc phải sau ngày bắt đầu");

            RuleFor(x => x.ProbationEndDate)
                .GreaterThan(x => x.HireDate)
                .When(x => x.ProbationEndDate.HasValue)
                .WithMessage("Ngày kết thúc thử việc phải sau ngày tuyển dụng");
        }
    }
}
