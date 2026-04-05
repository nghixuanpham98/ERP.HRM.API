using ERP.HRM.Application.Common;
using ERP.HRM.Application.DTOs.Dashboard;
using ERP.HRM.Application.Features.Dashboard.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ERP.HRM.API.Controllers
{
    /// <summary>
    /// Dashboard controller for payroll analytics and metrics
    /// Provides endpoints for dashboard data and visualizations
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DashboardController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<DashboardController> _logger;

        public DashboardController(IMediator mediator, ILogger<DashboardController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        /// <summary>
        /// Get comprehensive payroll dashboard
        /// Includes summary, department breakdown, leave statistics, and salary distribution
        /// </summary>
        /// <param name="year">Optional: Year for the dashboard (defaults to current year)</param>
        /// <param name="month">Optional: Month for the dashboard (defaults to current month)</param>
        /// <returns>Complete dashboard data with all metrics</returns>
        [HttpGet("payroll-dashboard")]
        [Authorize(Roles = "HR, Manager, Admin")]
        public async Task<IActionResult> GetPayrollDashboard([FromQuery] int? year, [FromQuery] int? month)
        {
            try
            {
                _logger.LogInformation("Getting payroll dashboard for year={Year}, month={Month}", year, month);

                var query = new GetPayrollDashboardQuery { Year = year, Month = month };
                var result = await _mediator.Send(query);

                var response = new ApiResponse<PayrollDashboardDto>(true, "Dashboard retrieved successfully", result);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting payroll dashboard");
                var response = new ApiResponse<PayrollDashboardDto>(false, $"Error: {ex.Message}", null);
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        /// <summary>
        /// Get dashboard summary cards
        /// Shows key metrics: total employees, active employees, average salary, payroll cost, insurance, pending leaves
        /// </summary>
        /// <returns>Summary metrics for quick overview</returns>
        [HttpGet("summary")]
        [Authorize(Roles = "HR, Manager, Admin")]
        public async Task<IActionResult> GetDashboardSummary()
        {
            try
            {
                _logger.LogInformation("Getting dashboard summary");

                var query = new GetDashboardSummaryQuery();
                var result = await _mediator.Send(query);

                var response = new ApiResponse<DashboardSummaryDto>(true, "Summary retrieved successfully", result);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting dashboard summary");
                var response = new ApiResponse<DashboardSummaryDto>(false, $"Error: {ex.Message}", null);
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        /// <summary>
        /// Get recent payroll information
        /// Shows the latest payroll period processed with aggregated data
        /// </summary>
        /// <returns>Recent payroll data with processing details</returns>
        [HttpGet("recent-payroll")]
        [Authorize(Roles = "HR, Manager, Admin")]
        public async Task<IActionResult> GetRecentPayroll()
        {
            try
            {
                _logger.LogInformation("Getting recent payroll information");

                var query = new GetRecentPayrollQuery();
                var result = await _mediator.Send(query);

                var response = new ApiResponse<RecentPayrollDto>(true, "Recent payroll retrieved successfully", result);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting recent payroll");
                var response = new ApiResponse<RecentPayrollDto>(false, $"Error: {ex.Message}", null);
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        /// <summary>
        /// Get department-wise breakdown
        /// Shows employee count, salary metrics for each department
        /// </summary>
        /// <returns>Department breakdown with metrics</returns>
        [HttpGet("department-breakdown")]
        [Authorize(Roles = "HR, Manager, Admin")]
        public async Task<IActionResult> GetDepartmentBreakdown()
        {
            try
            {
                _logger.LogInformation("Getting department breakdown");

                var query = new GetDepartmentBreakdownQuery();
                var result = await _mediator.Send(query);

                var response = new ApiResponse<List<DepartmentSummaryDto>>(true, "Department breakdown retrieved successfully", result);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting department breakdown");
                var response = new ApiResponse<List<DepartmentSummaryDto>>(false, $"Error: {ex.Message}", null);
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        /// <summary>
        /// Get salary distribution analysis
        /// Shows employees grouped by salary ranges for distribution analysis
        /// </summary>
        /// <returns>Salary range distribution with percentages</returns>
        [HttpGet("salary-distribution")]
        [Authorize(Roles = "HR, Manager, Admin")]
        public async Task<IActionResult> GetSalaryDistribution()
        {
            try
            {
                _logger.LogInformation("Getting salary distribution");

                var query = new GetSalaryDistributionQuery();
                var result = await _mediator.Send(query);

                var response = new ApiResponse<List<SalaryRangeDistributionDto>>(true, "Salary distribution retrieved successfully", result);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting salary distribution");
                var response = new ApiResponse<List<SalaryRangeDistributionDto>>(false, $"Error: {ex.Message}", null);
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }
    }
}
