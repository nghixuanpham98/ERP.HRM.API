using ERP.HRM.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.HRM.Application.Interfaces.Services
{
    public interface IPositionService
    {
        Task<IEnumerable<PositionDto>> GetAllPositionsAsync();
        Task<PositionDto> GetPositionByIdAsync(int id);
        Task<PositionDto> AddPositionAsync(CreatePositionDto dto);
        Task<PositionDto> UpdatePositionAsync(UpdatePositionDto dto);
        Task DeletePositionAsync(int id);
    }
}