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
    public class ResignationDecisionsController : ControllerBase
    {
        private readonly IResignationDecisionRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<ResignationDecisionsController> _logger;

        public ResignationDecisionsController(
            IResignationDecisionRepository repository,
            IMapper mapper,
            ILogger<ResignationDecisionsController> logger)
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
                _logger.LogInformation("Getting all resignation decisions");
                var decisions = await _repository.GetAllAsync();
                var dtos = _mapper.Map<IEnumerable<ResignationDecisionDto>>(decisions);
                return Ok(new ApiResponse<IEnumerable<ResignationDecisionDto>>(true, "Lấy danh sách quyết định nghỉ việc thành công", dtos));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting resignation decisions");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }

        [HttpGet("employee/{employeeId}")]
        public async Task<IActionResult> GetByEmployeeId(int employeeId)
        {
            try
            {
                _logger.LogInformation("Getting resignation decisions for employee {EmployeeId}", employeeId);
                var decisions = await _repository.GetByEmployeeIdAsync(employeeId);
                var dtos = _mapper.Map<IEnumerable<ResignationDecisionDto>>(decisions);
                return Ok(new ApiResponse<IEnumerable<ResignationDecisionDto>>(true, "Lấy danh sách quyết định thành công", dtos));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting decisions");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }

        [HttpGet("pending")]
        [Authorize(Roles = $"{RoleConstants.Admin},{RoleConstants.HR}")]
        public async Task<IActionResult> GetPendingDecisions()
        {
            try
            {
                _logger.LogInformation("Getting pending resignation decisions");
                var decisions = await _repository.GetPendingDecisionsAsync();
                var dtos = _mapper.Map<IEnumerable<ResignationDecisionDto>>(decisions);
                return Ok(new ApiResponse<IEnumerable<ResignationDecisionDto>>(true, "Lấy danh sách quyết định chờ duyệt thành công", dtos));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting pending decisions");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                _logger.LogInformation("Getting resignation decision {Id}", id);
                var decision = await _repository.GetByIdAsync(id);
                if (decision == null)
                    return NotFound(new ApiResponse<string>(false, "Không tìm thấy quyết định", null));

                var dto = _mapper.Map<ResignationDecisionDto>(decision);
                return Ok(new ApiResponse<ResignationDecisionDto>(true, "Lấy quyết định thành công", dto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting resignation decision");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }

        [HttpPost]
        [Authorize(Roles = $"{RoleConstants.Admin},{RoleConstants.HR}")]
        public async Task<IActionResult> Create([FromBody] CreateResignationDecisionDto createDto)
        {
            try
            {
                _logger.LogInformation("Creating new resignation decision for employee {EmployeeId}", createDto.EmployeeId);

                var decision = new ResignationDecision
                {
                    EmployeeId = createDto.EmployeeId,
                    ResignationType = createDto.ResignationType,
                    NoticeDate = createDto.NoticeDate,
                    EffectiveDate = createDto.EffectiveDate,
                    Reason = createDto.Reason,
                    DetailedReason = createDto.DetailedReason,
                    Status = "Pending",
                    SettlementAmount = createDto.SettlementAmount,
                    CreatedDate = DateTime.UtcNow
                };

                await _repository.AddAsync(decision);
                var dto = _mapper.Map<ResignationDecisionDto>(decision);
                return CreatedAtAction(nameof(GetById), new { id = decision.ResignationDecisionId }, dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating resignation decision");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = $"{RoleConstants.Admin},{RoleConstants.HR}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateResignationDecisionDto updateDto)
        {
            try
            {
                _logger.LogInformation("Updating resignation decision {Id}", id);
                var decision = await _repository.GetByIdAsync(id);
                if (decision == null)
                    return NotFound(new ApiResponse<string>(false, "Không tìm thấy quyết định", null));

                decision.ResignationType = updateDto.ResignationType;
                decision.NoticeDate = updateDto.NoticeDate;
                decision.EffectiveDate = updateDto.EffectiveDate;
                decision.Reason = updateDto.Reason;
                decision.DetailedReason = updateDto.DetailedReason;
                decision.Status = updateDto.Status;
                decision.SettlementAmount = updateDto.SettlementAmount;
                decision.FinalPaymentDate = updateDto.FinalPaymentDate;
                decision.ModifiedDate = DateTime.UtcNow;

                await _repository.UpdateAsync(decision);
                var dto = _mapper.Map<ResignationDecisionDto>(decision);
                return Ok(new ApiResponse<ResignationDecisionDto>(true, "Cập nhật quyết định thành công", dto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating resignation decision");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }

        [HttpPost("{id}/approve")]
        [Authorize(Roles = $"{RoleConstants.Admin},{RoleConstants.HR}")]
        public async Task<IActionResult> ApproveDecision(int id, [FromBody] string remarks)
        {
            try
            {
                _logger.LogInformation("Approving resignation decision {Id}", id);
                var decision = await _repository.GetByIdAsync(id);
                if (decision == null)
                    return NotFound(new ApiResponse<string>(false, "Không tìm thấy quyết định", null));

                decision.Status = "Approved";
                decision.ApprovalDate = DateTime.UtcNow;
                decision.ModifiedDate = DateTime.UtcNow;

                await _repository.UpdateAsync(decision);
                var dto = _mapper.Map<ResignationDecisionDto>(decision);
                return Ok(new ApiResponse<ResignationDecisionDto>(true, "Duyệt quyết định thành công", dto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error approving resignation decision");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = $"{RoleConstants.Admin}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                _logger.LogInformation("Deleting resignation decision {Id}", id);
                var decision = await _repository.GetByIdAsync(id);
                if (decision == null)
                    return NotFound(new ApiResponse<string>(false, "Không tìm thấy quyết định", null));

                await _repository.DeleteAsync(id);
                return Ok(new ApiResponse<string>(true, "Xóa quyết định thành công", null));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting resignation decision");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }
    }
}
