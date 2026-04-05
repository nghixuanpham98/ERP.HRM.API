using AutoMapper;
using ERP.HRM.Application.DTOs.HR;
using ERP.HRM.Application.Features.Leave.Queries;
using ERP.HRM.Domain.Exceptions;
using ERP.HRM.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ERP.HRM.Application.Features.Leave.Handlers
{
    public class GetLeaveRequestQueryHandler : IRequestHandler<GetLeaveRequestQuery, LeaveRequestDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<GetLeaveRequestQueryHandler> _logger;

        public GetLeaveRequestQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetLeaveRequestQueryHandler> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<LeaveRequestDto> Handle(GetLeaveRequestQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Fetching leave request {LeaveRequestId}", request.LeaveRequestId);

            var leaveRequest = await _unitOfWork.LeaveRequestRepository.GetByIdAsync(request.LeaveRequestId);
            if (leaveRequest == null)
                throw new NotFoundException($"Leave request with Id {request.LeaveRequestId} not found");

            return _mapper.Map<LeaveRequestDto>(leaveRequest);
        }
    }

    public class GetEmployeeLeaveRequestsQueryHandler : IRequestHandler<GetEmployeeLeaveRequestsQuery, IEnumerable<LeaveRequestDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<GetEmployeeLeaveRequestsQueryHandler> _logger;

        public GetEmployeeLeaveRequestsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetEmployeeLeaveRequestsQueryHandler> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<LeaveRequestDto>> Handle(GetEmployeeLeaveRequestsQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Fetching leave requests for employee {EmployeeId}", request.EmployeeId);

            var leaveRequests = await _unitOfWork.LeaveRequestRepository.GetByEmployeeIdAsync(request.EmployeeId);
            return _mapper.Map<IEnumerable<LeaveRequestDto>>(leaveRequests);
        }
    }

    public class GetLeaveBalanceQueryHandler : IRequestHandler<GetLeaveBalanceQuery, IEnumerable<object>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GetLeaveBalanceQueryHandler> _logger;

        public GetLeaveBalanceQueryHandler(IUnitOfWork unitOfWork, ILogger<GetLeaveBalanceQueryHandler> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger;
        }

        public async Task<IEnumerable<object>> Handle(GetLeaveBalanceQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Fetching leave balance for employee {EmployeeId}, year {Year}", request.EmployeeId, request.Year);

            var balances = await _unitOfWork.LeaveBalanceRepository.GetAllByEmployeeAsync(request.EmployeeId);
            var yearBalances = balances.Where(b => b.Year == request.Year).ToList();

            return yearBalances.Select(b => new { b.LeaveType, b.AllocatedDays, b.UsedDays, RemainingDays = b.AllocatedDays - b.UsedDays }).Cast<object>().ToList();
        }
    }

    public class GetLeaveHistoryQueryHandler : IRequestHandler<GetLeaveHistoryQuery, IEnumerable<LeaveRequestDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<GetLeaveHistoryQueryHandler> _logger;

        public GetLeaveHistoryQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetLeaveHistoryQueryHandler> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<LeaveRequestDto>> Handle(GetLeaveHistoryQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Fetching leave history for employee {EmployeeId}, year {Year}", request.EmployeeId, request.Year);

            var leaveRequests = await _unitOfWork.LeaveRequestRepository.GetByEmployeeIdAsync(request.EmployeeId);
            var yearRequests = leaveRequests
                .Where(lr => lr.StartDate.Year == request.Year)
                .OrderByDescending(lr => lr.StartDate);

            return _mapper.Map<IEnumerable<LeaveRequestDto>>(yearRequests);
        }
    }

    public class GetPendingLeaveRequestsQueryHandler : IRequestHandler<GetPendingLeaveRequestsQuery, IEnumerable<LeaveRequestDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<GetPendingLeaveRequestsQueryHandler> _logger;

        public GetPendingLeaveRequestsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetPendingLeaveRequestsQueryHandler> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<LeaveRequestDto>> Handle(GetPendingLeaveRequestsQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Fetching pending leave requests. Page: {PageNumber}, Size: {PageSize}", request.PageNumber, request.PageSize);

            var pendingRequests = await _unitOfWork.LeaveRequestRepository.GetPendingRequestsAsync();
            var paginatedRequests = pendingRequests
                .OrderByDescending(lr => lr.RequestDate)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize);

            return _mapper.Map<IEnumerable<LeaveRequestDto>>(paginatedRequests);
        }
    }

    public class CalculateRemainingLeaveDaysQueryHandler : IRequestHandler<CalculateRemainingLeaveDaysQuery, decimal>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CalculateRemainingLeaveDaysQueryHandler> _logger;

        public CalculateRemainingLeaveDaysQueryHandler(IUnitOfWork unitOfWork, ILogger<CalculateRemainingLeaveDaysQueryHandler> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger;
        }

        public async Task<decimal> Handle(CalculateRemainingLeaveDaysQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Calculating remaining leave days for employee {EmployeeId}, year {Year}", request.EmployeeId, request.Year);

            var balances = await _unitOfWork.LeaveBalanceRepository.GetAllByEmployeeAsync(request.EmployeeId);
            var yearBalance = balances.FirstOrDefault(b => b.Year == request.Year);

            if (yearBalance == null)
                return 0;

            return yearBalance.AllocatedDays - yearBalance.UsedDays;
        }
    }
}
