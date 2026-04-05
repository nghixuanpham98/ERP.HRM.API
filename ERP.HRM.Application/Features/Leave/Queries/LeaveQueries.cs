using ERP.HRM.Application.DTOs.HR;
using MediatR;

namespace ERP.HRM.Application.Features.Leave.Queries
{
    public class GetLeaveRequestQuery : IRequest<LeaveRequestDto>
    {
        public int LeaveRequestId { get; set; }
    }

    public class GetEmployeeLeaveRequestsQuery : IRequest<IEnumerable<LeaveRequestDto>>
    {
        public int EmployeeId { get; set; }
    }

    public class GetLeaveBalanceQuery : IRequest<IEnumerable<object>>
    {
        public int EmployeeId { get; set; }
        public int Year { get; set; }
    }

    public class GetLeaveHistoryQuery : IRequest<IEnumerable<LeaveRequestDto>>
    {
        public int EmployeeId { get; set; }
        public int Year { get; set; }
    }

    public class GetPendingLeaveRequestsQuery : IRequest<IEnumerable<LeaveRequestDto>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }

    public class CalculateRemainingLeaveDaysQuery : IRequest<decimal>
    {
        public int EmployeeId { get; set; }
        public int Year { get; set; }
    }
}
