using ERP.HRM.Application.Common;
using ERP.HRM.Application.DTOs.Employee;
using ERP.HRM.Application.Interfaces;
using ERP.HRM.Domain.Constants;
using ERP.HRM.Domain.Interfaces.Repositories;
using ERP.HRM.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ERP.HRM.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly ILogger<EmployeeController> _logger;

        public EmployeeController(IEmployeeService employeeService, ILogger<EmployeeController> logger)
        {
            _employeeService = employeeService;
            _logger = logger;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllEmployees(int pageNumber = 1, int pageSize = 10)
        {
            _logger.LogInformation("GetAllEmployees called with PageNumber: {PageNumber}, PageSize: {PageSize}", pageNumber, pageSize);
            var employees = await _employeeService.GetAllEmployeesAsync(pageNumber, pageSize);
            return Ok(new ApiResponse<PagedResult<EmployeeDto>>(true, "Danh sách nhân viên", employees));
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetEmployeeById(int id)
        {
            _logger.LogInformation("GetEmployeeById called with Id: {EmployeeId}", id);
            var employee = await _employeeService.GetEmployeeByIdAsync(id);
            return Ok(new ApiResponse<EmployeeDto?>(true, "Chi tiết nhân viên", employee));
        }

        [HttpPost]
        [Authorize(Roles = $"{RoleConstants.Admin},{RoleConstants.HR}")]
        public async Task<IActionResult> CreateEmployee([FromBody] CreateEmployeeDto dto)
        {
            _logger.LogInformation("CreateEmployee called with FullName: {FullName}", dto.FullName);
            var employee = await _employeeService.AddEmployeeAsync(dto);
            _logger.LogInformation("Employee '{FullName}' created successfully", dto.FullName);
            return Ok(new ApiResponse<EmployeeDto>(true, "Tạo nhân viên thành công", employee));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = $"{RoleConstants.Admin},{RoleConstants.HR}")]
        public async Task<IActionResult> UpdateEmployee(int id, [FromBody] UpdateEmployeeDto dto)
        {
            _logger.LogInformation("UpdateEmployee called with Id: {EmployeeId}", id);
            // đảm bảo id trong URL khớp với dto.EmployeeId
            if (id != dto.EmployeeId)
            {
                _logger.LogWarning("UpdateEmployee failed: Id mismatch. URL Id: {UrlId}, DTO Id: {DtoId}", id, dto.EmployeeId);
                return BadRequest(new ApiResponse<string>(false, "Id không khớp", null));
            }

            var employee = await _employeeService.UpdateEmployeeAsync(dto);
            _logger.LogInformation("Employee with Id {EmployeeId} updated successfully", id);
            return Ok(new ApiResponse<EmployeeDto>(true, "Cập nhật nhân viên thành công", employee));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = $"{RoleConstants.Admin},{RoleConstants.HR}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            _logger.LogInformation("DeleteEmployee called with Id: {EmployeeId}", id);
            await _employeeService.DeleteEmployeeAsync(id);
            _logger.LogInformation("Employee with Id {EmployeeId} deleted successfully", id);
            return Ok(new ApiResponse<string>(true, "Xóa nhân viên thành công", null));
        }
    }
}