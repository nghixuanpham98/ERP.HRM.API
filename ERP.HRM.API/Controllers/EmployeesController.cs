using ERP.HRM.Application.Common;
using ERP.HRM.Application.DTOs.Employee;
using ERP.HRM.Application.Interfaces;
using ERP.HRM.Domain.Constants;
using ERP.HRM.Domain.Interfaces.Repositories;
using ERP.HRM.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ERP.HRM.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllEmployees(int pageNumber = 1, int pageSize = 10)
        {
            var employees = await _employeeService.GetAllEmployeesAsync(pageNumber, pageSize);
            return Ok(new ApiResponse<PagedResult<EmployeeDto>>(true, "Danh sách nhân viên", employees));
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetEmployeeById(int id)
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(id);
            return Ok(new ApiResponse<EmployeeDto?>(true, "Chi tiết nhân viên", employee));
        }

        [HttpPost]
        [Authorize(Roles = $"{RoleConstants.Admin},{RoleConstants.HR}")]
        public async Task<IActionResult> CreateEmployee([FromBody] CreateEmployeeDto dto)
        {
            var employee = await _employeeService.AddEmployeeAsync(dto);
            return Ok(new ApiResponse<EmployeeDto>(true, "Tạo nhân viên thành công", employee));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = $"{RoleConstants.Admin},{RoleConstants.HR}")]
        public async Task<IActionResult> UpdateEmployee(int id, [FromBody] UpdateEmployeeDto dto)
        {
            // đảm bảo id trong URL khớp với dto.EmployeeId
            if (id != dto.EmployeeId)
                return BadRequest(new ApiResponse<string>(false, "Id không khớp", null));

            var employee = await _employeeService.UpdateEmployeeAsync(dto);
            return Ok(new ApiResponse<EmployeeDto>(true, "Cập nhật nhân viên thành công", employee));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = $"{RoleConstants.Admin},{RoleConstants.HR}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            await _employeeService.DeleteEmployeeAsync(id);
            return Ok(new ApiResponse<string>(true, "Xóa nhân viên thành công", null));
        }
    }
}