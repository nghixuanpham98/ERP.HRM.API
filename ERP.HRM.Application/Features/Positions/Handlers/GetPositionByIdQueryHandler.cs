using AutoMapper;
using ERP.HRM.Application.Common;
using ERP.HRM.Application.DTOs.Position;
using ERP.HRM.Application.Features.Positions.Queries;
using ERP.HRM.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ERP.HRM.Application.Features.Positions.Handlers
{
    /// <summary>
    /// Handler for getting position by ID
    /// </summary>
    public class GetPositionByIdQueryHandler : IRequestHandler<GetPositionByIdQuery, PositionDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<GetPositionByIdQueryHandler> _logger;

        public GetPositionByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetPositionByIdQueryHandler> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<PositionDto> Handle(GetPositionByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Getting position with ID: {PositionId}", request.PositionId);

                var position = await _unitOfWork.PositionRepository.GetByIdAsync(request.PositionId);
                if (position == null)
                {
                    var ex = new Domain.Exceptions.NotFoundException("Position", request.PositionId);
                    _logger.LogWarning("Position not found with ID: {PositionId}", request.PositionId);
                    throw ex;
                }

                return _mapper.Map<PositionDto>(position);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting position with ID: {PositionId}", request.PositionId);
                throw;
            }
        }
    }
}
