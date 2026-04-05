using ERP.HRM.Application.Common;
using ERP.HRM.Application.DTOs.HR;
using ERP.HRM.Application.Features.Leave.Commands;
using ERP.HRM.Application.Features.Leave.Queries;
using ERP.HRM.Application.Interfaces;
using ERP.HRM.Domain.Constants;
using ERP.HRM.Domain.Entities;
using ERP.HRM.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ERP.HRM.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class LeaveRequestsController : ControllerBase
    {
        private readonly ILeaveManagementService _leaveService;
        private readonly ILeaveRequestRepository _repository;
        private readonly IMediator _mediator;
        private readonly ILogger<LeaveRequestsController> _logger;

        public LeaveRequestsController(
            ILeaveManagementService leaveService,
            ILeaveRequestRepository repository,
            IMediator mediator,
            ILogger<LeaveRequestsController> logger)
        {
            _leaveService = leaveService ?? throw new ArgumentNullException(nameof(leaveService));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                _logger.LogInformation("Getting all leave requests");
                var requests = await _repository.GetAllAsync();
                var dtos = requests.Select(r => new LeaveRequestDto
                {
                    LeaveRequestId = r.LeaveRequestId,
                    EmployeeId = r.EmployeeId,
                    LeaveType = r.LeaveType,
                    StartDate = r.StartDate.ToDateTime(TimeOnly.MinValue),
                    EndDate = r.EndDate.ToDateTime(TimeOnly.MinValue),
                    NumberOfDays = r.NumberOfDays,
                    Reason = r.Reason,
                    ApprovalStatus = r.ApprovalStatus
                }).ToList();
                return Ok(new ApiResponse<IEnumerable<LeaveRequestDto>>(true, "Lấy danh sách đơn xin nghỉ phép thành công", dtos));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting leave requests");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }

        [HttpGet("employee/{employeeId}")]
        public async Task<IActionResult> GetByEmployeeId(int employeeId)
        {
            try
            {
                _logger.LogInformation("Getting leave requests for employee {EmployeeId}", employeeId);
                var query = new GetEmployeeLeaveRequestsQuery { EmployeeId = employeeId };
                var result = await _mediator.Send(query);
                return Ok(new ApiResponse<IEnumerable<LeaveRequestDto>>(true, "Lấy danh sách đơn xin nghỉ phép thành công", result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting leave requests for employee {EmployeeId}", employeeId);
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }

        [HttpGet("pending")]
        [Authorize(Roles = $"{RoleConstants.Admin},{RoleConstants.HR}")]
        public async Task<IActionResult> GetPendingRequests([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                _logger.LogInformation("Getting pending leave requests - Page {PageNumber}, Size {PageSize}", pageNumber, pageSize);
                var query = new GetPendingLeaveRequestsQuery { PageNumber = pageNumber, PageSize = pageSize };
                var result = await _mediator.Send(query);
                return Ok(new ApiResponse<IEnumerable<LeaveRequestDto>>(true, "Lấy danh sách đơn xin chờ duyệt thành công", result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting pending requests");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }

        [HttpGet("approved")]
        public async Task<IActionResult> GetApprovedRequests()
        {
            try
            {
                _logger.LogInformation("Getting approved leave requests");
                var requests = await _repository.GetApprovedRequestsAsync();
                var dtos = requests.Select(r => new LeaveRequestDto
                {
                    LeaveRequestId = r.LeaveRequestId,
                    EmployeeId = r.EmployeeId,
                    LeaveType = r.LeaveType,
                    StartDate = r.StartDate.ToDateTime(TimeOnly.MinValue),
                    EndDate = r.EndDate.ToDateTime(TimeOnly.MinValue),
                    NumberOfDays = r.NumberOfDays,
                    Reason = r.Reason,
                    ApprovalStatus = r.ApprovalStatus
                }).ToList();
                return Ok(new ApiResponse<IEnumerable<LeaveRequestDto>>(true, "Lấy danh sách đơn xin được duyệt thành công", dtos));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting approved requests");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                _logger.LogInformation("Getting leave request {Id}", id);
                var query = new GetLeaveRequestQuery { LeaveRequestId = id };
                var result = await _mediator.Send(query);
                if (result == null)
                    return NotFound(new ApiResponse<string>(false, "Không tìm thấy đơn xin nghỉ phép", null));

                return Ok(new ApiResponse<LeaveRequestDto>(true, "Lấy đơn xin nghỉ phép thành công", result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting leave request {Id}", id);
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] CreateLeaveRequestDto createDto)
        {
            try
            {
                _logger.LogInformation("Creating new leave request for employee {EmployeeId}", createDto.EmployeeId);
                var command = new SubmitLeaveRequestCommand { EmployeeId = createDto.EmployeeId, LeaveRequestDto = createDto };
                var result = await _mediator.Send(command);
                return CreatedAtAction(nameof(GetById), new { id = result.LeaveRequestId }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating leave request");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = $"{RoleConstants.Admin},{RoleConstants.HR}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateLeaveRequestDto updateDto)
        {
            try
            {
                _logger.LogInformation("Updating leave request {Id}", id);
                return BadRequest(new ApiResponse<string>(false, "Use specific endpoints for approval/rejection", null));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating leave request");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }

        [HttpPost("{id}/approve")]
        [Authorize(Roles = $"{RoleConstants.Admin},{RoleConstants.HR}")]
        public async Task<IActionResult> ApproveRequest(int id, [FromBody] ApproveLeaveRequestDto approveDto)
        {
            try
            {
                _logger.LogInformation("Approving leave request {Id}", id);
                var command = new ApproveLeaveRequestCommand 
                { 
                    LeaveRequestId = id,
                    ApproverId = approveDto.ApproverId,
                    ApproverNotes = approveDto.ApproverNotes
                };
                var result = await _mediator.Send(command);
                return Ok(new ApiResponse<LeaveRequestDto>(true, "Duyệt đơn xin nghỉ phép thành công", result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error approving leave request {Id}", id);
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }

        [HttpPost("{id}/reject")]
        [Authorize(Roles = $"{RoleConstants.Admin},{RoleConstants.HR}")]
        public async Task<IActionResult> RejectRequest(int id, [FromBody] RejectLeaveRequestDto rejectDto)
        {
            try
            {
                _logger.LogInformation("Rejecting leave request {Id}", id);
                var command = new RejectLeaveRequestCommand 
                { 
                    LeaveRequestId = id,
                    RejecterId = rejectDto.RejecterId,
                    RejectionReason = rejectDto.RejectionReason
                };
                var result = await _mediator.Send(command);
                return Ok(new ApiResponse<LeaveRequestDto>(true, "Từ chối đơn xin nghỉ phép thành công", result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error rejecting leave request {Id}", id);
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }

        [HttpPost("{id}/cancel")]
        [Authorize]
        public async Task<IActionResult> CancelRequest(int id, [FromBody] CancelLeaveRequestDto cancelDto)
        {
            try
            {
                _logger.LogInformation("Cancelling leave request {Id}", id);
                var command = new CancelLeaveRequestCommand 
                { 
                    LeaveRequestId = id,
                    EmployeeId = cancelDto.EmployeeId,
                    CancelReason = cancelDto.CancelReason
                };
                var result = await _mediator.Send(command);
                return Ok(new ApiResponse<LeaveRequestDto>(true, "Hủy đơn xin nghỉ phép thành công", result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cancelling leave request {Id}", id);
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = $"{RoleConstants.Admin}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                _logger.LogInformation("Deleting leave request {Id}", id);
                var request = await _repository.GetByIdAsync(id);
                if (request == null)
                    return NotFound(new ApiResponse<string>(false, "Không tìm thấy đơn xin nghỉ phép", null));

                await _repository.DeleteAsync(id);
                return Ok(new ApiResponse<string>(true, "Xóa đơn xin nghỉ phép thành công", null));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting leave request {Id}", id);
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }

        /// <summary>
        /// Get leave balance for an employee in a specific year
        /// </summary>
        [HttpGet("balance/{employeeId}/{year}")]
        public async Task<IActionResult> GetLeaveBalance(int employeeId, int year)
        {
            try
            {
                _logger.LogInformation("Getting leave balance for employee {EmployeeId}, year {Year}", employeeId, year);
                var query = new GetLeaveBalanceQuery { EmployeeId = employeeId, Year = year };
                var result = await _mediator.Send(query);
                return Ok(new ApiResponse<IEnumerable<object>>(true, "Lấy thông tin số ngày phép thành công", result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting leave balance");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }

        /// <summary>
        /// Get remaining leave days for an employee in a specific year
        /// </summary>
        [HttpGet("remaining/{employeeId}/{year}")]
        public async Task<IActionResult> GetRemainingLeaveDays(int employeeId, int year)
        {
            try
            {
                _logger.LogInformation("Getting remaining leave days for employee {EmployeeId}, year {Year}", employeeId, year);
                var query = new CalculateRemainingLeaveDaysQuery { EmployeeId = employeeId, Year = year };
                var result = await _mediator.Send(query);
                return Ok(new ApiResponse<decimal>(true, "Lấy số ngày phép còn lại thành công", result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting remaining leave days");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }

        /// <summary>
        /// Get leave history for an employee in a specific year
        /// </summary>
        [HttpGet("history/{employeeId}/{year}")]
        public async Task<IActionResult> GetLeaveHistory(int employeeId, int year)
        {
            try
            {
                _logger.LogInformation("Getting leave history for employee {EmployeeId}, year {Year}", employeeId, year);
                var query = new GetLeaveHistoryQuery { EmployeeId = employeeId, Year = year };
                var result = await _mediator.Send(query);
                return Ok(new ApiResponse<IEnumerable<LeaveRequestDto>>(true, "Lấy lịch sử đơn xin thành công", result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting leave history");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }
    }
}
