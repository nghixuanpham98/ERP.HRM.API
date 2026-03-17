using ERP.HRM.Application.Common;
using ERP.HRM.Application.DTOs;
using ERP.HRM.Application.Interfaces.Services;
using ERP.HRM.Domain.Interfaces.Repositories;
using ERP.HRM.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace ERP.HRM.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDepartments()
        {
            var departments = await _departmentService.GetAllDepartmentsAsync();
            return Ok(new ApiResponse<IEnumerable<DepartmentDto>>(true, "Danh sách phòng ban", departments));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDepartmentById(int id)
        {
            var department = await _departmentService.GetDepartmentByIdAsync(id);
            return Ok(new ApiResponse<DepartmentDto>(true, "Chi tiết phòng ban", department));
        }

        [HttpPost]
        public async Task<IActionResult> CreateDepartment([FromBody] CreateDepartmentDto dto)
        {
            var department = await _departmentService.AddDepartmentAsync(dto);
            return Ok(new ApiResponse<DepartmentDto>(true, "Tạo phòng ban thành công", department));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDepartment(int id, [FromBody] UpdateDepartmentDto dto)
        {
            if (id != dto.DepartmentId)
                return BadRequest(new ApiResponse<string>(false, "Id không khớp", null));

            var department = await _departmentService.UpdateDepartmentAsync(dto);
            return Ok(new ApiResponse<DepartmentDto>(true, "Cập nhật phòng ban thành công", department));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            await _departmentService.DeleteDepartmentAsync(id);
            return Ok(new ApiResponse<string>(true, "Xóa phòng ban thành công", null));
        }
    }
}
