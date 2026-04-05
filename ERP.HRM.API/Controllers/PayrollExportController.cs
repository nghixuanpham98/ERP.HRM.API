using ERP.HRM.Application.Common;
using ERP.HRM.Application.DTOs.Payroll;
using ERP.HRM.Application.Features.Payroll.Commands;
using ERP.HRM.Application.Features.Payroll.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ERP.HRM.API.Controllers
{
    /// <summary>
    /// Payroll Export API Controller
    /// Provides endpoints to export payroll data in various formats (Excel/PDF)
    /// Supports exports for bank transfers and tax authorities
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PayrollExportController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<PayrollExportController> _logger;

        public PayrollExportController(
            IMediator mediator,
            ILogger<PayrollExportController> logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        #region General Export Endpoints

        /// <summary>
        /// Export payroll data in Excel or PDF format
        /// </summary>
        /// <param name="request">Export request with period, format, and optional department filter</param>
        /// <returns>File download response (Excel or PDF)</returns>
        /// <response code="200">Returns the exported file</response>
        /// <response code="400">Invalid request parameters</response>
        /// <response code="404">No payroll records found for the specified period</response>
        /// <response code="401">Unauthorized</response>
        [HttpPost("export")]
        [Authorize(Roles = "HR, Manager, Admin")]
        [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ExportPayroll([FromBody] PayrollExportRequestDto request)
        {
            try
            {
                _logger.LogInformation("Exporting payroll for period {PayrollPeriodId} in {ExportFormat} format with purpose: {ExportPurpose}",
                    request.PayrollPeriodId, request.ExportFormat, request.ExportPurpose);

                var command = new ExportPayrollCommand
                {
                    PayrollPeriodId = request.PayrollPeriodId,
                    ExportFormat = request.ExportFormat ?? "Excel",
                    ExportPurpose = request.ExportPurpose ?? "General",
                    DepartmentId = request.DepartmentId,
                    IncludeEmployeeDetails = request.IncludeEmployeeDetails,
                    IncludeSalaryBreakdown = request.IncludeSalaryBreakdown,
                    IncludeDeductionsBreakdown = request.IncludeDeductionsBreakdown
                };

                var result = await _mediator.Send(command);

                // Return file for download
                return File(
                    result.FileContent,
                    result.ContentType,
                    result.FileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error exporting payroll");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }

        #endregion

        #region Bank Transfer Export Endpoints

        /// <summary>
        /// Export payroll data optimized for bank transfer
        /// Includes employee names, account numbers, and transfer amounts
        /// </summary>
        /// <param name="payrollPeriodId">Payroll period ID to export</param>
        /// <param name="departmentId">Optional department filter</param>
        /// <returns>File download response (Excel format)</returns>
        /// <response code="200">Returns the bank transfer export file</response>
        /// <response code="400">Invalid period ID</response>
        /// <response code="404">No payroll records found</response>
        /// <response code="401">Unauthorized</response>
        [HttpPost("export-bank-transfer")]
        [Authorize(Roles = "Finance, Manager, Admin")]
        [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ExportBankTransfer(
            [FromQuery] int payrollPeriodId,
            [FromQuery] int? departmentId = null)
        {
            try
            {
                _logger.LogInformation("Exporting payroll for bank transfer: Period {PayrollPeriodId}, Department {DepartmentId}",
                    payrollPeriodId, departmentId);

                var command = new ExportPayrollForBankCommand
                {
                    PayrollPeriodId = payrollPeriodId,
                    DepartmentId = departmentId
                };
                var result = await _mediator.Send(command);

                return File(
                    result.FileContent,
                    result.ContentType,
                    result.FileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error exporting payroll for bank transfer");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }

        #endregion

        #region Tax Authority Export Endpoints

        /// <summary>
        /// Export payroll data for tax authority (PIT - Thuế TNCN)
        /// Includes tax calculations and compliance information
        /// </summary>
        /// <param name="payrollPeriodId">Payroll period ID to export</param>
        /// <param name="departmentId">Optional department filter</param>
        /// <returns>File download response (Excel format)</returns>
        /// <response code="200">Returns the tax authority export file</response>
        /// <response code="400">Invalid period ID</response>
        /// <response code="404">No payroll records found</response>
        /// <response code="401">Unauthorized</response>
        [HttpPost("export-tax-authority")]
        [Authorize(Roles = "Finance, Accounting, Manager, Admin")]
        [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ExportTaxAuthority(
            [FromQuery] int payrollPeriodId,
            [FromQuery] int? departmentId = null)
        {
            try
            {
                _logger.LogInformation("Exporting payroll for tax authority: Period {PayrollPeriodId}, Department {DepartmentId}",
                    payrollPeriodId, departmentId);

                var command = new ExportPayrollForTaxAuthorityCommand
                {
                    PayrollPeriodId = payrollPeriodId,
                    DepartmentId = departmentId
                };
                var result = await _mediator.Send(command);

                return File(
                    result.FileContent,
                    result.ContentType,
                    result.FileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error exporting payroll for tax authority");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }

        #endregion

        #region Data Query Endpoints

        /// <summary>
        /// Get payroll export lines for a specific period
        /// Useful for previewing data before export
        /// </summary>
        /// <param name="payrollPeriodId">Payroll period ID</param>
        /// <param name="departmentId">Optional department filter</param>
        /// <returns>List of payroll export lines</returns>
        /// <response code="200">Returns list of payroll export lines</response>
        /// <response code="404">No payroll records found</response>
        /// <response code="401">Unauthorized</response>
        [HttpGet("lines/{payrollPeriodId}")]
        [Authorize(Roles = "HR, Finance, Manager, Admin")]
        [ProducesResponseType(typeof(ApiResponse<List<PayrollExportLineDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPayrollExportLines(
            [FromRoute] int payrollPeriodId,
            [FromQuery] int? departmentId = null)
        {
            try
            {
                _logger.LogInformation("Getting payroll export lines for period {PayrollPeriodId}", payrollPeriodId);

                var query = new GetPayrollExportLinesQuery
                {
                    PayrollPeriodId = payrollPeriodId,
                    DepartmentId = departmentId
                };
                var result = await _mediator.Send(query);

                return Ok(new ApiResponse<List<PayrollExportLineDto>>(
                    true,
                    $"Retrieved {result.Count} payroll export lines",
                    result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting payroll export lines");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }

        /// <summary>
        /// Get bank transfer export lines for a specific period
        /// Shows employee data formatted for bank processing
        /// </summary>
        /// <param name="payrollPeriodId">Payroll period ID</param>
        /// <param name="departmentId">Optional department filter</param>
        /// <returns>List of bank transfer export lines</returns>
        /// <response code="200">Returns list of bank transfer lines</response>
        /// <response code="404">No payroll records found</response>
        /// <response code="401">Unauthorized</response>
        [HttpGet("bank-lines/{payrollPeriodId}")]
        [Authorize(Roles = "Finance, Manager, Admin")]
        [ProducesResponseType(typeof(ApiResponse<List<BankTransferExportDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetBankTransferLines(
            [FromRoute] int payrollPeriodId,
            [FromQuery] int? departmentId = null)
        {
            try
            {
                _logger.LogInformation("Getting bank transfer export lines for period {PayrollPeriodId}", payrollPeriodId);

                var query = new GetBankTransferExportLinesQuery
                {
                    PayrollPeriodId = payrollPeriodId,
                    DepartmentId = departmentId
                };
                var result = await _mediator.Send(query);

                return Ok(new ApiResponse<List<BankTransferExportDto>>(
                    true,
                    $"Retrieved {result.Count} bank transfer lines",
                    result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting bank transfer export lines");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }

        /// <summary>
        /// Get tax authority export lines for a specific period
        /// Shows employee data formatted for tax authority reporting
        /// </summary>
        /// <param name="payrollPeriodId">Payroll period ID</param>
        /// <param name="departmentId">Optional department filter</param>
        /// <returns>List of tax authority export lines</returns>
        /// <response code="200">Returns list of tax authority export lines</response>
        /// <response code="404">No payroll records found</response>
        /// <response code="401">Unauthorized</response>
        [HttpGet("tax-lines/{payrollPeriodId}")]
        [Authorize(Roles = "Finance, Accounting, Manager, Admin")]
        [ProducesResponseType(typeof(ApiResponse<List<TaxAuthorityExportDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTaxAuthorityExportLines(
            [FromRoute] int payrollPeriodId,
            [FromQuery] int? departmentId = null)
        {
            try
            {
                _logger.LogInformation("Getting tax authority export lines for period {PayrollPeriodId}", payrollPeriodId);

                var query = new GetTaxAuthorityExportLinesQuery
                {
                    PayrollPeriodId = payrollPeriodId,
                    DepartmentId = departmentId
                };
                var result = await _mediator.Send(query);

                return Ok(new ApiResponse<List<TaxAuthorityExportDto>>(
                    true,
                    $"Retrieved {result.Count} tax authority export lines",
                    result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting tax authority export lines");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }

        /// <summary>
        /// Get payroll export summary (totals and statistics)
        /// </summary>
        /// <param name="payrollPeriodId">Payroll period ID</param>
        /// <param name="departmentId">Optional department filter</param>
        /// <returns>Export summary with totals</returns>
        /// <response code="200">Returns export summary</response>
        /// <response code="404">No payroll records found</response>
        /// <response code="401">Unauthorized</response>
        [HttpGet("summary/{payrollPeriodId}")]
        [Authorize(Roles = "HR, Finance, Manager, Admin")]
        [ProducesResponseType(typeof(ApiResponse<PayrollExportSummaryDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPayrollExportSummary(
            [FromRoute] int payrollPeriodId,
            [FromQuery] int? departmentId = null)
        {
            try
            {
                _logger.LogInformation("Getting payroll export summary for period {PayrollPeriodId}", payrollPeriodId);

                var query = new GetPayrollExportSummaryQuery
                {
                    PayrollPeriodId = payrollPeriodId,
                    DepartmentId = departmentId
                };
                var result = await _mediator.Send(query);

                return Ok(new ApiResponse<PayrollExportSummaryDto>(
                    true,
                    "Payroll export summary retrieved successfully",
                    result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting payroll export summary");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }

        #endregion
    }
}
