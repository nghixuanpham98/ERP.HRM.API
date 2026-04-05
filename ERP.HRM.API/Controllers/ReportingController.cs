using ERP.HRM.Application.Common;
using ERP.HRM.Application.DTOs.Reporting;
using ERP.HRM.Application.Features.Reporting.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ERP.HRM.API.Controllers
{
    /// <summary>
    /// Reporting and Analytics API Controller
    /// Provides endpoints for business intelligence and reporting
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ReportingController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ReportingController> _logger;

        public ReportingController(
            IMediator mediator,
            ILogger<ReportingController> logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Get monthly payroll summary for a specific month
        /// </summary>
        /// <param name="year">Report year</param>
        /// <param name="month">Report month (1-12)</param>
        /// <returns>Monthly payroll summary with totals and averages</returns>
        /// <response code="200">Returns the monthly payroll summary</response>
        /// <response code="400">Invalid year or month parameters</response>
        /// <response code="401">Unauthorized</response>
        [HttpGet("payroll-summary/{year}/{month}")]
        [Authorize(Roles = "HR, Manager, Admin")]
        [ProducesResponseType(typeof(ApiResponse<MonthlyPayrollSummaryDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public async Task<IActionResult> GetMonthlyPayrollSummary(
            [FromRoute] int year,
            [FromRoute] int month)
        {
            try
            {
                _logger.LogInformation("Getting monthly payroll summary for {Year}-{Month}", year, month);

                var query = new GetMonthlyPayrollSummaryQuery
                {
                    Year = year,
                    Month = month
                };

                var result = await _mediator.Send(query);

                var response = new ApiResponse<MonthlyPayrollSummaryDto>(true, $"Payroll summary for {month}/{year} retrieved successfully", result);
                return Ok(response);
            }
            catch (FluentValidation.ValidationException ex)
            {
                _logger.LogWarning(ex, "Validation failed");
                var response = new ApiResponse<string>(false, "Validation failed", string.Join("; ", ex.Errors.Select(e => e.ErrorMessage)));
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving payroll summary");
                var response = new ApiResponse<string>(false, "An error occurred", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        /// <summary>
        /// Get tax summary report for a specific month
        /// </summary>
        /// <param name="year">Report year</param>
        /// <param name="month">Report month (1-12)</param>
        /// <returns>Tax summary with total tax liability and metrics</returns>
        /// <response code="200">Returns the tax summary report</response>
        /// <response code="400">Invalid parameters</response>
        /// <response code="401">Unauthorized</response>
        [HttpGet("tax-summary/{year}/{month}")]
        [Authorize(Roles = "HR, Manager, Admin")]
        [ProducesResponseType(typeof(ApiResponse<TaxSummaryReportDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public async Task<IActionResult> GetTaxSummary(
            [FromRoute] int year,
            [FromRoute] int month)
        {
            try
            {
                _logger.LogInformation("Getting tax summary for {Year}-{Month}", year, month);

                var query = new GetTaxSummaryReportQuery
                {
                    Year = year,
                    Month = month
                };

                var result = await _mediator.Send(query);

                var response = new ApiResponse<TaxSummaryReportDto>(true, $"Tax summary for {month}/{year} retrieved successfully", result);
                return Ok(response);
            }
            catch (FluentValidation.ValidationException ex)
            {
                _logger.LogWarning(ex, "Validation failed");
                var response = new ApiResponse<string>(false, "Validation failed", string.Join("; ", ex.Errors.Select(e => e.ErrorMessage)));
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving tax summary");
                var response = new ApiResponse<string>(false, "An error occurred", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        /// <summary>
        /// Get insurance summary report for a specific month
        /// </summary>
        /// <param name="year">Report year</param>
        /// <param name="month">Report month (1-12)</param>
        /// <returns>Insurance summary with deduction totals</returns>
        /// <response code="200">Returns the insurance summary</response>
        /// <response code="400">Invalid parameters</response>
        /// <response code="401">Unauthorized</response>
        [HttpGet("insurance-summary/{year}/{month}")]
        [Authorize(Roles = "HR, Manager, Admin")]
        [ProducesResponseType(typeof(ApiResponse<InsuranceSummaryReportDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public async Task<IActionResult> GetInsuranceSummary(
            [FromRoute] int year,
            [FromRoute] int month)
        {
            try
            {
                _logger.LogInformation("Getting insurance summary for {Year}-{Month}", year, month);

                var query = new GetInsuranceSummaryReportQuery
                {
                    Year = year,
                    Month = month
                };

                var result = await _mediator.Send(query);

                var response = new ApiResponse<InsuranceSummaryReportDto>(true, $"Insurance summary for {month}/{year} retrieved successfully", result);
                return Ok(response);
            }
            catch (FluentValidation.ValidationException ex)
            {
                _logger.LogWarning(ex, "Validation failed");
                var response = new ApiResponse<string>(false, "Validation failed", string.Join("; ", ex.Errors.Select(e => e.ErrorMessage)));
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving insurance summary");
                var response = new ApiResponse<string>(false, "An error occurred", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        /// <summary>
        /// Get leave summary report for a specific month
        /// </summary>
        /// <param name="year">Report year</param>
        /// <param name="month">Report month (1-12)</param>
        /// <returns>Leave summary with approval statistics</returns>
        /// <response code="200">Returns the leave summary</response>
        /// <response code="400">Invalid parameters</response>
        /// <response code="401">Unauthorized</response>
        [HttpGet("leave-summary/{year}/{month}")]
        [Authorize(Roles = "HR, Manager, Admin")]
        [ProducesResponseType(typeof(ApiResponse<LeaveSummaryReportDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public async Task<IActionResult> GetLeaveSummary(
            [FromRoute] int year,
            [FromRoute] int month)
        {
            try
            {
                _logger.LogInformation("Getting leave summary for {Year}-{Month}", year, month);

                var query = new GetLeaveSummaryReportQuery
                {
                    Year = year,
                    Month = month
                };

                var result = await _mediator.Send(query);

                var response = new ApiResponse<LeaveSummaryReportDto>(true, $"Leave summary for {month}/{year} retrieved successfully", result);
                return Ok(response);
            }
            catch (FluentValidation.ValidationException ex)
            {
                _logger.LogWarning(ex, "Validation failed");
                var response = new ApiResponse<string>(false, "Validation failed", string.Join("; ", ex.Errors.Select(e => e.ErrorMessage)));
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving leave summary");
                var response = new ApiResponse<string>(false, "An error occurred", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        /// <summary>
        /// Get comprehensive HR metrics for a year
        /// </summary>
        /// <param name="year">Report year</param>
        /// <returns>HR metrics with employee and cost analysis</returns>
        /// <response code="200">Returns the HR metrics</response>
        /// <response code="400">Invalid year</response>
        /// <response code="401">Unauthorized</response>
        [HttpGet("hr-metrics/{year}")]
        [Authorize(Roles = "HR, Manager, Admin")]
        [ProducesResponseType(typeof(ApiResponse<HRMetricsDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public async Task<IActionResult> GetHRMetrics(
            [FromRoute] int year)
        {
            try
            {
                _logger.LogInformation("Getting HR metrics for {Year}", year);

                var query = new GetHRMetricsQuery
                {
                    Year = year
                };

                var result = await _mediator.Send(query);

                var response = new ApiResponse<HRMetricsDto>(true, $"HR metrics for {year} retrieved successfully", result);
                return Ok(response);
            }
            catch (FluentValidation.ValidationException ex)
            {
                _logger.LogWarning(ex, "Validation failed");
                var response = new ApiResponse<string>(false, "Validation failed", string.Join("; ", ex.Errors.Select(e => e.ErrorMessage)));
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving HR metrics");
                var response = new ApiResponse<string>(false, "An error occurred", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }
    }
}
