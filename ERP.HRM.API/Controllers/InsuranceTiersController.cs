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
    public class InsuranceTiersController : ControllerBase
    {
        private readonly IInsuranceTierRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<InsuranceTiersController> _logger;

        public InsuranceTiersController(
            IInsuranceTierRepository repository,
            IMapper mapper,
            ILogger<InsuranceTiersController> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Get all insurance tiers
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                _logger.LogInformation("Getting all insurance tiers");
                var tiers = await _repository.GetAllAsync();
                var dtos = _mapper.Map<IEnumerable<InsuranceTierDto>>(tiers);
                return Ok(new ApiResponse<IEnumerable<InsuranceTierDto>>(true, "Lấy danh sách bậc bảo hiểm thành công", dtos));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting insurance tiers");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }

        /// <summary>
        /// Get active insurance tiers
        /// </summary>
        [HttpGet("active")]
        public async Task<IActionResult> GetActiveTiers()
        {
            try
            {
                _logger.LogInformation("Getting active insurance tiers");
                var tiers = await _repository.GetActiveTiersAsync(DateTime.Now);
                var dtos = _mapper.Map<IEnumerable<InsuranceTierDto>>(tiers);
                return Ok(new ApiResponse<IEnumerable<InsuranceTierDto>>(true, "Lấy danh sách bậc bảo hiểm hoạt động thành công", dtos));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting active tiers");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }

        /// <summary>
        /// Get insurance tiers by type
        /// </summary>
        [HttpGet("type/{insuranceType}")]
        public async Task<IActionResult> GetByType(string insuranceType)
        {
            try
            {
                _logger.LogInformation("Getting insurance tiers for type {InsuranceType}", insuranceType);
                var tiers = await _repository.GetByTypeAsync(insuranceType);
                var dtos = _mapper.Map<IEnumerable<InsuranceTierDto>>(tiers);
                return Ok(new ApiResponse<IEnumerable<InsuranceTierDto>>(true, "Lấy danh sách bậc bảo hiểm thành công", dtos));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting tiers by type");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }

        /// <summary>
        /// Get insurance tier by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                _logger.LogInformation("Getting insurance tier {Id}", id);
                var tier = await _repository.GetByIdAsync(id);
                if (tier == null)
                    return NotFound(new ApiResponse<string>(false, "Không tìm thấy bậc bảo hiểm", null));

                var dto = _mapper.Map<InsuranceTierDto>(tier);
                return Ok(new ApiResponse<InsuranceTierDto>(true, "Lấy bậc bảo hiểm thành công", dto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting insurance tier");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }

        /// <summary>
        /// Create a new insurance tier
        /// </summary>
        [HttpPost]
        [Authorize(Roles = RoleConstants.Admin)]
        public async Task<IActionResult> Create([FromBody] CreateInsuranceTierDto createDto)
        {
            try
            {
                _logger.LogInformation("Creating new insurance tier: {TierName}", createDto.TierName);
                
                var tier = new InsuranceTier
                {
                    TierName = createDto.TierName,
                    InsuranceType = createDto.InsuranceType,
                    MinSalary = createDto.MinSalary,
                    MaxSalary = createDto.MaxSalary,
                    EmployeeRate = createDto.EmployeeRate,
                    EmployerRate = createDto.EmployerRate,
                    EffectiveDate = createDto.EffectiveDate,
                    IsActive = true
                };

                await _repository.AddAsync(tier);
                var dto = _mapper.Map<InsuranceTierDto>(tier);
                return CreatedAtAction(nameof(GetById), new { id = tier.InsuranceTierId }, dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating insurance tier");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }

        /// <summary>
        /// Update an insurance tier
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = RoleConstants.Admin)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateInsuranceTierDto updateDto)
        {
            try
            {
                _logger.LogInformation("Updating insurance tier {Id}", id);
                
                var tier = await _repository.GetByIdAsync(id);
                if (tier == null)
                    return NotFound(new ApiResponse<string>(false, "Không tìm thấy bậc bảo hiểm", null));

                tier.TierName = updateDto.TierName;
                tier.InsuranceType = updateDto.InsuranceType;
                tier.MinSalary = updateDto.MinSalary;
                tier.MaxSalary = updateDto.MaxSalary;
                tier.EmployeeRate = updateDto.EmployeeRate;
                tier.EmployerRate = updateDto.EmployerRate;
                tier.EndDate = updateDto.EndDate;
                tier.IsActive = updateDto.IsActive;

                await _repository.UpdateAsync(tier);
                var dto = _mapper.Map<InsuranceTierDto>(tier);
                return Ok(new ApiResponse<InsuranceTierDto>(true, "Cập nhật bậc bảo hiểm thành công", dto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating insurance tier");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }

        /// <summary>
        /// Delete an insurance tier
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = RoleConstants.Admin)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                _logger.LogInformation("Deleting insurance tier {Id}", id);
                
                var tier = await _repository.GetByIdAsync(id);
                if (tier == null)
                    return NotFound(new ApiResponse<string>(false, "Không tìm thấy bậc bảo hiểm", null));

                await _repository.DeleteAsync(id);
                return Ok(new ApiResponse<string>(true, "Xóa bậc bảo hiểm thành công", null));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting insurance tier");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }
    }
}
