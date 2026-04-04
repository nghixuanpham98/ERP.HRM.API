using MediatR;

namespace ERP.HRM.Application.Features.Positions.Commands
{
    /// <summary>
    /// Command to delete (soft delete) a position
    /// </summary>
    public class DeletePositionCommand : IRequest<bool>
    {
        public int PositionId { get; set; }

        public DeletePositionCommand(int positionId)
        {
            PositionId = positionId;
        }
    }
}
