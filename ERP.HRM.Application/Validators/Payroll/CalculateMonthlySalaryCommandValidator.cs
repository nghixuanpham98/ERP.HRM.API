using ERP.HRM.Application.Features.Payroll.Commands;
using FluentValidation;

namespace ERP.HRM.Application.Validators.Payroll
{
    /// <summary>
    /// Validator for CalculateMonthlySalaryCommand
    /// </summary>
    public class CalculateMonthlySalaryCommandValidator : AbstractValidator<CalculateMonthlySalaryCommand>
    {
        public CalculateMonthlySalaryCommandValidator()
        {
            RuleFor(x => x.EmployeeId)
                .GreaterThan(0)
                .WithMessage("Mã nhân viên phải lớn hơn 0");

            RuleFor(x => x.PayrollPeriodId)
                .GreaterThan(0)
                .WithMessage("Kỳ lương phải lớn hơn 0");

            RuleFor(x => x.OverrideBaseSalary)
                .GreaterThanOrEqualTo(0)
                .When(x => x.OverrideBaseSalary.HasValue)
                .WithMessage("Lương cơ bản ghi đè phải lớn hơn hoặc bằng 0");

            RuleFor(x => x.OverrideAllowance)
                .GreaterThanOrEqualTo(0)
                .When(x => x.OverrideAllowance.HasValue)
                .WithMessage("Phụ cấp ghi đè phải lớn hơn hoặc bằng 0");
        }
    }
}
