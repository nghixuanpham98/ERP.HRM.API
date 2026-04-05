using AutoMapper;
using ERP.HRM.Application.Common;
using ERP.HRM.Application.DTOs.HR;
using ERP.HRM.Application.Interfaces;
using ERP.HRM.Domain.Entities;
using ERP.HRM.Domain.Exceptions;
using ERP.HRM.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Logging;

namespace ERP.HRM.Application.Services
{
    public class LeaveManagementService : ILeaveManagementService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<LeaveManagementService> _logger;

        public LeaveManagementService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<LeaveManagementService> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<LeaveRequestDto> SubmitLeaveRequestAsync(CreateLeaveRequestDto dto, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Submitting leave request");

                await ValidateLeaveRequestAsync(dto, cancellationToken);

                var numberOfDays = (int)(dto.EndDate.Date - dto.StartDate.Date).TotalDays + 1;
                var leaveRequest = new LeaveRequest
                {
                    EmployeeId = dto.EmployeeId,
                    StartDate = DateOnly.FromDateTime(dto.StartDate),
                    EndDate = DateOnly.FromDateTime(dto.EndDate),
                    LeaveType = dto.LeaveType,
                    Reason = dto.Reason,
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
                _logger.LogError(ex, "Error submitting leave request");
                throw;
            }
        }

        public async Task<LeaveRequestDto> ApproveLeaveRequestAsync(int leaveRequestId, string? approverNotes = null, CancellationToken cancellationToken = default)
        {
            try
            {
                var leaveRequest = await _unitOfWork.LeaveRequestRepository.GetByIdAsync(leaveRequestId);
                if (leaveRequest == null)
                    throw new NotFoundException($"Leave request with Id {leaveRequestId} not found");

                if (leaveRequest.ApprovalStatus != "Pending")
                    throw new BusinessRuleException($"Cannot approve leave request with status {leaveRequest.ApprovalStatus}");

                leaveRequest.ApprovalStatus = "Approved";
                leaveRequest.ApprovalRemarks = approverNotes;
                leaveRequest.ApprovalDate = DateTime.UtcNow;

                await _unitOfWork.LeaveRequestRepository.UpdateAsync(leaveRequest);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Leave request {LeaveRequestId} approved successfully", leaveRequestId);
                return _mapper.Map<LeaveRequestDto>(leaveRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error approving leave request {LeaveRequestId}", leaveRequestId);
                throw;
            }
        }

        public async Task<LeaveRequestDto> RejectLeaveRequestAsync(int leaveRequestId, string rejectionReason, CancellationToken cancellationToken = default)
        {
            try
            {
                var leaveRequest = await _unitOfWork.LeaveRequestRepository.GetByIdAsync(leaveRequestId);
                if (leaveRequest == null)
                    throw new NotFoundException($"Leave request with Id {leaveRequestId} not found");

                if (leaveRequest.ApprovalStatus != "Pending")
                    throw new BusinessRuleException($"Cannot reject leave request with status {leaveRequest.ApprovalStatus}");

                leaveRequest.ApprovalStatus = "Rejected";
                leaveRequest.ApprovalRemarks = rejectionReason;
                leaveRequest.ApprovalDate = DateTime.UtcNow;

                await _unitOfWork.LeaveRequestRepository.UpdateAsync(leaveRequest);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Leave request {LeaveRequestId} rejected successfully", leaveRequestId);
                return _mapper.Map<LeaveRequestDto>(leaveRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error rejecting leave request {LeaveRequestId}", leaveRequestId);
                throw;
            }
        }

        public async Task<LeaveRequestDto> CancelLeaveRequestAsync(int leaveRequestId, string cancelReason, CancellationToken cancellationToken = default)
        {
            try
            {
                var leaveRequest = await _unitOfWork.LeaveRequestRepository.GetByIdAsync(leaveRequestId);
                if (leaveRequest == null)
                    throw new NotFoundException($"Leave request with Id {leaveRequestId} not found");

                if (leaveRequest.ApprovalStatus == "Cancelled")
                    throw new BusinessRuleException($"Cannot cancel leave request with status {leaveRequest.ApprovalStatus}");

                leaveRequest.ApprovalStatus = "Cancelled";

                await _unitOfWork.LeaveRequestRepository.UpdateAsync(leaveRequest);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Leave request {LeaveRequestId} cancelled successfully", leaveRequestId);
                return _mapper.Map<LeaveRequestDto>(leaveRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cancelling leave request {LeaveRequestId}", leaveRequestId);
                throw;
            }
        }

        public async Task<LeaveRequestDto> GetLeaveRequestAsync(int leaveRequestId, CancellationToken cancellationToken = default)
        {
            var leaveRequest = await _unitOfWork.LeaveRequestRepository.GetByIdAsync(leaveRequestId);
            if (leaveRequest == null)
                throw new NotFoundException($"Leave request with Id {leaveRequestId} not found");

            return _mapper.Map<LeaveRequestDto>(leaveRequest);
        }

        public async Task<PagedResult<LeaveRequestDto>> GetEmployeeLeaveRequestsAsync(int employeeId, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            var leaveRequests = await _unitOfWork.LeaveRequestRepository.GetByEmployeeIdAsync(employeeId);
            var totalCount = leaveRequests.Count();
            var paginatedRequests = leaveRequests
                .OrderByDescending(lr => lr.RequestDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);

            return new PagedResult<LeaveRequestDto>
            {
                Items = _mapper.Map<IEnumerable<LeaveRequestDto>>(paginatedRequests),
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<LeaveBalanceDto> GetLeaveBalanceAsync(int employeeId, int year, CancellationToken cancellationToken = default)
        {
            var balances = await _unitOfWork.LeaveBalanceRepository.GetAllByEmployeeAsync(employeeId);
            var yearBalance = balances.FirstOrDefault(b => b.Year == year);

            if (yearBalance == null)
                throw new NotFoundException($"Leave balance not found for employee {employeeId} in year {year}");

            return _mapper.Map<LeaveBalanceDto>(yearBalance);
        }

        public async Task<IEnumerable<LeaveBalanceHistoryDto>> GetLeaveHistoryAsync(int employeeId, int year, CancellationToken cancellationToken = default)
        {
            var leaveRequests = await _unitOfWork.LeaveRequestRepository.GetByEmployeeIdAsync(employeeId);
            var yearRequests = leaveRequests
                .Where(lr => lr.StartDate.Year == year)
                .OrderByDescending(lr => lr.StartDate);

            return _mapper.Map<IEnumerable<LeaveBalanceHistoryDto>>(yearRequests);
        }

        public async Task<bool> ValidateLeaveRequestAsync(CreateLeaveRequestDto dto, CancellationToken cancellationToken = default)
        {
            if (dto.StartDate >= dto.EndDate)
                throw new BusinessRuleException("Start date must be before end date");

            if (dto.StartDate < DateTime.Today)
                throw new BusinessRuleException("Cannot submit leave request for past dates");

            var employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(dto.EmployeeId);
            if (employee == null)
                throw new NotFoundException($"Employee with Id {dto.EmployeeId} not found");

            var leaveDays = (int)(dto.EndDate.Date - dto.StartDate.Date).TotalDays + 1;
            var remainingDays = await CalculateRemainingLeaveDaysAsync(dto.EmployeeId, dto.StartDate.Year, cancellationToken);

            if (leaveDays > remainingDays)
                throw new BusinessRuleException($"Insufficient leave balance. Required: {leaveDays} days, Available: {remainingDays} days");

            return true;
        }

        public async Task<PagedResult<LeaveRequestDto>> GetPendingLeaveRequestsAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            var pendingRequests = await _unitOfWork.LeaveRequestRepository.GetPendingRequestsAsync();
            var totalCount = pendingRequests.Count();
            var paginatedRequests = pendingRequests
                .OrderByDescending(lr => lr.RequestDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);

            return new PagedResult<LeaveRequestDto>
            {
                Items = _mapper.Map<IEnumerable<LeaveRequestDto>>(paginatedRequests),
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<decimal> CalculateRemainingLeaveDaysAsync(int employeeId, int year, CancellationToken cancellationToken = default)
        {
            var balances = await _unitOfWork.LeaveBalanceRepository.GetAllByEmployeeAsync(employeeId);
            var yearBalance = balances.FirstOrDefault(b => b.Year == year);

            if (yearBalance == null)
                return 0;

            return yearBalance.AllocatedDays - yearBalance.UsedDays;
        }
    }
}
