using ERP.HRM.Application.Common;
using ERP.HRM.Application.DTOs.Position;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.HRM.Application.Interfaces
{
    public interface IPositionService
    {
        Task<PagedResult<PositionDto>> GetAllPositionsAsync(int pageNumber, int pageSize);
        Task<PositionDto> GetPositionByIdAsync(int id);
        Task<PositionDto> AddPositionAsync(CreatePositionDto dto);
        Task<PositionDto> UpdatePositionAsync(UpdatePositionDto dto);
        Task DeletePositionAsync(int id);
    }
}