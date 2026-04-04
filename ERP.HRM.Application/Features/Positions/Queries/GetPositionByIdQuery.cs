using ERP.HRM.Application.DTOs.Position;
using MediatR;

namespace ERP.HRM.Application.Features.Positions.Queries
{
    /// <summary>
    /// Query to get position by ID
    /// </summary>
    public class GetPositionByIdQuery : IRequest<PositionDto>
    {
        public int PositionId { get; set; }
    }
}
