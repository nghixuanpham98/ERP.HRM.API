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
    public class SalaryGradesController : ControllerBase
    {
        private readonly ISalaryGradeRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<SalaryGradesController> _logger;

        public SalaryGradesController(
            ISalaryGradeRepository repository,
            IMapper mapper,
            ILogger<SalaryGradesController> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Get all salary grades
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                _logger.LogInformation("Getting all salary grades");
                var grades = await _repository.GetAllAsync();
                var dtos = _mapper.Map<IEnumerable<SalaryGradeDto>>(grades);
                return Ok(new ApiResponse<IEnumerable<SalaryGradeDto>>(true, "Lấy danh sách bậc lương thành công", dtos));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting salary grades");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }

        /// <summary>
        /// Get salary grade by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                _logger.LogInformation("Getting salary grade {Id}", id);
                var grade = await _repository.GetByIdAsync(id);
                if (grade == null)
                    return NotFound(new ApiResponse<string>(false, "Không tìm thấy bậc lương", null));

                var dto = _mapper.Map<SalaryGradeDto>(grade);
                return Ok(new ApiResponse<SalaryGradeDto>(true, "Lấy bậc lương thành công", dto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting salary grade");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }

        /// <summary>
        /// Create a new salary grade
        /// </summary>
        [HttpPost]
        [Authorize(Roles = $"{RoleConstants.Admin},{RoleConstants.HR}")]
        public async Task<IActionResult> Create([FromBody] CreateSalaryGradeDto createDto)
        {
            try
            {
                _logger.LogInformation("Creating new salary grade: {GradeName}", createDto.GradeName);
                
                var grade = new SalaryGrade
                {
                    GradeName = createDto.GradeName,
                    GradeLevel = createDto.GradeLevel,
                    MinSalary = createDto.MinSalary,
                    MidSalary = createDto.MidSalary,
                    MaxSalary = createDto.MaxSalary,
                    Description = createDto.Description,
                    EffectiveDate = createDto.EffectiveDate,
                    IsActive = true
                };

                await _repository.AddAsync(grade);
                var dto = _mapper.Map<SalaryGradeDto>(grade);
                return CreatedAtAction(nameof(GetById), new { id = grade.SalaryGradeId }, dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating salary grade");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }

        /// <summary>
        /// Update a salary grade
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = $"{RoleConstants.Admin},{RoleConstants.HR}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateSalaryGradeDto updateDto)
        {
            try
            {
                _logger.LogInformation("Updating salary grade {Id}", id);
                
                var grade = await _repository.GetByIdAsync(id);
                if (grade == null)
                    return NotFound(new ApiResponse<string>(false, "Không tìm thấy bậc lương", null));

                grade.GradeName = updateDto.GradeName;
                grade.GradeLevel = updateDto.GradeLevel;
                grade.MinSalary = updateDto.MinSalary;
                grade.MidSalary = updateDto.MidSalary;
                grade.MaxSalary = updateDto.MaxSalary;
                grade.IsActive = updateDto.IsActive;
                grade.Description = updateDto.Description;

                await _repository.UpdateAsync(grade);
                var dto = _mapper.Map<SalaryGradeDto>(grade);
                return Ok(new ApiResponse<SalaryGradeDto>(true, "Cập nhật bậc lương thành công", dto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating salary grade");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }

        /// <summary>
        /// Delete a salary grade
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = RoleConstants.Admin)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                _logger.LogInformation("Deleting salary grade {Id}", id);
                
                var grade = await _repository.GetByIdAsync(id);
                if (grade == null)
                    return NotFound(new ApiResponse<string>(false, "Không tìm thấy bậc lương", null));

                await _repository.DeleteAsync(id);
                return Ok(new ApiResponse<string>(true, "Xóa bậc lương thành công", null));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting salary grade");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }
    }
}
