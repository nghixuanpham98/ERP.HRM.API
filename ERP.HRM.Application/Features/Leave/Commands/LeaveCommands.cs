using ERP.HRM.Application.DTOs.HR;
using MediatR;

namespace ERP.HRM.Application.Features.Leave.Commands
{
    public class SubmitLeaveRequestCommand : IRequest<LeaveRequestDto>
    {
        public int EmployeeId { get; set; }
        public CreateLeaveRequestDto LeaveRequestDto { get; set; } = null!;
    }

    public class ApproveLeaveRequestCommand : IRequest<LeaveRequestDto>
    {
        public int LeaveRequestId { get; set; }
        public int ApproverId { get; set; }
        public string? ApproverNotes { get; set; }
    }

    public class RejectLeaveRequestCommand : IRequest<LeaveRequestDto>
    {
        public int LeaveRequestId { get; set; }
        public int RejecterId { get; set; }
        public string RejectionReason { get; set; } = string.Empty;
    }

    public class CancelLeaveRequestCommand : IRequest<LeaveRequestDto>
    {
        public int LeaveRequestId { get; set; }
        public int EmployeeId { get; set; }
        public string CancelReason { get; set; } = string.Empty;
    }
}
