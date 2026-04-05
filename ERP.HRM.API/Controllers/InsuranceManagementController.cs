using ERP.HRM.Application.Common;
using ERP.HRM.Application.DTOs.HR;
using ERP.HRM.Application.Features.Insurance.Commands;
using ERP.HRM.Application.Features.Insurance.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ERP.HRM.API.Controllers
{
    /// <summary>
    /// Insurance Management API Controller
    /// Manages insurance participation and tier operations
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class InsuranceManagementController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<InsuranceManagementController> _logger;

        public InsuranceManagementController(
            IMediator mediator,
            ILogger<InsuranceManagementController> logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        #region Insurance Participation Endpoints

        /// <summary>
        /// Enroll an employee in insurance
        /// </summary>
        [HttpPost("participation/enroll")]
        [Authorize(Roles = "HR, Admin")]
        public async Task<IActionResult> EnrollInsurance([FromBody] CreateInsuranceParticipationDto dto)
        {
            try
            {
                _logger.LogInformation($"Enrolling employee {dto.EmployeeId} in insurance");
                var command = new EnrollEmployeeInInsuranceCommand(dto);
                var result = await _mediator.Send(command);
                return Ok(new ApiResponse<InsuranceParticipationDto>(true, "Employee enrolled in insurance successfully", result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error enrolling employee in insurance");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }

        /// <summary>
        /// Update insurance participation details
        /// </summary>
        [HttpPut("participation/{participationId}")]
        [Authorize(Roles = "HR, Admin")]
        public async Task<IActionResult> UpdateInsuranceParticipation(int participationId, [FromBody] UpdateInsuranceParticipationDto dto)
        {
            try
            {
                _logger.LogInformation($"Updating insurance participation {participationId}");
                var command = new UpdateInsuranceParticipationCommand(participationId, dto);
                var result = await _mediator.Send(command);
                return Ok(new ApiResponse<InsuranceParticipationDto>(true, "Insurance participation updated successfully", result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating insurance participation");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }

        /// <summary>
        /// Terminate insurance participation
        /// </summary>
        [HttpPost("participation/{participationId}/terminate")]
        [Authorize(Roles = "HR, Admin")]
        public async Task<IActionResult> TerminateInsurance(int participationId)
        {
            try
            {
                _logger.LogInformation($"Terminating insurance participation {participationId}");
                var command = new TerminateInsuranceParticipationCommand(participationId);
                var result = await _mediator.Send(command);
                return Ok(new ApiResponse<InsuranceParticipationDto>(true, "Insurance participation terminated successfully", result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error terminating insurance participation");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }

        /// <summary>
        /// Get specific insurance participation
        /// </summary>
        [HttpGet("participation/{participationId}")]
        public async Task<IActionResult> GetInsuranceParticipation(int participationId)
        {
            try
            {
                _logger.LogInformation($"Fetching insurance participation {participationId}");
                var query = new GetInsuranceParticipationQuery(participationId);
                var result = await _mediator.Send(query);
                return Ok(new ApiResponse<InsuranceParticipationDto>(true, "Insurance participation retrieved successfully", result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching insurance participation");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }

        /// <summary>
        /// Get all insurance participations for an employee
        /// </summary>
        [HttpGet("participation/employee/{employeeId}")]
        public async Task<IActionResult> GetEmployeeInsurances(int employeeId)
        {
            try
            {
                _logger.LogInformation($"Fetching insurances for employee {employeeId}");
                var query = new GetEmployeeInsurancesQuery(employeeId);
                var result = await _mediator.Send(query);
                return Ok(new ApiResponse<IEnumerable<InsuranceParticipationDto>>(true, "Employee insurances retrieved successfully", result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching employee insurances");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }

        /// <summary>
        /// Get all active insurance participations
        /// </summary>
        [HttpGet("participation/active")]
        public async Task<IActionResult> GetActiveInsurances()
        {
            try
            {
                _logger.LogInformation("Fetching all active insurances");
                var query = new GetActiveInsurancesQuery();
                var result = await _mediator.Send(query);
                return Ok(new ApiResponse<IEnumerable<InsuranceParticipationDto>>(true, "Active insurances retrieved successfully", result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching active insurances");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }

        #endregion

        #region Insurance Tier Endpoints

        /// <summary>
        /// Create a new insurance tier
        /// </summary>
        [HttpPost("tier")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateInsuranceTier([FromBody] CreateInsuranceTierDto dto)
        {
            try
            {
                _logger.LogInformation($"Creating insurance tier {dto.TierName}");
                var command = new CreateInsuranceTierCommand(dto);
                var result = await _mediator.Send(command);
                return CreatedAtAction(nameof(GetInsuranceTier), new { tierId = result.InsuranceTierId }, 
                    new ApiResponse<InsuranceTierDto>(true, "Insurance tier created successfully", result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating insurance tier");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }

        /// <summary>
        /// Update insurance tier
        /// </summary>
        [HttpPut("tier/{tierId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateInsuranceTier(int tierId, [FromBody] UpdateInsuranceTierDto dto)
        {
            try
            {
                _logger.LogInformation($"Updating insurance tier {tierId}");
                var command = new UpdateInsuranceTierCommand(tierId, dto);
                var result = await _mediator.Send(command);
                return Ok(new ApiResponse<InsuranceTierDto>(true, "Insurance tier updated successfully", result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating insurance tier");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }

        /// <summary>
        /// Delete insurance tier
        /// </summary>
        [HttpDelete("tier/{tierId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteInsuranceTier(int tierId)
        {
            try
            {
                _logger.LogInformation($"Deleting insurance tier {tierId}");
                var command = new DeleteInsuranceTierCommand(tierId);
                var result = await _mediator.Send(command);
                return Ok(new ApiResponse<bool>(true, "Insurance tier deleted successfully", result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting insurance tier");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }

        /// <summary>
        /// Get specific insurance tier
        /// </summary>
        [HttpGet("tier/{tierId}")]
        public async Task<IActionResult> GetInsuranceTier(int tierId)
        {
            try
            {
                _logger.LogInformation($"Fetching insurance tier {tierId}");
                var query = new GetInsuranceTierQuery(tierId);
                var result = await _mediator.Send(query);
                return Ok(new ApiResponse<InsuranceTierDto>(true, "Insurance tier retrieved successfully", result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching insurance tier");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }

        /// <summary>
        /// Get all active insurance tiers
        /// </summary>
        [HttpGet("tier/active")]
        public async Task<IActionResult> GetAllActiveInsuranceTiers()
        {
            try
            {
                _logger.LogInformation("Fetching all active insurance tiers");
                var query = new GetAllActiveInsuranceTiersQuery();
                var result = await _mediator.Send(query);
                return Ok(new ApiResponse<IEnumerable<InsuranceTierDto>>(true, "Active insurance tiers retrieved successfully", result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching active insurance tiers");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }

        /// <summary>
        /// Get insurance tiers by type
        /// </summary>
        [HttpGet("tier/type/{insuranceType}")]
        public async Task<IActionResult> GetInsuranceTiersByType(string insuranceType)
        {
            try
            {
                _logger.LogInformation($"Fetching insurance tiers for type {insuranceType}");
                var query = new GetInsuranceTiersByTypeQuery(insuranceType);
                var result = await _mediator.Send(query);
                return Ok(new ApiResponse<IEnumerable<InsuranceTierDto>>(true, "Insurance tiers retrieved successfully", result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching insurance tiers by type");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }

        #endregion

        #region Calculation Endpoints

        /// <summary>
        /// Calculate insurance contribution for an employee
        /// </summary>
        [HttpPost("calculate/contribution")]
        public async Task<IActionResult> CalculateInsuranceContribution(
            [FromQuery] int employeeId,
            [FromQuery] string insuranceType,
            [FromQuery] decimal salary)
        {
            try
            {
                _logger.LogInformation($"Calculating insurance contribution for employee {employeeId}");
                var query = new CalculateInsuranceContributionQuery(employeeId, insuranceType, salary);
                var result = await _mediator.Send(query);
                return Ok(new ApiResponse<InsuranceCalculationResultDto>(true, "Insurance contribution calculated successfully", result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating insurance contribution");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }

        /// <summary>
        /// Calculate total insurance contribution for an employee
        /// </summary>
        [HttpPost("calculate/total-contribution")]
        public async Task<IActionResult> CalculateTotalInsuranceContribution(
            [FromQuery] int employeeId,
            [FromQuery] decimal totalSalary)
        {
            try
            {
                _logger.LogInformation($"Calculating total insurance contribution for employee {employeeId}");
                var query = new CalculateTotalInsuranceContributionQuery(employeeId, totalSalary);
                var result = await _mediator.Send(query);
                return Ok(new ApiResponse<decimal>(true, "Total insurance contribution calculated successfully", result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating total insurance contribution");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }

        #endregion
    }
}
