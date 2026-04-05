using ERP.HRM.Application.DTOs.HR;
using ERP.HRM.Application.Features.Leave.Commands;
using FluentValidation;

namespace ERP.HRM.Application.Validators.HR
{
    public class CreateLeaveRequestDtoValidator : AbstractValidator<CreateLeaveRequestDto>
    {
        public CreateLeaveRequestDtoValidator()
        {
            RuleFor(x => x.EmployeeId)
                .GreaterThan(0)
                .WithMessage("Employee ID must be greater than 0");

            RuleFor(x => x.StartDate)
                .NotEmpty()
                .WithMessage("Start date is required")
                .GreaterThanOrEqualTo(DateTime.Today)
                .WithMessage("Start date must be today or in the future");

            RuleFor(x => x.EndDate)
                .NotEmpty()
                .WithMessage("End date is required")
                .GreaterThan(x => x.StartDate)
                .WithMessage("End date must be after start date");

            RuleFor(x => x.LeaveType)
                .NotEmpty()
                .WithMessage("Leave type is required")
                .Length(1, 50)
                .WithMessage("Leave type must be between 1 and 50 characters");

            RuleFor(x => x.Reason)
                .NotEmpty()
                .WithMessage("Reason is required")
                .Length(5, 500)
                .WithMessage("Reason must be between 5 and 500 characters");
        }
    }

    public class SubmitLeaveRequestCommandValidator : AbstractValidator<SubmitLeaveRequestCommand>
    {
        public SubmitLeaveRequestCommandValidator()
        {
            RuleFor(x => x.EmployeeId)
                .GreaterThan(0)
                .WithMessage("Employee ID must be greater than 0");

            RuleFor(x => x.LeaveRequestDto)
                .NotNull()
                .WithMessage("Leave request details are required")
                .SetValidator(new CreateLeaveRequestDtoValidator());
        }
    }

    public class ApproveLeaveRequestCommandValidator : AbstractValidator<ApproveLeaveRequestCommand>
    {
        public ApproveLeaveRequestCommandValidator()
        {
            RuleFor(x => x.LeaveRequestId)
                .GreaterThan(0)
                .WithMessage("Leave request ID must be greater than 0");

            RuleFor(x => x.ApproverId)
                .GreaterThan(0)
                .WithMessage("Approver ID must be greater than 0");

            RuleFor(x => x.ApproverNotes)
                .MaximumLength(500)
                .WithMessage("Approver notes must not exceed 500 characters");
        }
    }

    public class RejectLeaveRequestCommandValidator : AbstractValidator<RejectLeaveRequestCommand>
    {
        public RejectLeaveRequestCommandValidator()
        {
            RuleFor(x => x.LeaveRequestId)
                .GreaterThan(0)
                .WithMessage("Leave request ID must be greater than 0");

            RuleFor(x => x.RejecterId)
                .GreaterThan(0)
                .WithMessage("Rejecter ID must be greater than 0");

            RuleFor(x => x.RejectionReason)
                .NotEmpty()
                .WithMessage("Rejection reason is required")
                .Length(5, 500)
                .WithMessage("Rejection reason must be between 5 and 500 characters");
        }
    }

    public class CancelLeaveRequestCommandValidator : AbstractValidator<CancelLeaveRequestCommand>
    {
        public CancelLeaveRequestCommandValidator()
        {
            RuleFor(x => x.LeaveRequestId)
                .GreaterThan(0)
                .WithMessage("Leave request ID must be greater than 0");

            RuleFor(x => x.EmployeeId)
                .GreaterThan(0)
                .WithMessage("Employee ID must be greater than 0");

            RuleFor(x => x.CancelReason)
                .NotEmpty()
                .WithMessage("Cancel reason is required")
                .Length(5, 500)
                .WithMessage("Cancel reason must be between 5 and 500 characters");
        }
    }
}
