using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.HRM.Domain.Interfaces.Repositories
{
    public interface IPermissionRepository
    {
        Task<List<string>> GetPermissionsByRoleNameAsync(string roleName);
    }
}
