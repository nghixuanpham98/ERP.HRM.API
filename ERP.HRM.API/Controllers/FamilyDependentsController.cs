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
    public class FamilyDependentsController : ControllerBase
    {
        private readonly IFamilyDependentRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<FamilyDependentsController> _logger;

        public FamilyDependentsController(
            IFamilyDependentRepository repository,
            IMapper mapper,
            ILogger<FamilyDependentsController> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Get all family dependents
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                _logger.LogInformation("Getting all family dependents");
                var dependents = await _repository.GetAllAsync();
                var dtos = _mapper.Map<IEnumerable<FamilyDependentDto>>(dependents);
                return Ok(new ApiResponse<IEnumerable<FamilyDependentDto>>(true, "Lấy danh sách gia nhân thành công", dtos));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting family dependents");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }

        /// <summary>
        /// Get dependents by employee
        /// </summary>
        [HttpGet("employee/{employeeId}")]
        public async Task<IActionResult> GetByEmployeeId(int employeeId)
        {
            try
            {
                _logger.LogInformation("Getting dependents for employee {EmployeeId}", employeeId);
                var dependents = await _repository.GetByEmployeeIdAsync(employeeId);
                var dtos = _mapper.Map<IEnumerable<FamilyDependentDto>>(dependents);
                return Ok(new ApiResponse<IEnumerable<FamilyDependentDto>>(true, "Lấy danh sách gia nhân thành công", dtos));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting dependents");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }

        /// <summary>
        /// Get qualified dependents by employee (for tax calculation)
        /// </summary>
        [HttpGet("employee/{employeeId}/qualified")]
        public async Task<IActionResult> GetQualifiedDependents(int employeeId)
        {
            try
            {
                _logger.LogInformation("Getting qualified dependents for employee {EmployeeId}", employeeId);
                var dependents = await _repository.GetQualifiedDependentsByEmployeeIdAsync(employeeId, DateTime.Now);
                var dtos = _mapper.Map<IEnumerable<FamilyDependentDto>>(dependents);
                return Ok(new ApiResponse<IEnumerable<FamilyDependentDto>>(true, "Lấy danh sách gia nhân hợp lệ thành công", dtos));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting qualified dependents");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }

        /// <summary>
        /// Get dependent by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                _logger.LogInformation("Getting dependent {Id}", id);
                var dependent = await _repository.GetByIdAsync(id);
                if (dependent == null)
                    return NotFound(new ApiResponse<string>(false, "Không tìm thấy gia nhân", null));

                var dto = _mapper.Map<FamilyDependentDto>(dependent);
                return Ok(new ApiResponse<FamilyDependentDto>(true, "Lấy gia nhân thành công", dto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting dependent");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }

        /// <summary>
        /// Create a new family dependent
        /// </summary>
        [HttpPost]
        [Authorize(Roles = $"{RoleConstants.Admin},{RoleConstants.HR}")]
        public async Task<IActionResult> Create([FromBody] CreateFamilyDependentDto createDto)
        {
            try
            {
                _logger.LogInformation("Creating new dependent for employee {EmployeeId}", createDto.EmployeeId);
                
                var dependent = new FamilyDependent
                {
                    EmployeeId = createDto.EmployeeId,
                    FullName = createDto.FullName,
                    Relationship = createDto.Relationship,
                    DateOfBirth = createDto.DateOfBirth,
                    IsQualified = true,
                    QualificationStartDate = createDto.QualificationStartDate,
                    QualificationEndDate = createDto.QualificationEndDate,
                    NationalId = createDto.NationalId,
                    Notes = createDto.Notes
                };

                await _repository.AddAsync(dependent);
                var dto = _mapper.Map<FamilyDependentDto>(dependent);
                return CreatedAtAction(nameof(GetById), new { id = dependent.FamilyDependentId }, dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating dependent");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }

        /// <summary>
        /// Update a family dependent
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = $"{RoleConstants.Admin},{RoleConstants.HR}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateFamilyDependentDto updateDto)
        {
            try
            {
                _logger.LogInformation("Updating dependent {Id}", id);
                
                var dependent = await _repository.GetByIdAsync(id);
                if (dependent == null)
                    return NotFound(new ApiResponse<string>(false, "Không tìm thấy gia nhân", null));

                dependent.FullName = updateDto.FullName;
                dependent.Relationship = updateDto.Relationship;
                dependent.DateOfBirth = updateDto.DateOfBirth;
                dependent.IsQualified = updateDto.IsQualified;
                dependent.QualificationStartDate = updateDto.QualificationStartDate;
                dependent.QualificationEndDate = updateDto.QualificationEndDate;
                dependent.NationalId = updateDto.NationalId;
                dependent.Notes = updateDto.Notes;

                await _repository.UpdateAsync(dependent);
                var dto = _mapper.Map<FamilyDependentDto>(dependent);
                return Ok(new ApiResponse<FamilyDependentDto>(true, "Cập nhật gia nhân thành công", dto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating dependent");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }

        /// <summary>
        /// Delete a family dependent
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = RoleConstants.Admin)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                _logger.LogInformation("Deleting dependent {Id}", id);
                
                var dependent = await _repository.GetByIdAsync(id);
                if (dependent == null)
                    return NotFound(new ApiResponse<string>(false, "Không tìm thấy gia nhân", null));

                await _repository.DeleteAsync(id);
                return Ok(new ApiResponse<string>(true, "Xóa gia nhân thành công", null));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting dependent");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }
    }
}
