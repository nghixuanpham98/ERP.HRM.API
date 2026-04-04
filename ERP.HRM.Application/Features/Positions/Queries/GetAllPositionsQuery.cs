using ERP.HRM.Application.Common;
using ERP.HRM.Application.DTOs.Position;
using MediatR;

namespace ERP.HRM.Application.Features.Positions.Queries
{
    /// <summary>
    /// Query to get all positions with pagination
    /// </summary>
    public class GetAllPositionsQuery : IRequest<PagedResult<PositionDto>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SearchTerm { get; set; }
        public string? Status { get; set; }
        public int? Level { get; set; }
    }
}
