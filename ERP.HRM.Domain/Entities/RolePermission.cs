using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.HRM.Domain.Entities
{
    public class RolePermission
    {
        public Guid RoleId { get; set; }

        // FK tới AspNetRoles
        public IdentityRole<Guid> Role { get; set; }

        // FK tới Permissions
        public Guid PermissionId { get; set; }

        public Permission Permission { get; set; }
    }

}
