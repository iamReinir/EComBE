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

        public static int Map(UserRole role) => role switch
        {
            UserRole.Admin => 0,
            UserRole.Customer => 1,
            _ => throw new NotImplementedException()
        };

        public static UserRole Map(int role) => role switch
        {
            0 => UserRole.Admin,
            1 => UserRole.Customer,
            _ => throw new NotImplementedException()
        };
    }
    
}
