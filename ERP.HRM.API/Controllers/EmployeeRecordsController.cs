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
    public class EmployeeRecordsController : ControllerBase
    {
        private readonly IEmployeeRecordRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<EmployeeRecordsController> _logger;

        public EmployeeRecordsController(
            IEmployeeRecordRepository repository,
            IMapper mapper,
            ILogger<EmployeeRecordsController> logger)
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
                _logger.LogInformation("Getting all employee records");
                var records = await _repository.GetAllAsync();
                var dtos = _mapper.Map<IEnumerable<EmployeeRecordDto>>(records);
                return Ok(new ApiResponse<IEnumerable<EmployeeRecordDto>>(true, "Lấy danh sách hồ sơ thành công", dtos));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting employee records");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }

        [HttpGet("employee/{employeeId}")]
        public async Task<IActionResult> GetByEmployeeId(int employeeId)
        {
            try
            {
                _logger.LogInformation("Getting records for employee {EmployeeId}", employeeId);
                var records = await _repository.GetByEmployeeIdAsync(employeeId);
                var dtos = _mapper.Map<IEnumerable<EmployeeRecordDto>>(records);
                return Ok(new ApiResponse<IEnumerable<EmployeeRecordDto>>(true, "Lấy danh sách hồ sơ thành công", dtos));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting records");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }

        [HttpGet("status/{status}")]
        public async Task<IActionResult> GetByStatus(string status)
        {
            try
            {
                _logger.LogInformation("Getting records with status {Status}", status);
                var records = await _repository.GetByStatusAsync(status);
                var dtos = _mapper.Map<IEnumerable<EmployeeRecordDto>>(records);
                return Ok(new ApiResponse<IEnumerable<EmployeeRecordDto>>(true, "Lấy danh sách hồ sơ thành công", dtos));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting records");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetActiveRecords()
        {
            try
            {
                _logger.LogInformation("Getting active employee records");
                var records = await _repository.GetActiveRecordsAsync();
                var dtos = _mapper.Map<IEnumerable<EmployeeRecordDto>>(records);
                return Ok(new ApiResponse<IEnumerable<EmployeeRecordDto>>(true, "Lấy danh sách hồ sơ hoạt động thành công", dtos));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting active records");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                _logger.LogInformation("Getting record {Id}", id);
                var record = await _repository.GetByIdAsync(id);
                if (record == null)
                    return NotFound(new ApiResponse<string>(false, "Không tìm thấy hồ sơ", null));

                var dto = _mapper.Map<EmployeeRecordDto>(record);
                return Ok(new ApiResponse<EmployeeRecordDto>(true, "Lấy hồ sơ thành công", dto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting record");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }

        [HttpPost]
        [Authorize(Roles = $"{RoleConstants.Admin},{RoleConstants.HR}")]
        public async Task<IActionResult> Create([FromBody] CreateEmployeeRecordDto createDto)
        {
            try
            {
                _logger.LogInformation("Creating new record for employee {EmployeeId}", createDto.EmployeeId);

                var record = new EmployeeRecord
                {
                    EmployeeId = createDto.EmployeeId,
                    RecordNumber = createDto.RecordNumber,
                    FilePath = createDto.FilePath,
                    DocumentType = createDto.DocumentType,
                    IssueDate = createDto.IssueDate,
                    ExpiryDate = createDto.ExpiryDate,
                    IssuingOrganization = createDto.IssuingOrganization,
                    Status = "Active",
                    CreatedDate = DateTime.UtcNow
                };

                await _repository.AddAsync(record);
                var dto = _mapper.Map<EmployeeRecordDto>(record);
                return CreatedAtAction(nameof(GetById), new { id = record.EmployeeRecordId }, dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating record");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = $"{RoleConstants.Admin},{RoleConstants.HR}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateEmployeeRecordDto updateDto)
        {
            try
            {
                _logger.LogInformation("Updating record {Id}", id);
                var record = await _repository.GetByIdAsync(id);
                if (record == null)
                    return NotFound(new ApiResponse<string>(false, "Không tìm thấy hồ sơ", null));

                record.RecordNumber = updateDto.RecordNumber;
                record.FilePath = updateDto.FilePath;
                record.DocumentType = updateDto.DocumentType;
                record.IssueDate = updateDto.IssueDate;
                record.ExpiryDate = updateDto.ExpiryDate;
                record.IssuingOrganization = updateDto.IssuingOrganization;
                record.Status = updateDto.Status;
                record.ModifiedDate = DateTime.UtcNow;

                await _repository.UpdateAsync(record);
                var dto = _mapper.Map<EmployeeRecordDto>(record);
                return Ok(new ApiResponse<EmployeeRecordDto>(true, "Cập nhật hồ sơ thành công", dto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating record");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = $"{RoleConstants.Admin}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                _logger.LogInformation("Deleting record {Id}", id);
                var record = await _repository.GetByIdAsync(id);
                if (record == null)
                    return NotFound(new ApiResponse<string>(false, "Không tìm thấy hồ sơ", null));

                await _repository.DeleteAsync(id);
                return Ok(new ApiResponse<string>(true, "Xóa hồ sơ thành công", null));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting record");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }
    }
}
