using AutoMapper;
using ERP.HRM.Application.DTOs.Position;
using ERP.HRM.Application.Features.Positions.Commands;
using ERP.HRM.Domain.Exceptions;
using ERP.HRM.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ERP.HRM.Application.Features.Positions.Handlers
{
    /// <summary>
    /// Handler for updating an existing position
    /// </summary>
    public class UpdatePositionCommandHandler : IRequestHandler<UpdatePositionCommand, PositionDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdatePositionCommandHandler> _logger;

        public UpdatePositionCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<UpdatePositionCommandHandler> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<PositionDto> Handle(UpdatePositionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Updating position with ID: {PositionId}", request.PositionId);

                // Find the position
                var position = await _unitOfWork.PositionRepository.GetByIdAsync(request.PositionId);
                if (position == null)
                {
                    var ex = new NotFoundException("Position", request.PositionId);
                    _logger.LogWarning("Position not found with ID: {PositionId}", request.PositionId);
                    throw ex;
                }

                // Check if position code is being changed and if new code already exists
                if (position.PositionCode != request.PositionCode)
                {
                    var positions = await _unitOfWork.PositionRepository.GetAllAsync();
                    if (positions.Any(p => p.PositionCode == request.PositionCode && p.PositionId != request.PositionId))
                    {
                        var ex = new ConflictException($"Position code '{request.PositionCode}' already exists");
                        _logger.LogWarning("Duplicate position code during update: {PositionCode}", request.PositionCode);
                        throw ex;
                    }
                }

                // Map and update
                _mapper.Map(request, position);
                position.ModifiedDate = DateTime.UtcNow;

                await _unitOfWork.PositionRepository.UpdateAsync(position);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Position with ID: {PositionId} updated successfully", request.PositionId);

                return _mapper.Map<PositionDto>(position);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating position with ID: {PositionId}", request.PositionId);
                throw;
            }
        }
    }
}
