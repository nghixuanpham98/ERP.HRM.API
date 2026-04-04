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
    public class SalaryAdjustmentDecisionsController : ControllerBase
    {
        private readonly ISalaryAdjustmentDecisionRepository _repository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<SalaryAdjustmentDecisionsController> _logger;

        public SalaryAdjustmentDecisionsController(
            ISalaryAdjustmentDecisionRepository repository,
            IEmployeeRepository employeeRepository,
            IMapper mapper,
            ILogger<SalaryAdjustmentDecisionsController> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _employeeRepository = employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Get all salary adjustment decisions
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                _logger.LogInformation("Getting all salary adjustment decisions");
                var decisions = await _repository.GetAllAsync();
                var dtos = _mapper.Map<IEnumerable<SalaryAdjustmentDecisionDto>>(decisions);
                return Ok(new ApiResponse<IEnumerable<SalaryAdjustmentDecisionDto>>(true, "Lấy danh sách quyết định điều chỉnh lương thành công", dtos));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting salary adjustment decisions");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }

        /// <summary>
        /// Get decisions by employee
        /// </summary>
        [HttpGet("employee/{employeeId}")]
        public async Task<IActionResult> GetByEmployeeId(int employeeId)
        {
            try
            {
                _logger.LogInformation("Getting decisions for employee {EmployeeId}", employeeId);
                var decisions = await _repository.GetByEmployeeIdAsync(employeeId);
                var dtos = _mapper.Map<IEnumerable<SalaryAdjustmentDecisionDto>>(decisions);
                return Ok(new ApiResponse<IEnumerable<SalaryAdjustmentDecisionDto>>(true, "Lấy danh sách quyết định thành công", dtos));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting decisions");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }

        /// <summary>
        /// Get pending decisions
        /// </summary>
        [HttpGet("status/pending")]
        [Authorize(Roles = $"{RoleConstants.Admin},{RoleConstants.HR}")]
        public async Task<IActionResult> GetPendingDecisions()
        {
            try
            {
                _logger.LogInformation("Getting pending salary decisions");
                var decisions = await _repository.GetPendingDecisionsAsync();
                var dtos = _mapper.Map<IEnumerable<SalaryAdjustmentDecisionDto>>(decisions);
                return Ok(new ApiResponse<IEnumerable<SalaryAdjustmentDecisionDto>>(true, "Lấy danh sách quyết định chờ duyệt thành công", dtos));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting pending decisions");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }

        /// <summary>
        /// Get decision by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                _logger.LogInformation("Getting decision {Id}", id);
                var decision = await _repository.GetByIdAsync(id);
                if (decision == null)
                    return NotFound(new ApiResponse<string>(false, "Không tìm thấy quyết định", null));

                var dto = _mapper.Map<SalaryAdjustmentDecisionDto>(decision);
                return Ok(new ApiResponse<SalaryAdjustmentDecisionDto>(true, "Lấy quyết định thành công", dto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting decision");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }

        /// <summary>
        /// Create a new salary adjustment decision
        /// </summary>
        [HttpPost]
        [Authorize(Roles = $"{RoleConstants.Admin},{RoleConstants.HR}")]
        public async Task<IActionResult> Create([FromBody] CreateSalaryAdjustmentDecisionDto createDto)
        {
            try
            {
                _logger.LogInformation("Creating new salary decision for employee {EmployeeId}", createDto.EmployeeId);
                
                var employee = await _employeeRepository.GetByIdAsync(createDto.EmployeeId);
                if (employee == null)
                    return NotFound(new ApiResponse<string>(false, "Không tìm thấy nhân viên", null));

                var decision = new SalaryAdjustmentDecision
                {
                    EmployeeId = createDto.EmployeeId,
                    CreatedByUserId = 1, // Should come from current user context
                    DecisionType = createDto.DecisionType,
                    OldBaseSalary = createDto.OldBaseSalary,
                    NewBaseSalary = createDto.NewBaseSalary,
                    SalaryChange = createDto.NewBaseSalary - createDto.OldBaseSalary,
                    EffectiveDate = createDto.EffectiveDate,
                    Reason = createDto.Reason,
                    Status = "Pending",
                    DecisionDate = DateTime.UtcNow
                };

                await _repository.AddAsync(decision);
                var dto = _mapper.Map<SalaryAdjustmentDecisionDto>(decision);
                return CreatedAtAction(nameof(GetById), new { id = decision.SalaryAdjustmentDecisionId }, dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating decision");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }

        /// <summary>
        /// Approve or reject a salary adjustment decision
        /// </summary>
        [HttpPost("{id}/approve")]
        [Authorize(Roles = RoleConstants.Admin)]
        public async Task<IActionResult> Approve(int id, [FromBody] ApproveSalaryAdjustmentDecisionDto approveDto)
        {
            try
            {
                _logger.LogInformation("Approving/Rejecting decision {Id}", id);
                
                var decision = await _repository.GetByIdAsync(id);
                if (decision == null)
                    return NotFound(new ApiResponse<string>(false, "Không tìm thấy quyết định", null));

                decision.ApprovedByUserId = approveDto.ApprovedByUserId;
                decision.Status = approveDto.Status;
                decision.ApprovalNotes = approveDto.ApprovalNotes;
                decision.ApprovedDate = DateTime.UtcNow;

                if (approveDto.Status == "Approved")
                {
                    decision.EffectiveImplementationDate = DateTime.UtcNow;
                    decision.Status = "Applied";

                    // Update employee base salary
                    var employee = await _employeeRepository.GetByIdAsync(decision.EmployeeId);
                    if (employee != null)
                    {
                        employee.BaseSalary = decision.NewBaseSalary;
                        await _employeeRepository.UpdateAsync(employee);
                    }
                }

                await _repository.UpdateAsync(decision);
                var dto = _mapper.Map<SalaryAdjustmentDecisionDto>(decision);
                return Ok(new ApiResponse<SalaryAdjustmentDecisionDto>(true, "Xử lý quyết định thành công", dto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error approving decision");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }

        /// <summary>
        /// Update a salary adjustment decision
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = $"{RoleConstants.Admin},{RoleConstants.HR}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateSalaryAdjustmentDecisionDto updateDto)
        {
            try
            {
                _logger.LogInformation("Updating decision {Id}", id);
                
                var decision = await _repository.GetByIdAsync(id);
                if (decision == null)
                    return NotFound(new ApiResponse<string>(false, "Không tìm thấy quyết định", null));

                if (decision.Status != "Pending")
                    return BadRequest(new ApiResponse<string>(false, "Chỉ có thể cập nhật quyết định chờ duyệt", null));

                decision.DecisionType = updateDto.DecisionType;
                decision.OldBaseSalary = updateDto.OldBaseSalary;
                decision.NewBaseSalary = updateDto.NewBaseSalary;
                decision.SalaryChange = updateDto.NewBaseSalary - updateDto.OldBaseSalary;
                decision.EffectiveDate = updateDto.EffectiveDate;
                decision.Reason = updateDto.Reason;

                await _repository.UpdateAsync(decision);
                var dto = _mapper.Map<SalaryAdjustmentDecisionDto>(decision);
                return Ok(new ApiResponse<SalaryAdjustmentDecisionDto>(true, "Cập nhật quyết định thành công", dto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating decision");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }

        /// <summary>
        /// Delete a salary adjustment decision
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = RoleConstants.Admin)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                _logger.LogInformation("Deleting decision {Id}", id);
                
                var decision = await _repository.GetByIdAsync(id);
                if (decision == null)
                    return NotFound(new ApiResponse<string>(false, "Không tìm thấy quyết định", null));

                await _repository.DeleteAsync(id);
                return Ok(new ApiResponse<string>(true, "Xóa quyết định thành công", null));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting decision");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }
    }
}
