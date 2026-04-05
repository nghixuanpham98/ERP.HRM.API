using ERP.HRM.Application.DTOs.HR;
using FluentValidation;

namespace ERP.HRM.Application.Validators.HR
{
    public class CreateLeaveBalanceDtoValidator : AbstractValidator<CreateLeaveBalanceDto>
    {
        public CreateLeaveBalanceDtoValidator()
        {
            RuleFor(x => x.EmployeeId)
                .GreaterThan(0).WithMessage("Employee ID must be greater than 0");

            RuleFor(x => x.Year)
                .GreaterThanOrEqualTo(DateTime.UtcNow.Year - 5).WithMessage("Year cannot be more than 5 years in the past")
                .LessThanOrEqualTo(DateTime.UtcNow.Year + 1).WithMessage("Year cannot be more than 1 year in the future");

            RuleFor(x => x.LeaveType)
                .NotEmpty().WithMessage("Leave type is required")
                .MaximumLength(50).WithMessage("Leave type cannot exceed 50 characters")
                .Must(x => new[] { "Annual", "Sick", "Maternity", "Unpaid", "Compassionate" }.Contains(x))
                .WithMessage("Invalid leave type. Must be one of: Annual, Sick, Maternity, Unpaid, Compassionate");

            RuleFor(x => x.AllocatedDays)
                .GreaterThanOrEqualTo(0).WithMessage("Allocated days must be greater than or equal to 0")
                .LessThanOrEqualTo(365).WithMessage("Allocated days cannot exceed 365");

            RuleFor(x => x.CarryOverLimit)
                .GreaterThanOrEqualTo(0).WithMessage("Carry over limit must be greater than or equal to 0")
                .LessThanOrEqualTo(x => x.AllocatedDays).WithMessage("Carry over limit cannot exceed allocated days");

            RuleFor(x => x.ExpiryDate)
                .GreaterThan(DateTime.UtcNow.Date).WithMessage("Expiry date must be in the future");
        }
    }

    public class UpdateLeaveBalanceDtoValidator : AbstractValidator<UpdateLeaveBalanceDto>
    {
        public UpdateLeaveBalanceDtoValidator()
        {
            RuleFor(x => x.UsedDays)
                .GreaterThanOrEqualTo(0).WithMessage("Used days must be greater than or equal to 0");

            RuleFor(x => x.CarriedOverDays)
                .GreaterThanOrEqualTo(0).WithMessage("Carried over days must be greater than or equal to 0");

            When(x => x.ExpiryDate.HasValue, () =>
            {
                RuleFor(x => x.ExpiryDate)
                    .Must(x => x > DateTime.UtcNow.Date).WithMessage("Expiry date must be in the future");
            });
        }
    }
}
