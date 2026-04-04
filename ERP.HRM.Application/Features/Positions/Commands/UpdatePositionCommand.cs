using ERP.HRM.Application.DTOs.Position;
using MediatR;

namespace ERP.HRM.Application.Features.Positions.Commands
{
    /// <summary>
    /// Command to update an existing position
    /// </summary>
    public class UpdatePositionCommand : IRequest<PositionDto>
    {
        public int PositionId { get; set; }
        public string PositionName { get; set; } = null!;
        public string PositionCode { get; set; } = null!;
        public string? Description { get; set; }
        public decimal? Allowance { get; set; }
        public string? Status { get; set; }
        public int? Level { get; set; }
    }
}
