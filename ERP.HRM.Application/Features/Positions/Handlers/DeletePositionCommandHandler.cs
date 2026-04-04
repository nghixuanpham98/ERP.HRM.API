using ERP.HRM.Application.Features.Positions.Commands;
using ERP.HRM.Domain.Exceptions;
using ERP.HRM.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ERP.HRM.Application.Features.Positions.Handlers
{
    /// <summary>
    /// Handler for deleting (soft delete) a position
    /// </summary>
    public class DeletePositionCommandHandler : IRequestHandler<DeletePositionCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DeletePositionCommandHandler> _logger;

        public DeletePositionCommandHandler(IUnitOfWork unitOfWork, ILogger<DeletePositionCommandHandler> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<bool> Handle(DeletePositionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Deleting position with ID: {PositionId}", request.PositionId);

                // Find the position
                var position = await _unitOfWork.PositionRepository.GetByIdAsync(request.PositionId);
                if (position == null)
                {
                    var ex = new NotFoundException("Position", request.PositionId);
                    _logger.LogWarning("Position not found with ID: {PositionId}", request.PositionId);
                    throw ex;
                }

                // Soft delete
                position.IsDeleted = true;
                position.ModifiedDate = DateTime.UtcNow;
                await _unitOfWork.PositionRepository.UpdateAsync(position);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Position with ID: {PositionId} deleted successfully", request.PositionId);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting position with ID: {PositionId}", request.PositionId);
                throw;
            }
        }
    }
}
