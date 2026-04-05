using AutoMapper;
using ERP.HRM.Application.DTOs.HR;
using ERP.HRM.Application.Features.Leave.Commands;
using ERP.HRM.Domain.Entities;
using ERP.HRM.Domain.Exceptions;
using ERP.HRM.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ERP.HRM.Application.Features.Leave.Handlers
{
    public class SubmitLeaveRequestCommandHandler : IRequestHandler<SubmitLeaveRequestCommand, LeaveRequestDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<SubmitLeaveRequestCommandHandler> _logger;

        public SubmitLeaveRequestCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<SubmitLeaveRequestCommandHandler> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<LeaveRequestDto> Handle(SubmitLeaveRequestCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Submitting leave request for employee {EmployeeId}", request.EmployeeId);

                var employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(request.EmployeeId);
                if (employee == null)
                    throw new NotFoundException($"Employee with Id {request.EmployeeId} not found");

                var numberOfDays = (int)(request.LeaveRequestDto.EndDate.Date - request.LeaveRequestDto.StartDate.Date).TotalDays + 1;
                var leaveRequest = new LeaveRequest
                {
                    EmployeeId = request.EmployeeId,
                    StartDate = DateOnly.FromDateTime(request.LeaveRequestDto.StartDate),
                    EndDate = DateOnly.FromDateTime(request.LeaveRequestDto.EndDate),
                    LeaveType = request.LeaveRequestDto.LeaveType,
                    Reason = request.LeaveRequestDto.Reason,
                    ApprovalStatus = "Pending",
                    RequestDate = DateTime.UtcNow,
                    NumberOfDays = numberOfDays,
                    RequestNumber = $"LR-{DateTime.UtcNow:yyyyMMddHHmmss}"
                };

                await _unitOfWork.LeaveRequestRepository.AddAsync(leaveRequest);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Leave request submitted successfully");

                return _mapper.Map<LeaveRequestDto>(leaveRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error submitting leave request for employee {EmployeeId}", request.EmployeeId);
                throw;
            }
        }
    }

    public class ApproveLeaveRequestCommandHandler : IRequestHandler<ApproveLeaveRequestCommand, LeaveRequestDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<ApproveLeaveRequestCommandHandler> _logger;

        public ApproveLeaveRequestCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<ApproveLeaveRequestCommandHandler> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<LeaveRequestDto> Handle(ApproveLeaveRequestCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Approving leave request {LeaveRequestId}", request.LeaveRequestId);

                var leaveRequest = await _unitOfWork.LeaveRequestRepository.GetByIdAsync(request.LeaveRequestId);
                if (leaveRequest == null)
                    throw new NotFoundException($"Leave request with Id {request.LeaveRequestId} not found");

                if (leaveRequest.ApprovalStatus != "Pending")
                    throw new BusinessRuleException($"Cannot approve leave request with status {leaveRequest.ApprovalStatus}");

                leaveRequest.ApprovalStatus = "Approved";
                leaveRequest.ApprovedByUserId = request.ApproverId;
                leaveRequest.ApprovalRemarks = request.ApproverNotes;
                leaveRequest.ApprovalDate = DateTime.UtcNow;

                await _unitOfWork.LeaveRequestRepository.UpdateAsync(leaveRequest);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Leave request {LeaveRequestId} approved successfully", request.LeaveRequestId);

                return _mapper.Map<LeaveRequestDto>(leaveRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error approving leave request {LeaveRequestId}", request.LeaveRequestId);
                throw;
            }
        }
    }

    public class RejectLeaveRequestCommandHandler : IRequestHandler<RejectLeaveRequestCommand, LeaveRequestDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<RejectLeaveRequestCommandHandler> _logger;

        public RejectLeaveRequestCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<RejectLeaveRequestCommandHandler> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<LeaveRequestDto> Handle(RejectLeaveRequestCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Rejecting leave request {LeaveRequestId}", request.LeaveRequestId);

                var leaveRequest = await _unitOfWork.LeaveRequestRepository.GetByIdAsync(request.LeaveRequestId);
                if (leaveRequest == null)
                    throw new NotFoundException($"Leave request with Id {request.LeaveRequestId} not found");

                if (leaveRequest.ApprovalStatus != "Pending")
                    throw new BusinessRuleException($"Cannot reject leave request with status {leaveRequest.ApprovalStatus}");

                leaveRequest.ApprovalStatus = "Rejected";
                leaveRequest.ApprovedByUserId = request.RejecterId;
                leaveRequest.ApprovalRemarks = request.RejectionReason;
                leaveRequest.ApprovalDate = DateTime.UtcNow;

                await _unitOfWork.LeaveRequestRepository.UpdateAsync(leaveRequest);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Leave request {LeaveRequestId} rejected successfully", request.LeaveRequestId);

                return _mapper.Map<LeaveRequestDto>(leaveRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error rejecting leave request {LeaveRequestId}", request.LeaveRequestId);
                throw;
            }
        }
    }

    public class CancelLeaveRequestCommandHandler : IRequestHandler<CancelLeaveRequestCommand, LeaveRequestDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<CancelLeaveRequestCommandHandler> _logger;

        public CancelLeaveRequestCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<CancelLeaveRequestCommandHandler> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<LeaveRequestDto> Handle(CancelLeaveRequestCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Cancelling leave request {LeaveRequestId}", request.LeaveRequestId);

                var leaveRequest = await _unitOfWork.LeaveRequestRepository.GetByIdAsync(request.LeaveRequestId);
                if (leaveRequest == null)
                    throw new NotFoundException($"Leave request with Id {request.LeaveRequestId} not found");

                if (leaveRequest.EmployeeId != request.EmployeeId)
                    throw new BusinessRuleException("Unauthorized: Cannot cancel someone else's leave request");

                if (leaveRequest.ApprovalStatus != "Approved" && leaveRequest.ApprovalStatus != "Pending")
                    throw new BusinessRuleException($"Cannot cancel leave request with status {leaveRequest.ApprovalStatus}");

                leaveRequest.ApprovalStatus = "Cancelled";

                await _unitOfWork.LeaveRequestRepository.UpdateAsync(leaveRequest);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Leave request {LeaveRequestId} cancelled successfully", request.LeaveRequestId);

                return _mapper.Map<LeaveRequestDto>(leaveRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cancelling leave request {LeaveRequestId}", request.LeaveRequestId);
                throw;
            }
        }
    }
}
