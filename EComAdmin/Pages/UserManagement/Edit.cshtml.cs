using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ECom;
using EComBusiness.Entity;
using EComBusiness.HelperModel;
using ECom.Service;
using Grpc.Net.Client;
using System.Runtime.CompilerServices;

namespace EComAdmin.Pages.UserManagement
{
    public class EditModel : PageModel
    {
        private readonly ECom.EComContext _context;
        private readonly GrpcChannel _channel;

        public EditModel(ECom.EComContext context, GrpcChannel channel)
        {
            _context = context;
            _channel = channel;
        }

        [BindProperty]
        public User User { get; set; } = default!;

        [BindProperty]
        public string? NewPassword { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            var client = new Proto.UserManagement.UserManagementClient(_channel);

            var reply = await client.GetUserAsync(new Proto.GetUserRequest
            {
                Id = id
            });

            if (reply.IsOk == false)
            {
                return NotFound();
            }
            User = new User
            {
                Address = reply.User.Address,
                Email = reply.User.Email,
                Name = reply.User.Name,
                PhoneNumber = reply.User.Phonenumber,
                Role = Role.Map(reply.User.Role),
                UserId = reply.User.Id,
                PasswordHash = reply.User.Password,
            };
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var client = new Proto.UserManagement.UserManagementClient(_channel);

            if (!string.IsNullOrEmpty(NewPassword))
            {
                User.PasswordHash = NewPassword;
            }
            var reply = await client.UpdateUserAsync(new Proto.UpdateUserRequest
            {
                Id = User.UserId,
                Address = User.Address,
                Password = User.PasswordHash,
                Email = User.Email,
                Name= User.Name,
                Phonenumber = User.PhoneNumber,
                Role = Role.Map(User.Role),
            });

            if(reply.IsOk)
                return RedirectToPage("./Index");
            return Page();
        }

        private bool UserExists(string id)
        {
            return _context.AppUsers.Any(e => e.UserId == id);
        }
    }
}
