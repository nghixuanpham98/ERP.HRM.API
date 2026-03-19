using AutoMapper;
using ERP.HRM.API;
using ERP.HRM.Application.Common;
using ERP.HRM.Application.DTOs.Position;
using ERP.HRM.Application.Interfaces;
using ERP.HRM.Domain.Exceptions;
using ERP.HRM.Domain.Interfaces.Repositories;

namespace ERP.HRM.Application.Services
{
    public class PositionService : IPositionService
    {
        private readonly IPositionRepository _positionRepository;
        private readonly IMapper _mapper;

        public PositionService(IPositionRepository positionRepository, IMapper mapper)
        {
            _positionRepository = positionRepository;
            _mapper = mapper;
        }

        public async Task<PagedResult<PositionDto>> GetAllPositionsAsync(int pageNumber, int pageSize)
        {
            var (positions, totalCount) = await _positionRepository.GetPagedAsync(pageNumber, pageSize);

            return new PagedResult<PositionDto>
            {
                Items = _mapper.Map<IEnumerable<PositionDto>>(positions),
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<PositionDto> GetPositionByIdAsync(int id)
        {
            var position = await _positionRepository.GetByIdAsync(id);
            if (position == null)
                throw new NotFoundException($"Position with Id {id} not found");

            return _mapper.Map<PositionDto>(position);
        }

        public async Task<PositionDto> AddPositionAsync(CreatePositionDto dto)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(dto.PositionName))
                errors.Add("PositionName is required");

            if (string.IsNullOrWhiteSpace(dto.PositionCode))
                errors.Add("PositionCode is required");

            if (await _positionRepository.ExistsByCodeAsync(dto.PositionCode))
                errors.Add($"Position code '{dto.PositionCode}' already exists");

            if (errors.Any())
            {
                var ex = new ValidationException("Position data is invalid");
                ex.Data["Errors"] = errors;
                throw ex;
            }

            var position = _mapper.Map<Position>(dto);
            await _positionRepository.AddAsync(position);

            return _mapper.Map<PositionDto>(position);
        }

        public async Task<PositionDto> UpdatePositionAsync(UpdatePositionDto dto)
        {
            var position = await _positionRepository.GetByIdAsync(dto.PositionId);
            if (position == null)
                throw new NotFoundException($"Position with Id {dto.PositionId} not found");

            _mapper.Map(dto, position);
            await _positionRepository.UpdateAsync(position);

            return _mapper.Map<PositionDto>(position);
        }

        public async Task DeletePositionAsync(int id)
        {
            var position = await _positionRepository.GetByIdAsync(id);
            if (position == null)
                throw new NotFoundException($"Position with Id {id} not found");

            await _positionRepository.DeleteAsync(id);
        }
    }
}
