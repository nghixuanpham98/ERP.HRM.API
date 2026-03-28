using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.HRM.Domain.Constants
{
    public static class RoleConstants
    {
        public const string Admin = "Admin";
        public const string HR = "HR";
        public const string Accountant = "Accountant";
        public const string Employee = "Employee";

        public static readonly string[] AllRoles =
            [Admin, HR, Accountant, Employee];
    }
}
