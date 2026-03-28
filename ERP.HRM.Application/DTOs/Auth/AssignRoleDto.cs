using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.HRM.Application.DTOs.Auth
{
    public class AssignRoleDto
    {
        public string Username { get; set; } = null!;
        public string Role { get; set; } = null!;
    }
}
