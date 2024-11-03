using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ECom;
using EComBusiness.Entity;

namespace EComAdmin.Pages.UserManagement
{
    public class IndexModel : PageModel
    {
        private readonly ECom.EComContext _context;

        public IndexModel(ECom.EComContext context)
        {
            _context = context;
        }

        public IList<User> User { get;set; } = default!;

        public async Task OnGetAsync()
        {
            User = await _context.AppUsers.ToListAsync();
        }
    }
}
