using ERP.HRM.Application.Common;
using ERP.HRM.Application.DTOs.Metrics;
using ERP.HRM.Application.Features.Metrics.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ERP.HRM.API.Controllers
{
    /// <summary>
    /// Metrics controller for department analytics
    /// Provides endpoints for department-level metrics and performance analysis
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MetricsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<MetricsController> _logger;

        public MetricsController(IMediator mediator, ILogger<MetricsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        /// <summary>
        /// Get metrics for a specific department
        /// Shows employee count, salary info, and cost metrics
        /// </summary>
        /// <param name="departmentId">The ID of the department</param>
        /// <returns>Department metrics including employees, salary, and costs</returns>
        [HttpGet("department/{departmentId}")]
        [Authorize(Roles = "HR, Manager, Admin")]
        public async Task<IActionResult> GetDepartmentMetrics(int departmentId)
        {
            try
            {
                _logger.LogInformation("Getting metrics for department {DepartmentId}", departmentId);

                var query = new GetDepartmentMetricsQuery { DepartmentId = departmentId };
                var result = await _mediator.Send(query);

                var response = new ApiResponse<DepartmentMetricsDto>(true, "Department metrics retrieved successfully", result);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting department metrics");
                var response = new ApiResponse<DepartmentMetricsDto>(false, $"Error: {ex.Message}", null);
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        /// <summary>
        /// Compare metrics across all departments
        /// Shows which department has best performance and highest costs
        /// </summary>
        /// <returns>Comparison data for all departments</returns>
        [HttpGet("comparison")]
        [Authorize(Roles = "HR, Manager, Admin")]
        public async Task<IActionResult> GetDepartmentComparison()
        {
            try
            {
                _logger.LogInformation("Getting department comparison");

                var query = new GetDepartmentComparisonQuery();
                var result = await _mediator.Send(query);

                var response = new ApiResponse<DepartmentComparisonDto>(true, "Department comparison retrieved successfully", result);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting department comparison");
                var response = new ApiResponse<DepartmentComparisonDto>(false, $"Error: {ex.Message}", null);
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        /// <summary>
        /// Get employee distribution across departments
        /// Shows percentage and count of employees in each department
        /// </summary>
        /// <returns>Employee distribution by department</returns>
        [HttpGet("employee-distribution")]
        [Authorize(Roles = "HR, Manager, Admin")]
        public async Task<IActionResult> GetEmployeeDistribution()
        {
            try
            {
                _logger.LogInformation("Getting employee distribution");

                var query = new GetEmployeeDistributionQuery();
                var result = await _mediator.Send(query);

                var response = new ApiResponse<List<EmployeeDistributionDto>>(true, "Employee distribution retrieved successfully", result);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting employee distribution");
                var response = new ApiResponse<List<EmployeeDistributionDto>>(false, $"Error: {ex.Message}", null);
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        /// <summary>
        /// Get payroll trends for a department
        /// Shows recent payroll data for the department
        /// </summary>
        /// <param name="departmentId">The ID of the department</param>
        /// <returns>Recent payroll trend data for the department</returns>
        [HttpGet("payroll-trend/{departmentId}")]
        [Authorize(Roles = "HR, Manager, Admin")]
        public async Task<IActionResult> GetDepartmentPayrollTrend(int departmentId)
        {
            try
            {
                _logger.LogInformation("Getting payroll trend for department {DepartmentId}", departmentId);

                var query = new GetDepartmentPayrollTrendQuery { DepartmentId = departmentId };
                var result = await _mediator.Send(query);

                var response = new ApiResponse<DepartmentPayrollTrendDto>(true, "Payroll trend retrieved successfully", result);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting payroll trend");
                var response = new ApiResponse<DepartmentPayrollTrendDto>(false, $"Error: {ex.Message}", null);
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        /// <summary>
        /// Get comprehensive analytics for a department
        /// Includes metrics, payroll trends, leave analysis, and cost breakdown
        /// </summary>
        /// <param name="departmentId">The ID of the department</param>
        /// <returns>Complete department analytics data</returns>
        [HttpGet("analytics/{departmentId}")]
        [Authorize(Roles = "HR, Manager, Admin")]
        public async Task<IActionResult> GetDepartmentAnalytics(int departmentId)
        {
            try
            {
                _logger.LogInformation("Getting comprehensive analytics for department {DepartmentId}", departmentId);

                var query = new GetDepartmentAnalyticsQuery { DepartmentId = departmentId };
                var result = await _mediator.Send(query);

                var response = new ApiResponse<DepartmentAnalyticsDto>(true, "Department analytics retrieved successfully", result);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting department analytics");
                var response = new ApiResponse<DepartmentAnalyticsDto>(false, $"Error: {ex.Message}", null);
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }
    }
}
