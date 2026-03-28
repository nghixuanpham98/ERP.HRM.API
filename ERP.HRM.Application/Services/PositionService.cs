using AutoMapper;
using ERP.HRM.Domain.Entities;
using ERP.HRM.Application.Common;
using ERP.HRM.Application.DTOs.Position;
using ERP.HRM.Application.Interfaces;
using ERP.HRM.Domain.Exceptions;
using ERP.HRM.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Logging;

namespace ERP.HRM.Application.Services
{
    public class PositionService : IPositionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<PositionService> _logger;

        public PositionService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<PositionService> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PagedResult<PositionDto>> GetAllPositionsAsync(int pageNumber, int pageSize)
        {
            _logger.LogInformation("Fetching all positions. Page: {PageNumber}, Size: {PageSize}", pageNumber, pageSize);
            var (positions, totalCount) = await _unitOfWork.PositionRepository.GetPagedAsync(pageNumber, pageSize);

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
            _logger.LogInformation("Fetching position with Id {PositionId}", id);
            var position = await _unitOfWork.PositionRepository.GetByIdAsync(id);
            if (position == null)
            {
                _logger.LogWarning("Position with Id {PositionId} not found", id);
                throw new NotFoundException($"Position with Id {id} not found");
            }

            return _mapper.Map<PositionDto>(position);
        }

        public async Task<PositionDto> AddPositionAsync(CreatePositionDto dto)
        {
            try
            {
                var position = _mapper.Map<Position>(dto);
                await _unitOfWork.PositionRepository.AddAsync(position);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Position '{PositionName}' created successfully", dto.PositionName);

                return _mapper.Map<PositionDto>(position);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating position");
                throw;
            }
        }

        public async Task<PositionDto> UpdatePositionAsync(UpdatePositionDto dto)
        {
            try
            {
                _logger.LogInformation("Updating position with Id {PositionId}", dto.PositionId);
                var position = await _unitOfWork.PositionRepository.GetByIdAsync(dto.PositionId);
                if (position == null)
                {
                    _logger.LogWarning("Position with Id {PositionId} not found", dto.PositionId);
                    throw new NotFoundException($"Position with Id {dto.PositionId} not found");
                }

                _mapper.Map(dto, position);
                await _unitOfWork.PositionRepository.UpdateAsync(position);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Position with Id {PositionId} updated successfully", dto.PositionId);

                return _mapper.Map<PositionDto>(position);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating position");
                throw;
            }
        }

        public async Task DeletePositionAsync(int id)
        {
            try
            {
                _logger.LogInformation("Deleting position with Id {PositionId}", id);
                var position = await _unitOfWork.PositionRepository.GetByIdAsync(id);
                if (position == null)
                {
                    _logger.LogWarning("Position with Id {PositionId} not found", id);
                    throw new NotFoundException($"Position with Id {id} not found");
                }

                await _unitOfWork.PositionRepository.SoftDeleteAsync(id);
                await _unitOfWork.SaveChangesAsync();
                _logger.LogInformation("Position with Id {PositionId} deleted successfully", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting position");
                throw;
            }
        }
    }
}
