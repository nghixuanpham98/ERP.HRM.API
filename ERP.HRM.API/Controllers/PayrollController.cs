using ERP.HRM.Application.Common;
using ERP.HRM.Application.DTOs.Payroll;
using ERP.HRM.Application.Features.Payroll.Commands;
using ERP.HRM.Application.Features.Payroll.Queries;
using ERP.HRM.Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ERP.HRM.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PayrollController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<PayrollController> _logger;

        public PayrollController(IMediator mediator, ILogger<PayrollController> logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Calculate monthly salary for an employee
        /// </summary>
        [HttpPost("calculate-monthly-salary")]
        [Authorize(Roles = $"{RoleConstants.Admin},{RoleConstants.HR}")]
        public async Task<IActionResult> CalculateMonthlySalary([FromBody] CalculateMonthlySalaryCommand command)
        {
            try
            {
                _logger.LogInformation("CalculateMonthlySalary called for Employee: {EmployeeId}, Period: {PayrollPeriodId}", 
                    command.EmployeeId, command.PayrollPeriodId);

                var result = await _mediator.Send(command);
                
                _logger.LogInformation("Monthly salary calculated successfully for Employee: {EmployeeId}", command.EmployeeId);
                return Ok(new ApiResponse<PayrollRecordDto>(true, "Lương tháng được tính toán thành công", result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating monthly salary");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }

        /// <summary>
        /// Calculate production-based salary for an employee
        /// </summary>
        [HttpPost("calculate-production-salary")]
        [Authorize(Roles = $"{RoleConstants.Admin},{RoleConstants.HR}")]
        public async Task<IActionResult> CalculateProductionSalary([FromBody] CalculateProductionSalaryCommand command)
        {
            try
            {
                _logger.LogInformation("CalculateProductionSalary called for Employee: {EmployeeId}, Period: {PayrollPeriodId}",
                    command.EmployeeId, command.PayrollPeriodId);

                var result = await _mediator.Send(command);
                
                _logger.LogInformation("Production salary calculated successfully for Employee: {EmployeeId}", command.EmployeeId);
                return Ok(new ApiResponse<PayrollRecordDto>(true, "Lương sản lượng được tính toán thành công", result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating production salary");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }

        /// <summary>
        /// Record employee attendance
        /// </summary>
        [HttpPost("record-attendance")]
        [Authorize(Roles = $"{RoleConstants.Admin},{RoleConstants.HR}")]
        public async Task<IActionResult> RecordAttendance([FromBody] RecordAttendanceCommand command)
        {
            try
            {
                _logger.LogInformation("RecordAttendance called for Employee: {EmployeeId}, Date: {AttendanceDate}",
                    command.EmployeeId, command.AttendanceDate);

                var result = await _mediator.Send(command);
                
                _logger.LogInformation("Attendance recorded successfully for Employee: {EmployeeId}", command.EmployeeId);
                return Ok(new ApiResponse<AttendanceDto>(true, "Điểm danh được ghi lại thành công", result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error recording attendance");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }

        /// <summary>
        /// Record employee production output
        /// </summary>
        [HttpPost("record-production-output")]
        [Authorize(Roles = $"{RoleConstants.Admin},{RoleConstants.HR}")]
        public async Task<IActionResult> RecordProductionOutput([FromBody] RecordProductionOutputCommand command)
        {
            try
            {
                _logger.LogInformation("RecordProductionOutput called for Employee: {EmployeeId}, Product: {ProductId}",
                    command.EmployeeId, command.ProductId);

                var result = await _mediator.Send(command);
                
                _logger.LogInformation("Production output recorded successfully for Employee: {EmployeeId}", command.EmployeeId);
                return Ok(new ApiResponse<ProductionOutputDto>(true, "Sản lượng được ghi lại thành công", result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error recording production output");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }

        /// <summary>
        /// Get payroll records by period
        /// </summary>
        [HttpGet("records/by-period/{payrollPeriodId}")]
        [Authorize]
        public async Task<IActionResult> GetPayrollRecordsByPeriod(int payrollPeriodId)
        {
            try
            {
                _logger.LogInformation("GetPayrollRecordsByPeriod called for Period: {PayrollPeriodId}", payrollPeriodId);

                var query = new GetPayrollRecordsByPeriodQuery { PayrollPeriodId = payrollPeriodId };
                var result = await _mediator.Send(query);
                
                return Ok(new ApiResponse<PagedResult<PayrollRecordDto>>(true, "Danh sách bảng lương", result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting payroll records");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }

        /// <summary>
        /// Get salary slip for an employee
        /// </summary>
        [HttpGet("salary-slip/{employeeId}/{payrollPeriodId}")]
        [Authorize]
        public async Task<IActionResult> GetSalarySlip(int employeeId, int payrollPeriodId)
        {
            try
            {
                _logger.LogInformation("GetSalarySlip called for Employee: {EmployeeId}, Period: {PayrollPeriodId}",
                    employeeId, payrollPeriodId);

                var query = new GetSalarySlipQuery { EmployeeId = employeeId, PayrollPeriodId = payrollPeriodId };
                var result = await _mediator.Send(query);
                
                return Ok(new ApiResponse<SalarySlipDto>(true, "Phiếu lương", result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting salary slip");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }

        /// <summary>
        /// Get attendance records by employee and period
        /// </summary>
        [HttpGet("attendance/{employeeId}/{payrollPeriodId}")]
        [Authorize]
        public async Task<IActionResult> GetAttendanceByEmployeeAndPeriod(int employeeId, int payrollPeriodId)
        {
            try
            {
                _logger.LogInformation("GetAttendanceByEmployeeAndPeriod called for Employee: {EmployeeId}, Period: {PayrollPeriodId}",
                    employeeId, payrollPeriodId);

                var query = new GetAttendanceByEmployeeAndPeriodQuery 
                { 
                    EmployeeId = employeeId, 
                    PayrollPeriodId = payrollPeriodId 
                };
                var result = await _mediator.Send(query);
                
                return Ok(new ApiResponse<IEnumerable<AttendanceDto>>(true, "Danh sách điểm danh", result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting attendance records");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }

        /// <summary>
        /// Get production output records by employee and period
        /// </summary>
        [HttpGet("production-output/{employeeId}/{payrollPeriodId}")]
        [Authorize]
        public async Task<IActionResult> GetProductionOutputByEmployeeAndPeriod(int employeeId, int payrollPeriodId)
        {
            try
            {
                _logger.LogInformation("GetProductionOutputByEmployeeAndPeriod called for Employee: {EmployeeId}, Period: {PayrollPeriodId}",
                    employeeId, payrollPeriodId);

                var query = new GetProductionOutputByEmployeeAndPeriodQuery 
                { 
                    EmployeeId = employeeId, 
                    PayrollPeriodId = payrollPeriodId 
                };
                var result = await _mediator.Send(query);
                
                return Ok(new ApiResponse<IEnumerable<ProductionOutputDto>>(true, "Danh sách sản lượng", result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting production output records");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }
    }
}
