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
    public class InsuranceParticipationsController : ControllerBase
    {
        private readonly IInsuranceParticipationRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<InsuranceParticipationsController> _logger;

        public InsuranceParticipationsController(
            IInsuranceParticipationRepository repository,
            IMapper mapper,
            ILogger<InsuranceParticipationsController> logger)
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
                _logger.LogInformation("Getting all insurance participations");
                var participations = await _repository.GetAllAsync();
                var dtos = _mapper.Map<IEnumerable<InsuranceParticipationDto>>(participations);
                return Ok(new ApiResponse<IEnumerable<InsuranceParticipationDto>>(true, "Lấy danh sách bảo hiểm thành công", dtos));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting insurance participations");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }

        [HttpGet("employee/{employeeId}")]
        public async Task<IActionResult> GetByEmployeeId(int employeeId)
        {
            try
            {
                _logger.LogInformation("Getting insurances for employee {EmployeeId}", employeeId);
                var participations = await _repository.GetByEmployeeIdAsync(employeeId);
                var dtos = _mapper.Map<IEnumerable<InsuranceParticipationDto>>(participations);
                return Ok(new ApiResponse<IEnumerable<InsuranceParticipationDto>>(true, "Lấy danh sách bảo hiểm thành công", dtos));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting insurances");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetActiveInsurances()
        {
            try
            {
                _logger.LogInformation("Getting active insurance participations");
                var participations = await _repository.GetActiveInsurancesAsync();
                var dtos = _mapper.Map<IEnumerable<InsuranceParticipationDto>>(participations);
                return Ok(new ApiResponse<IEnumerable<InsuranceParticipationDto>>(true, "Lấy danh sách bảo hiểm hoạt động thành công", dtos));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting active insurances");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                _logger.LogInformation("Getting insurance participation {Id}", id);
                var participation = await _repository.GetByIdAsync(id);
                if (participation == null)
                    return NotFound(new ApiResponse<string>(false, "Không tìm thấy bảo hiểm", null));

                var dto = _mapper.Map<InsuranceParticipationDto>(participation);
                return Ok(new ApiResponse<InsuranceParticipationDto>(true, "Lấy bảo hiểm thành công", dto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting insurance participation");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }

        [HttpPost]
        [Authorize(Roles = $"{RoleConstants.Admin},{RoleConstants.HR}")]
        public async Task<IActionResult> Create([FromBody] CreateInsuranceParticipationDto createDto)
        {
            try
            {
                _logger.LogInformation("Creating new insurance for employee {EmployeeId}", createDto.EmployeeId);

                var participation = new InsuranceParticipation
                {
                    EmployeeId = createDto.EmployeeId,
                    InsuranceType = createDto.InsuranceType,
                    InsuranceNumber = createDto.InsuranceNumber,
                    StartDate = createDto.StartDate,
                    EndDate = createDto.EndDate,
                    Status = "Active",
                    ContributionBaseSalary = createDto.ContributionBaseSalary,
                    EmployeeContributionRate = createDto.EmployeeContributionRate,
                    EmployerContributionRate = createDto.EmployerContributionRate,
                    CreatedDate = DateTime.UtcNow
                };

                await _repository.AddAsync(participation);
                var dto = _mapper.Map<InsuranceParticipationDto>(participation);
                return CreatedAtAction(nameof(GetById), new { id = participation.InsuranceParticipationId }, dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating insurance");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = $"{RoleConstants.Admin},{RoleConstants.HR}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateInsuranceParticipationDto updateDto)
        {
            try
            {
                _logger.LogInformation("Updating insurance {Id}", id);
                var participation = await _repository.GetByIdAsync(id);
                if (participation == null)
                    return NotFound(new ApiResponse<string>(false, "Không tìm thấy bảo hiểm", null));

                participation.InsuranceNumber = updateDto.InsuranceNumber;
                participation.StartDate = updateDto.StartDate;
                participation.EndDate = updateDto.EndDate;
                participation.Status = updateDto.Status;
                participation.ContributionBaseSalary = updateDto.ContributionBaseSalary;
                participation.EmployeeContributionRate = updateDto.EmployeeContributionRate;
                participation.EmployerContributionRate = updateDto.EmployerContributionRate;
                participation.ModifiedDate = DateTime.UtcNow;

                await _repository.UpdateAsync(participation);
                var dto = _mapper.Map<InsuranceParticipationDto>(participation);
                return Ok(new ApiResponse<InsuranceParticipationDto>(true, "Cập nhật bảo hiểm thành công", dto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating insurance");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = $"{RoleConstants.Admin}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                _logger.LogInformation("Deleting insurance {Id}", id);
                var participation = await _repository.GetByIdAsync(id);
                if (participation == null)
                    return NotFound(new ApiResponse<string>(false, "Không tìm thấy bảo hiểm", null));

                await _repository.DeleteAsync(id);
                return Ok(new ApiResponse<string>(true, "Xóa bảo hiểm thành công", null));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting insurance");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }
    }
}
