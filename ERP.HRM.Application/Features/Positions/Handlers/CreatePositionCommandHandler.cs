using AutoMapper;
using ERP.HRM.Application.DTOs.Position;
using ERP.HRM.Application.Features.Positions.Commands;
using ERP.HRM.Domain.Entities;
using ERP.HRM.Domain.Exceptions;
using ERP.HRM.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ERP.HRM.Application.Features.Positions.Handlers
{
    /// <summary>
    /// Handler for creating a new position
    /// </summary>
    public class CreatePositionCommandHandler : IRequestHandler<CreatePositionCommand, PositionDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<CreatePositionCommandHandler> _logger;

        public CreatePositionCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<CreatePositionCommandHandler> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<PositionDto> Handle(CreatePositionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Creating position with code: {PositionCode}, Name: {PositionName}", 
                    request.PositionCode, request.PositionName);

                // Check if position code already exists
                var positions = await _unitOfWork.PositionRepository.GetAllAsync();
                if (positions.Any(p => p.PositionCode == request.PositionCode))
                {
                    var ex = new ConflictException($"Position code '{request.PositionCode}' already exists");
                    _logger.LogWarning("Duplicate position code: {PositionCode}", request.PositionCode);
                    throw ex;
                }

                var position = _mapper.Map<Position>(request);
                position.CreatedDate = DateTime.UtcNow;

                await _unitOfWork.PositionRepository.AddAsync(position);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Position '{PositionName}' ({PositionCode}) created successfully with ID: {PositionId}", 
                    request.PositionName, request.PositionCode, position.PositionId);

                return _mapper.Map<PositionDto>(position);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating position: {PositionCode}", request.PositionCode);
                throw;
            }
        }
    }
}
