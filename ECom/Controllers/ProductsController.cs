using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ECom;
using EComBusiness.Entity;
using EComBusiness.HelperModel;
using ECom.Migrations;

namespace ECom.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly EComContext _context;

        public ProductsController(EComContext context)
        {
            _context = context;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProducts([FromQuery] string? userId)
        {
            var products = await _context.Products
                .NotDeleted()
                .Include(p => p.Category)
                .ToListAsync();
            IEnumerable<string> wishedListed = new List<string>();
            IEnumerable<string> rated = new List<string>();
            if (string.IsNullOrEmpty(userId) == false)
            {
                wishedListed = await (from product in _context.Products.NotDeleted()
                                       join wishlist in _context.WishLists.NotDeleted()
                                       on product.ProductId equals wishlist.ProductId
                                       where wishlist.UserId == userId
                                       select product.ProductId).ToListAsync();
                rated = await (from product in _context.Products.NotDeleted()
                               join wishlist in _context.Ratings.NotDeleted()
                               on product.ProductId equals wishlist.ProductId
                               where wishlist.UserId == userId
                               select product.ProductId).ToListAsync();
            }
            var result =  (from product in products
                    join productid in wishedListed
                    on product.ProductId equals productid into productids
                    from productid in productids.DefaultIfEmpty()
                    select new ProductDTO
                    {
                        ProductId = product.ProductId,
                        Description = product.Description,
                        Name = product.Name,
                        ImageUrl = product.ImageUrl,
                        Price = product.Price,
                        QuantityAvailable = product.QuantityAvailable,
                        Rating = product.Rating,
                        RatingCount = product.RatingCount,
                        IsWishlisted = !string.IsNullOrEmpty(productid),
                        Category = new CategoryDTO
                        {
                            CategoryId = product.Category?.CategoryId,
                            Description = product.Category?.Description,
                            ImageUrl = product.Category?.ImageUrl,
                            Name = product.Category?.Name
                        }
                    });
            result = (from product in result
                      join productid in rated
                      on product.ProductId equals productid into productids
                      from productid in productids.DefaultIfEmpty()
                      select new { product, isRated = !string.IsNullOrEmpty(productid) })
                      .Select(p => 
                      {
                          p.product.IsRated = p.isRated;
                          return p.product;
                      });
            return Ok(result ?? new List<ProductDTO>());
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(string id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(string id, Product product)
        {
            if (id != product.ProductId)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            _context.Products.Add(product);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ProductExists(product.ProductId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetProduct", new { id = product.ProductId }, product);
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductExists(string id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }
    }
}
