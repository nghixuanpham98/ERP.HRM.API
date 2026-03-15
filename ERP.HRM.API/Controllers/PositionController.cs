using ERP.HRM.Domain.Interfaces.Repositories;
using ERP.HRM.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace ERP.HRM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PositionController : ControllerBase
    {
        private readonly IPositionRepository _positionRepository;

        public PositionController(IPositionRepository positionRepository)
        {
            _positionRepository = positionRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var positions = await _positionRepository.GetAllAsync();
            return Ok(positions);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var position = await _positionRepository.GetByIdAsync(id);
            if (position == null) return NotFound();
            return Ok(position);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Position position)
        {
            await _positionRepository.AddAsync(position);
            return CreatedAtAction(nameof(GetById), new { id = position.PositionId }, position);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Position position)
        {
            if (id != position.PositionId) return BadRequest();
            await _positionRepository.UpdateAsync(position);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _positionRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}
