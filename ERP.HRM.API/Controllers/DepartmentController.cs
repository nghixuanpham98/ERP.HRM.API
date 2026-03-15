using ERP.HRM.Application.DTOs;
using ERP.HRM.Application.Interfaces;
using ERP.HRM.Domain.Interfaces.Repositories;
using ERP.HRM.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace ERP.HRM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var departments = await _departmentService.GetAllDepartmentsAsync();
            return Ok(departments);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var department = await _departmentService.GetDepartmentByIdAsync(id);
            if (department == null) return NotFound();
            return Ok(department);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateDepartmentDto dto)
        {
            await _departmentService.AddDepartmentAsync(dto);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateDepartmentDto dto)
        {
            if (id != dto.DepartmentId) return BadRequest();
            await _departmentService.UpdateDepartmentAsync(dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _departmentService.DeleteDepartmentAsync(id);
            return NoContent();
        }
    }
}
