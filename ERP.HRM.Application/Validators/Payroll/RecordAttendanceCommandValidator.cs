using ERP.HRM.Application.Features.Payroll.Commands;
using FluentValidation;

namespace ERP.HRM.Application.Validators.Payroll
{
    /// <summary>
    /// Validator for RecordAttendanceCommand
    /// </summary>
    public class RecordAttendanceCommandValidator : AbstractValidator<RecordAttendanceCommand>
    {
        public RecordAttendanceCommandValidator()
        {
            RuleFor(x => x.EmployeeId)
                .GreaterThan(0)
                .WithMessage("Mã nhân viên phải lớn hơn 0");

            RuleFor(x => x.PayrollPeriodId)
                .GreaterThan(0)
                .WithMessage("Kỳ lương phải lớn hơn 0");

            RuleFor(x => x.AttendanceDate)
                .NotEmpty()
                .WithMessage("Ngày điểm danh không được để trống")
                .LessThanOrEqualTo(DateTime.UtcNow)
                .WithMessage("Ngày điểm danh không được trong tương lai");

            RuleFor(x => x.WorkingDays)
                .InclusiveBetween(0, 1)
                .WithMessage("Ngày công phải từ 0 đến 1 (0, 0.5, 1)");

            RuleFor(x => x.OvertimeHours)
                .GreaterThanOrEqualTo(0)
                .When(x => x.OvertimeHours.HasValue)
                .WithMessage("Giờ tăng ca phải lớn hơn hoặc bằng 0");
        }
    }
}
