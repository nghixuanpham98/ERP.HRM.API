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
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
            if (role == null) return new List<string>();

            return await _context.RolePermissions
                .Where(rp => rp.RoleId == role.Id)
                .Include(rp => rp.Permission)
                .Select(rp => rp.Permission.Name)
                .ToListAsync();
        }
    }
}
