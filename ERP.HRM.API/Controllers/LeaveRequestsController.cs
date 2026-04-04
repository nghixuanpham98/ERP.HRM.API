using AutoMapper;
using ERP.HRM.Application.Common;
using ERP.HRM.Application.DTOs.HR;
using ERP.HRM.Domain.Constants;
using ERP.HRM.Domain.Entities;
using ERP.HRM.Domain.Interfaces.Repositories;
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
        private readonly ILeaveRequestRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<LeaveRequestsController> _logger;

        public LeaveRequestsController(
            ILeaveRequestRepository repository,
            IMapper mapper,
            ILogger<LeaveRequestsController> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                _logger.LogInformation("Getting all leave requests");
                var requests = await _repository.GetAllAsync();
                var dtos = _mapper.Map<IEnumerable<LeaveRequestDto>>(requests);
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
                var requests = await _repository.GetByEmployeeIdAsync(employeeId);
                var dtos = _mapper.Map<IEnumerable<LeaveRequestDto>>(requests);
                return Ok(new ApiResponse<IEnumerable<LeaveRequestDto>>(true, "Lấy danh sách đơn xin nghỉ phép thành công", dtos));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting leave requests");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }

        [HttpGet("pending")]
        [Authorize(Roles = $"{RoleConstants.Admin},{RoleConstants.HR}")]
        public async Task<IActionResult> GetPendingRequests()
        {
            try
            {
                _logger.LogInformation("Getting pending leave requests");
                var requests = await _repository.GetPendingRequestsAsync();
                var dtos = _mapper.Map<IEnumerable<LeaveRequestDto>>(requests);
                return Ok(new ApiResponse<IEnumerable<LeaveRequestDto>>(true, "Lấy danh sách đơn xin chờ duyệt thành công", dtos));
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
                var dtos = _mapper.Map<IEnumerable<LeaveRequestDto>>(requests);
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
                var request = await _repository.GetByIdAsync(id);
                if (request == null)
                    return NotFound(new ApiResponse<string>(false, "Không tìm thấy đơn xin nghỉ phép", null));

                var dto = _mapper.Map<LeaveRequestDto>(request);
                return Ok(new ApiResponse<LeaveRequestDto>(true, "Lấy đơn xin nghỉ phép thành công", dto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting leave request");
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

                var request = new LeaveRequest
                {
                    EmployeeId = createDto.EmployeeId,
                    LeaveType = createDto.LeaveType,
                    StartDate = DateOnly.FromDateTime(createDto.StartDate),
                    EndDate = DateOnly.FromDateTime(createDto.EndDate),
                    NumberOfDays = createDto.NumberOfDays,
                    Reason = createDto.Reason,
                    EmergencyContact = createDto.EmergencyContact,
                    ApprovalStatus = "Pending",
                    RequestDate = DateTime.UtcNow,
                    CreatedDate = DateTime.UtcNow
                };

                await _repository.AddAsync(request);
                var dto = _mapper.Map<LeaveRequestDto>(request);
                return CreatedAtAction(nameof(GetById), new { id = request.LeaveRequestId }, dto);
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
                var request = await _repository.GetByIdAsync(id);
                if (request == null)
                    return NotFound(new ApiResponse<string>(false, "Không tìm thấy đơn xin nghỉ phép", null));

                request.LeaveType = updateDto.LeaveType;
                request.StartDate = DateOnly.FromDateTime(updateDto.StartDate);
                request.EndDate = DateOnly.FromDateTime(updateDto.EndDate);
                request.NumberOfDays = updateDto.NumberOfDays;
                request.Reason = updateDto.Reason;
                request.EmergencyContact = updateDto.EmergencyContact;
                request.ApprovalStatus = updateDto.ApprovalStatus;
                request.ApprovalRemarks = updateDto.ApprovalRemarks;
                request.ModifiedDate = DateTime.UtcNow;

                await _repository.UpdateAsync(request);
                var dto = _mapper.Map<LeaveRequestDto>(request);
                return Ok(new ApiResponse<LeaveRequestDto>(true, "Cập nhật đơn xin nghỉ phép thành công", dto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating leave request");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }

        [HttpPost("{id}/approve")]
        [Authorize(Roles = $"{RoleConstants.Admin},{RoleConstants.HR}")]
        public async Task<IActionResult> ApproveRequest(int id, [FromBody] string remarks)
        {
            try
            {
                _logger.LogInformation("Approving leave request {Id}", id);
                var request = await _repository.GetByIdAsync(id);
                if (request == null)
                    return NotFound(new ApiResponse<string>(false, "Không tìm thấy đơn xin nghỉ phép", null));

                request.ApprovalStatus = "Approved";
                request.ApprovalDate = DateTime.UtcNow;
                request.ApprovalRemarks = remarks;
                request.ModifiedDate = DateTime.UtcNow;

                await _repository.UpdateAsync(request);
                var dto = _mapper.Map<LeaveRequestDto>(request);
                return Ok(new ApiResponse<LeaveRequestDto>(true, "Duyệt đơn xin nghỉ phép thành công", dto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error approving leave request");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }

        [HttpPost("{id}/reject")]
        [Authorize(Roles = $"{RoleConstants.Admin},{RoleConstants.HR}")]
        public async Task<IActionResult> RejectRequest(int id, [FromBody] string remarks)
        {
            try
            {
                _logger.LogInformation("Rejecting leave request {Id}", id);
                var request = await _repository.GetByIdAsync(id);
                if (request == null)
                    return NotFound(new ApiResponse<string>(false, "Không tìm thấy đơn xin nghỉ phép", null));

                request.ApprovalStatus = "Rejected";
                request.ApprovalDate = DateTime.UtcNow;
                request.ApprovalRemarks = remarks;
                request.ModifiedDate = DateTime.UtcNow;

                await _repository.UpdateAsync(request);
                var dto = _mapper.Map<LeaveRequestDto>(request);
                return Ok(new ApiResponse<LeaveRequestDto>(true, "Từ chối đơn xin nghỉ phép thành công", dto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error rejecting leave request");
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
                _logger.LogError(ex, "Error deleting leave request");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }
    }
}
