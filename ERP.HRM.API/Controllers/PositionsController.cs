using ERP.HRM.Application.Common;
using ERP.HRM.Application.DTOs.Position;
using ERP.HRM.Application.Interfaces;
using ERP.HRM.Domain.Interfaces.Repositories;
using ERP.HRM.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ERP.HRM.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PositionController : ControllerBase
    {
        private readonly IPositionService _positionService;

        public PositionController(IPositionService positionService)
        {
            _positionService = positionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPositions(int pageNumber = 1, int pageSize = 10)
        {
            var positions = await _positionService.GetAllPositionsAsync(pageNumber, pageSize);
            return Ok(new ApiResponse<PagedResult<PositionDto>>(true, "Danh sách chức vụ", positions));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPositionById(int id)
        {
            var position = await _positionService.GetPositionByIdAsync(id);
            return Ok(new ApiResponse<PositionDto>(true, "Chi tiết chức vụ", position));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreatePosition([FromBody] CreatePositionDto dto)
        {
            var position = await _positionService.AddPositionAsync(dto);
            return Ok(new ApiResponse<PositionDto>(true, "Tạo chức vụ thành công", position));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdatePosition(int id, [FromBody] UpdatePositionDto dto)
        {
            if (id != dto.PositionId)
                return BadRequest(new ApiResponse<string>(false, "Id không khớp", null));

            var position = await _positionService.UpdatePositionAsync(dto);
            return Ok(new ApiResponse<PositionDto>(true, "Cập nhật chức vụ thành công", position));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeletePosition(int id)
        {
            await _positionService.DeletePositionAsync(id);
            return Ok(new ApiResponse<string>(true, "Xóa chức vụ thành công", null));
        }
    }
}