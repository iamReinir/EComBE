using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EComBusiness.HelperModel
{
    public enum UserRole
    {
        Admin,
        Customer
    }
    public static class Role
    {
        static public string[] Roles { get; } = { 
            nameof(UserRole.Admin),
            nameof(UserRole.Customer)
        };
    }
}
