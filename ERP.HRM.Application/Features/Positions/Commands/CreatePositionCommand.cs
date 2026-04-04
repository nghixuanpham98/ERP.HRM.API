using ERP.HRM.Application.DTOs.Position;
using MediatR;

namespace ERP.HRM.Application.Features.Positions.Commands
{
    /// <summary>
    /// Command to create a new position
    /// </summary>
    public class CreatePositionCommand : IRequest<PositionDto>
    {
        public string PositionName { get; set; } = null!;
        public string PositionCode { get; set; } = null!;
        public string? Description { get; set; }
        public decimal? Allowance { get; set; }
        public string? Status { get; set; }
        public int? Level { get; set; }
    }
}
