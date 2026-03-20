using ERP.HRM.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.HRM.Domain.Interfaces.Repositories
{
    public interface IUserRefreshTokenRepository
    {
        Task<UserRefreshToken?> GetByTokenAsync(string token);
        Task AddAsync(UserRefreshToken refreshToken);
        Task RevokeAsync(UserRefreshToken refreshToken);
    }

}
