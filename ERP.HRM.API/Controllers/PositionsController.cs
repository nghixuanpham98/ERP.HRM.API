using ERP.HRM.Application.Common;
using ERP.HRM.Application.DTOs.Position;
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
    public class PositionController : ControllerBase
    {
        private readonly IPositionService _positionService;
        private readonly ILogger<PositionController> _logger;

        public PositionController(IPositionService positionService, ILogger<PositionController> logger)
        {
            _positionService = positionService;
            _logger = logger;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllPositions(int pageNumber = 1, int pageSize = 10)
        {
            _logger.LogInformation("GetAllPositions called with PageNumber: {PageNumber}, PageSize: {PageSize}", pageNumber, pageSize);
            var positions = await _positionService.GetAllPositionsAsync(pageNumber, pageSize);
            return Ok(new ApiResponse<PagedResult<PositionDto>>(true, "Danh sách chức vụ", positions));
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetPositionById(int id)
        {
            _logger.LogInformation("GetPositionById called with Id: {PositionId}", id);
            var position = await _positionService.GetPositionByIdAsync(id);
            return Ok(new ApiResponse<PositionDto>(true, "Chi tiết chức vụ", position));
        }

        [HttpPost]
        [Authorize(Roles = $"{RoleConstants.Admin}")]
        public async Task<IActionResult> CreatePosition([FromBody] CreatePositionDto dto)
        {
            _logger.LogInformation("CreatePosition called with PositionName: {PositionName}", dto.PositionName);
            var position = await _positionService.AddPositionAsync(dto);
            _logger.LogInformation("Position '{PositionName}' created successfully", dto.PositionName);
            return Ok(new ApiResponse<PositionDto>(true, "Tạo chức vụ thành công", position));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = $"{RoleConstants.Admin}")]
        public async Task<IActionResult> UpdatePosition(int id, [FromBody] UpdatePositionDto dto)
        {
            _logger.LogInformation("UpdatePosition called with Id: {PositionId}", id);
            if (id != dto.PositionId)
            {
                _logger.LogWarning("UpdatePosition failed: Id mismatch. URL Id: {UrlId}, DTO Id: {DtoId}", id, dto.PositionId);
                return BadRequest(new ApiResponse<string>(false, "Id không khớp", null));
            }

            var position = await _positionService.UpdatePositionAsync(dto);
            _logger.LogInformation("Position with Id {PositionId} updated successfully", id);
            return Ok(new ApiResponse<PositionDto>(true, "Cập nhật chức vụ thành công", position));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = $"{RoleConstants.Admin}")]
        public async Task<IActionResult> DeletePosition(int id)
        {
            _logger.LogInformation("DeletePosition called with Id: {PositionId}", id);
            await _positionService.DeletePositionAsync(id);
            _logger.LogInformation("Position with Id {PositionId} deleted successfully", id);
            return Ok(new ApiResponse<string>(true, "Xóa chức vụ thành công", null));
        }
    }
}