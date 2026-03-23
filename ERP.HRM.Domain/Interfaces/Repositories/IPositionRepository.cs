using ERP.HRM.Domain.Entities;
using ERP.HRM.Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.HRM.Domain.Interfaces.Repositories
{
    public interface IPositionRepository : IPagedRepository<Position>
    {
        Task<IEnumerable<Position>> GetAllAsync();
        Task<Position?> GetByIdAsync(int id);
        Task AddAsync(Position position);
        Task UpdateAsync(Position position);
        Task DeleteAsync(int id);
        Task<bool> ExistsByCodeAsync(string positionCode);
        Task<int> GetEmployeeCountAsync(int positionId);
    }
}
