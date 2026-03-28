using ERP.HRM.Application.Common;
using ERP.HRM.Application.DTOs.Department;
using ERP.HRM.Application.Features.Departments.Commands;
using ERP.HRM.Application.Features.Departments.Queries;
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
    public class DepartmentController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<DepartmentController> _logger;

        public DepartmentController(IMediator mediator, ILogger<DepartmentController> logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllDepartments(int pageNumber = 1, int pageSize = 10)
        {
            _logger.LogInformation("GetAllDepartments called with PageNumber: {PageNumber}, PageSize: {PageSize}", pageNumber, pageSize);
            var query = new GetAllDepartmentsQuery { PageNumber = pageNumber, PageSize = pageSize };
            var departments = await _mediator.Send(query);
            return Ok(new ApiResponse<PagedResult<DepartmentDto>>(true, "Danh sách phòng ban", departments));
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetDepartmentById(int id)
        {
            _logger.LogInformation("GetDepartmentById called with Id: {DepartmentId}", id);
            var query = new GetDepartmentByIdQuery { DepartmentId = id };
            var department = await _mediator.Send(query);
            return Ok(new ApiResponse<DepartmentDto>(true, "Chi tiết phòng ban", department));
        }

        [HttpPost]
        [Authorize(Roles = $"{RoleConstants.Admin}")]
        public async Task<IActionResult> CreateDepartment([FromBody] CreateDepartmentCommand command)
        {
            _logger.LogInformation("CreateDepartment called with DepartmentName: {DepartmentName}", command.DepartmentName);
            var department = await _mediator.Send(command);
            _logger.LogInformation("Department '{DepartmentName}' created successfully", command.DepartmentName);
            return Ok(new ApiResponse<DepartmentDto>(true, "Tạo phòng ban thành công", department));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = $"{RoleConstants.Admin}")]
        public async Task<IActionResult> UpdateDepartment(int id, [FromBody] UpdateDepartmentDto dto)
        {
            _logger.LogInformation("UpdateDepartment called with Id: {DepartmentId}", id);
            if (id != dto.DepartmentId)
            {
                _logger.LogWarning("UpdateDepartment failed: Id mismatch. URL Id: {UrlId}, DTO Id: {DtoId}", id, dto.DepartmentId);
                return BadRequest(new ApiResponse<string>(false, "Id không khớp", null));
            }

            var command = new UpdateDepartmentCommand
            {
                DepartmentId = dto.DepartmentId,
                DepartmentName = dto.DepartmentName,
                DepartmentCode = dto.DepartmentCode,
                Description = dto.Description,
                ParentDepartmentId = dto.ParentDepartmentId,
                HeadOfDepartmentId = dto.HeadOfDepartmentId
            };

            var department = await _mediator.Send(command);
            _logger.LogInformation("Department with Id {DepartmentId} updated successfully", id);
            return Ok(new ApiResponse<DepartmentDto>(true, "Cập nhật phòng ban thành công", department));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = $"{RoleConstants.Admin}")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            _logger.LogInformation("DeleteDepartment called with Id: {DepartmentId}", id);
            var command = new DeleteDepartmentCommand { DepartmentId = id };
            await _mediator.Send(command);
            _logger.LogInformation("Department with Id {DepartmentId} deleted successfully", id);
            return Ok(new ApiResponse<string>(true, "Xóa phòng ban thành công", null));
        }
    }
}
