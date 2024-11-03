using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ECom;
using EComBusiness.Entity;
using Grpc.Net.Client;

namespace EComAdmin.Pages.UserManagement
{
    public class IndexModel : PageModel
    {
        private readonly ECom.EComContext _context;
        private readonly GrpcChannel _channel;

        public IndexModel(ECom.EComContext context, GrpcChannel channel)
        {
            _context = context;
            _channel = channel;
        }

        public IList<User> User { get;set; } = default!;

        public async Task OnGetAsync()
        {
            var client = new Proto.UserManagement.UserManagementClient(_channel);

            var result = await client.ListUsersAsync(new Proto.ListUsersRequest
            {
                Page = 1,
                PageSize = 100
            });
            User = result.Users
                .Select(x => new User
                {
                    UserId = x.Id,
                    Role = EComBusiness.HelperModel.Role.Map(x.Role),
                    PhoneNumber = x.Phonenumber,
                    Name = x.Name,
                    Email = x.Email,
                    IsDeleted = false,
                    Address = x.Address
                })
                .ToList();
        }
    }
}
