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
    public class PersonnelTransfersController : ControllerBase
    {
        private readonly IPersonnelTransferRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<PersonnelTransfersController> _logger;

        public PersonnelTransfersController(
            IPersonnelTransferRepository repository,
            IMapper mapper,
            ILogger<PersonnelTransfersController> logger)
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
                _logger.LogInformation("Getting all personnel transfers");
                var transfers = await _repository.GetAllAsync();
                var dtos = _mapper.Map<IEnumerable<PersonnelTransferDto>>(transfers);
                return Ok(new ApiResponse<IEnumerable<PersonnelTransferDto>>(true, "Lấy danh sách thuyên chuyển thành công", dtos));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting personnel transfers");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }

        [HttpGet("employee/{employeeId}")]
        public async Task<IActionResult> GetByEmployeeId(int employeeId)
        {
            try
            {
                _logger.LogInformation("Getting transfers for employee {EmployeeId}", employeeId);
                var transfers = await _repository.GetByEmployeeIdAsync(employeeId);
                var dtos = _mapper.Map<IEnumerable<PersonnelTransferDto>>(transfers);
                return Ok(new ApiResponse<IEnumerable<PersonnelTransferDto>>(true, "Lấy danh sách thuyên chuyển thành công", dtos));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting transfers");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }

        [HttpGet("pending")]
        [Authorize(Roles = $"{RoleConstants.Admin},{RoleConstants.HR}")]
        public async Task<IActionResult> GetPendingTransfers()
        {
            try
            {
                _logger.LogInformation("Getting pending personnel transfers");
                var transfers = await _repository.GetPendingTransfersAsync();
                var dtos = _mapper.Map<IEnumerable<PersonnelTransferDto>>(transfers);
                return Ok(new ApiResponse<IEnumerable<PersonnelTransferDto>>(true, "Lấy danh sách thuyên chuyển chờ duyệt thành công", dtos));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting pending transfers");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                _logger.LogInformation("Getting transfer {Id}", id);
                var transfer = await _repository.GetByIdAsync(id);
                if (transfer == null)
                    return NotFound(new ApiResponse<string>(false, "Không tìm thấy thuyên chuyển", null));

                var dto = _mapper.Map<PersonnelTransferDto>(transfer);
                return Ok(new ApiResponse<PersonnelTransferDto>(true, "Lấy thuyên chuyển thành công", dto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting transfer");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }

        [HttpPost]
        [Authorize(Roles = $"{RoleConstants.Admin},{RoleConstants.HR}")]
        public async Task<IActionResult> Create([FromBody] CreatePersonnelTransferDto createDto)
        {
            try
            {
                _logger.LogInformation("Creating new transfer for employee {EmployeeId}", createDto.EmployeeId);

                var transfer = new PersonnelTransfer
                {
                    EmployeeId = createDto.EmployeeId,
                    FromDepartmentId = createDto.FromDepartmentId,
                    ToDepartmentId = createDto.ToDepartmentId,
                    FromPositionId = createDto.FromPositionId,
                    ToPositionId = createDto.ToPositionId,
                    TransferType = createDto.TransferType,
                    OldSalary = createDto.OldSalary,
                    NewSalary = createDto.NewSalary,
                    EffectiveDate = createDto.EffectiveDate,
                    ApprovalStatus = "Pending",
                    CreatedDate = DateTime.UtcNow
                };

                await _repository.AddAsync(transfer);
                var dto = _mapper.Map<PersonnelTransferDto>(transfer);
                return CreatedAtAction(nameof(GetById), new { id = transfer.PersonnelTransferId }, dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating transfer");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = $"{RoleConstants.Admin},{RoleConstants.HR}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdatePersonnelTransferDto updateDto)
        {
            try
            {
                _logger.LogInformation("Updating transfer {Id}", id);
                var transfer = await _repository.GetByIdAsync(id);
                if (transfer == null)
                    return NotFound(new ApiResponse<string>(false, "Không tìm thấy thuyên chuyển", null));

                transfer.FromDepartmentId = updateDto.FromDepartmentId;
                transfer.ToDepartmentId = updateDto.ToDepartmentId;
                transfer.FromPositionId = updateDto.FromPositionId;
                transfer.ToPositionId = updateDto.ToPositionId;
                transfer.TransferType = updateDto.TransferType;
                transfer.OldSalary = updateDto.OldSalary;
                transfer.NewSalary = updateDto.NewSalary;
                transfer.EffectiveDate = updateDto.EffectiveDate;
                transfer.ApprovalStatus = updateDto.ApprovalStatus;
                transfer.ModifiedDate = DateTime.UtcNow;

                await _repository.UpdateAsync(transfer);
                var dto = _mapper.Map<PersonnelTransferDto>(transfer);
                return Ok(new ApiResponse<PersonnelTransferDto>(true, "Cập nhật thuyên chuyển thành công", dto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating transfer");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }

        [HttpPost("{id}/approve")]
        [Authorize(Roles = $"{RoleConstants.Admin},{RoleConstants.HR}")]
        public async Task<IActionResult> ApproveTransfer(int id, [FromBody] string remarks)
        {
            try
            {
                _logger.LogInformation("Approving transfer {Id}", id);
                var transfer = await _repository.GetByIdAsync(id);
                if (transfer == null)
                    return NotFound(new ApiResponse<string>(false, "Không tìm thấy thuyên chuyển", null));

                transfer.ApprovalStatus = "Approved";
                transfer.ApprovalDate = DateTime.UtcNow;
                transfer.ModifiedDate = DateTime.UtcNow;

                await _repository.UpdateAsync(transfer);
                var dto = _mapper.Map<PersonnelTransferDto>(transfer);
                return Ok(new ApiResponse<PersonnelTransferDto>(true, "Duyệt thuyên chuyển thành công", dto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error approving transfer");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = $"{RoleConstants.Admin}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                _logger.LogInformation("Deleting transfer {Id}", id);
                var transfer = await _repository.GetByIdAsync(id);
                if (transfer == null)
                    return NotFound(new ApiResponse<string>(false, "Không tìm thấy thuyên chuyển", null));

                await _repository.DeleteAsync(id);
                return Ok(new ApiResponse<string>(true, "Xóa thuyên chuyển thành công", null));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting transfer");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }
    }
}
