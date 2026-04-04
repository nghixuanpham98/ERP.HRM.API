using AutoMapper;
using ERP.HRM.Application.Common;
using ERP.HRM.Application.DTOs.HR;
using ERP.HRM.Domain.Constants;
using ERP.HRM.Domain.Entities;
using ERP.HRM.Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ERP.HRM.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TaxBracketsController : ControllerBase
    {
        private readonly ITaxBracketRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<TaxBracketsController> _logger;

        public TaxBracketsController(
            ITaxBracketRepository repository,
            IMapper mapper,
            ILogger<TaxBracketsController> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Get all tax brackets
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                _logger.LogInformation("Getting all tax brackets");
                var brackets = await _repository.GetAllAsync();
                var dtos = _mapper.Map<IEnumerable<TaxBracketDto>>(brackets);
                return Ok(new ApiResponse<IEnumerable<TaxBracketDto>>(true, "Lấy danh sách bậc thuế thành công", dtos));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting tax brackets");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }

        /// <summary>
        /// Get active tax brackets
        /// </summary>
        [HttpGet("active")]
        public async Task<IActionResult> GetActiveBrackets()
        {
            try
            {
                _logger.LogInformation("Getting active tax brackets");
                var brackets = await _repository.GetActiveBracketsAsync(DateTime.Now);
                var dtos = _mapper.Map<IEnumerable<TaxBracketDto>>(brackets);
                return Ok(new ApiResponse<IEnumerable<TaxBracketDto>>(true, "Lấy danh sách bậc thuế hoạt động thành công", dtos));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting active brackets");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }

        /// <summary>
        /// Get tax bracket by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                _logger.LogInformation("Getting tax bracket {Id}", id);
                var bracket = await _repository.GetByIdAsync(id);
                if (bracket == null)
                    return NotFound(new ApiResponse<string>(false, "Không tìm thấy bậc thuế", null));

                var dto = _mapper.Map<TaxBracketDto>(bracket);
                return Ok(new ApiResponse<TaxBracketDto>(true, "Lấy bậc thuế thành công", dto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting tax bracket");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }

        /// <summary>
        /// Create a new tax bracket
        /// </summary>
        [HttpPost]
        [Authorize(Roles = RoleConstants.Admin)]
        public async Task<IActionResult> Create([FromBody] CreateTaxBracketDto createDto)
        {
            try
            {
                _logger.LogInformation("Creating new tax bracket: {BracketName}", createDto.BracketName);
                
                var bracket = new TaxBracket
                {
                    BracketName = createDto.BracketName,
                    MinIncome = createDto.MinIncome,
                    MaxIncome = createDto.MaxIncome,
                    TaxRate = createDto.TaxRate,
                    EffectiveDate = createDto.EffectiveDate,
                    IsActive = true
                };

                await _repository.AddAsync(bracket);
                var dto = _mapper.Map<TaxBracketDto>(bracket);
                return CreatedAtAction(nameof(GetById), new { id = bracket.TaxBracketId }, dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating tax bracket");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }

        /// <summary>
        /// Update a tax bracket
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = RoleConstants.Admin)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateTaxBracketDto updateDto)
        {
            try
            {
                _logger.LogInformation("Updating tax bracket {Id}", id);
                
                var bracket = await _repository.GetByIdAsync(id);
                if (bracket == null)
                    return NotFound(new ApiResponse<string>(false, "Không tìm thấy bậc thuế", null));

                bracket.BracketName = updateDto.BracketName;
                bracket.MinIncome = updateDto.MinIncome;
                bracket.MaxIncome = updateDto.MaxIncome;
                bracket.TaxRate = updateDto.TaxRate;
                bracket.EndDate = updateDto.EndDate;
                bracket.IsActive = updateDto.IsActive;

                await _repository.UpdateAsync(bracket);
                var dto = _mapper.Map<TaxBracketDto>(bracket);
                return Ok(new ApiResponse<TaxBracketDto>(true, "Cập nhật bậc thuế thành công", dto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating tax bracket");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }

        /// <summary>
        /// Delete a tax bracket
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = RoleConstants.Admin)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                _logger.LogInformation("Deleting tax bracket {Id}", id);
                
                var bracket = await _repository.GetByIdAsync(id);
                if (bracket == null)
                    return NotFound(new ApiResponse<string>(false, "Không tìm thấy bậc thuế", null));

                await _repository.DeleteAsync(id);
                return Ok(new ApiResponse<string>(true, "Xóa bậc thuế thành công", null));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting tax bracket");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }
    }
}
