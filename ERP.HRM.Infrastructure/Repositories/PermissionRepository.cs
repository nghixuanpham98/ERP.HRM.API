using ERP.HRM.API;
using ERP.HRM.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ERP.HRM.Infrastructure.Repositories
{
    public class PermissionRepository : IPermissionRepository
    {
        private readonly ERPDbContext _context;

        public PermissionRepository(ERPDbContext context)
        {
            _context = context;
        }

        public async Task<List<string>> GetPermissionsByRoleNameAsync(string roleName)
        {
            return await _context.RolePermissions
                .Where(rp => rp.Role.Name == roleName)
                .Include(rp => rp.Permission)
                .Select(rp => rp.Permission.Name)
                .ToListAsync();
        }
    }
}
