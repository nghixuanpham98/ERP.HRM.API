using AutoMapper;
using ERP.HRM.API;
using ERP.HRM.Application.DTOs;
using ERP.HRM.Application.Interfaces;
using ERP.HRM.Domain.Exceptions;
using ERP.HRM.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<IEnumerable<PositionDto>> GetAllPositionsAsync()
        {
            var positions = await _positionRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<PositionDto>>(positions);
        }

        public async Task<PositionDto> GetPositionByIdAsync(int id)
        {
            var position = await _positionRepository.GetByIdAsync(id);
            if (position == null) throw new NotFoundException($"Position with Id {id} not found");

            return _mapper.Map<PositionDto>(position);
        }

        public async Task<PositionDto> AddPositionAsync(CreatePositionDto dto)
        {
            if (await _positionRepository.ExistsByCodeAsync(dto.PositionCode))
                throw new BusinessRuleException("Position code already exists");

            var position = _mapper.Map<Position>(dto);
            await _positionRepository.AddAsync(position);

            return _mapper.Map<PositionDto>(position);
        }

        public async Task<PositionDto> UpdatePositionAsync(UpdatePositionDto dto)
        {
            var position = await _positionRepository.GetByIdAsync(dto.PositionId);
            if (position == null) throw new NotFoundException($"Position with Id {dto.PositionId} not found");

            _mapper.Map(dto, position);
            await _positionRepository.UpdateAsync(position);

            return _mapper.Map<PositionDto>(position);
        }

        public async Task DeletePositionAsync(int id)
        {
            var position = await _positionRepository.GetByIdAsync(id);
            if (position == null) throw new NotFoundException($"Position with Id {id} not found");

            await _positionRepository.DeleteAsync(id);
        }
    }
}