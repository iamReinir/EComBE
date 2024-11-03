using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ECom;
using EComBusiness.Entity;
using Grpc.Net.Client;
using Microsoft.EntityFrameworkCore;
using EComAdmin.Proto;
using System.ComponentModel.DataAnnotations;

namespace EComAdmin.Pages.UserManagement
{
    public class CreateModel : PageModel
    {
        private readonly ECom.EComContext _context;
        private readonly GrpcChannel _channel;

        public CreateModel(ECom.EComContext context, GrpcChannel channel)
        {
            _context = context;
            _channel = channel;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public EComBusiness.Entity.User user { get; set; } = default!;

        [BindProperty]
        [MinLength(6, ErrorMessage = "It need to be at least 6 characters")]
        public string Password { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Repeat Password to ensure you type it right")]
        [Compare("Password", ErrorMessage = "Password do not match")]
        public string Repeat_Password { get; set; }
        public async Task<IActionResult> OnPostAsync()
        {
            user.PasswordHash = Password;
            var client = new Auth.AuthClient(_channel);
            var reply = await client.RegisterAsync(new RegisterRequest
            {
                Address = user.Address,
                Email = user.Email,
                Name = user.Name,
                Password = user.PasswordHash, // plain text
                PhoneNumber = user.PhoneNumber
            });
            if(reply.IsOk)
                return RedirectToPage("./Index");
            return Page();
        }
    }
}
