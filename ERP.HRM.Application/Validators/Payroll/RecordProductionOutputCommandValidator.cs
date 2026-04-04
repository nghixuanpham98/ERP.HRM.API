using ERP.HRM.Application.Features.Payroll.Commands;
using FluentValidation;

namespace ERP.HRM.Application.Validators.Payroll
{
    /// <summary>
    /// Validator for RecordProductionOutputCommand
    /// </summary>
    public class RecordProductionOutputCommandValidator : AbstractValidator<RecordProductionOutputCommand>
    {
        public RecordProductionOutputCommandValidator()
        {
            RuleFor(x => x.EmployeeId)
                .GreaterThan(0)
                .WithMessage("Mã nhân viên phải lớn hơn 0");

            RuleFor(x => x.PayrollPeriodId)
                .GreaterThan(0)
                .WithMessage("Kỳ lương phải lớn hơn 0");

            RuleFor(x => x.ProductId)
                .GreaterThan(0)
                .WithMessage("Mã sản phẩm phải lớn hơn 0");

            RuleFor(x => x.Quantity)
                .GreaterThan(0)
                .WithMessage("Số lượng sản phẩm phải lớn hơn 0");

            RuleFor(x => x.UnitPrice)
                .GreaterThan(0)
                .WithMessage("Đơn giá phải lớn hơn 0");

            RuleFor(x => x.ProductionDate)
                .NotEmpty()
                .WithMessage("Ngày sản xuất không được để trống")
                .LessThanOrEqualTo(DateTime.UtcNow)
                .WithMessage("Ngày sản xuất không được trong tương lai");

            RuleFor(x => x.QualityStatus)
                .Must(x => x == null || new[] { "OK", "Defective", "Rework" }.Contains(x))
                .WithMessage("Trạng thái chất lượng phải là OK, Defective hoặc Rework");
        }
    }
}
