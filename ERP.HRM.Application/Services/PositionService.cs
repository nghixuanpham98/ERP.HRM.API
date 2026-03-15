using AutoMapper;
using ERP.HRM.Application.DTOs;
using ERP.HRM.Application.Interfaces;
using ERP.HRM.Domain.Interfaces.Repositories;
using ERP.HRM.API;
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

        public async Task<PositionDto?> GetPositionByIdAsync(int id)
        {
            var position = await _positionRepository.GetByIdAsync(id);
            return _mapper.Map<PositionDto?>(position);
        }

        public async Task AddPositionAsync(CreatePositionDto dto)
        {
            var position = _mapper.Map<Position>(dto);
            await _positionRepository.AddAsync(position);
        }

        public async Task UpdatePositionAsync(UpdatePositionDto dto)
        {
            var position = _mapper.Map<Position>(dto);
            await _positionRepository.UpdateAsync(position);
        }

        public async Task DeletePositionAsync(int id)
        {
            await _positionRepository.DeleteAsync(id);
        }
    }
}