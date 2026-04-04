using ERP.HRM.Application.DTOs.HR;
using FluentValidation;

namespace ERP.HRM.Application.Validators.HR
{
    public class CreateLeaveRequestValidator : AbstractValidator<CreateLeaveRequestDto>
    {
        public CreateLeaveRequestValidator()
        {
            RuleFor(x => x.EmployeeId)
                .GreaterThan(0)
                .WithMessage("Employee ID must be greater than 0");

            RuleFor(x => x.LeaveType)
                .NotEmpty()
                .WithMessage("Leave type is required")
                .Must(x => new[] { "Annual", "Sick", "Unpaid", "Maternity", "Paternity", "Study", "Emergency" }.Contains(x))
                .WithMessage("Leave type must be one of: Annual, Sick, Unpaid, Maternity, Paternity, Study, Emergency");

            RuleFor(x => x.StartDate)
                .NotEmpty()
                .WithMessage("Start date is required")
                .GreaterThanOrEqualTo(DateTime.Today)
                .WithMessage("Start date must be today or later");

            RuleFor(x => x.EndDate)
                .GreaterThanOrEqualTo(x => x.StartDate)
                .WithMessage("End date must be on or after start date");

            RuleFor(x => x.NumberOfDays)
                .GreaterThan(0)
                .WithMessage("Number of days must be greater than 0");

            RuleFor(x => x.Reason)
                .NotEmpty()
                .WithMessage("Reason is required")
                .MinimumLength(10)
                .WithMessage("Reason must be at least 10 characters");
        }
    }

    public class UpdateLeaveRequestValidator : AbstractValidator<UpdateLeaveRequestDto>
    {
        public UpdateLeaveRequestValidator()
        {
            RuleFor(x => x.LeaveType)
                .NotEmpty()
                .WithMessage("Leave type is required")
                .Must(x => new[] { "Annual", "Sick", "Unpaid", "Maternity", "Paternity", "Study", "Emergency" }.Contains(x))
                .WithMessage("Leave type must be one of: Annual, Sick, Unpaid, Maternity, Paternity, Study, Emergency");

            RuleFor(x => x.StartDate)
                .NotEmpty()
                .WithMessage("Start date is required");

            RuleFor(x => x.EndDate)
                .GreaterThanOrEqualTo(x => x.StartDate)
                .WithMessage("End date must be on or after start date");

            RuleFor(x => x.NumberOfDays)
                .GreaterThan(0)
                .WithMessage("Number of days must be greater than 0");

            RuleFor(x => x.Reason)
                .NotEmpty()
                .WithMessage("Reason is required")
                .MinimumLength(10)
                .WithMessage("Reason must be at least 10 characters");

            RuleFor(x => x.ApprovalStatus)
                .NotEmpty()
                .WithMessage("Approval status is required")
                .Must(x => new[] { "Pending", "Approved", "Rejected", "Cancelled" }.Contains(x))
                .WithMessage("Approval status must be one of: Pending, Approved, Rejected, Cancelled");
        }
    }
}
