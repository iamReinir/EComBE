using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ECom;
using EComBusiness.Entity;

namespace EComAdmin.Pages.ProductManagement
{
    public class IndexModel : PageModel
    {
        private readonly ECom.EComContext _context;

        public IndexModel(ECom.EComContext context)
        {
            _context = context;
        }

        public IList<Product> Product { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Product = await _context.Products
                .Include(p => p.Category).ToListAsync();
        }
    }
}
