using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ECom;
using EComBusiness.Entity;
using Microsoft.AspNetCore.Authorization;
using EComBusiness.HelperModel;
using ECom.Service;

namespace ECom.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishlistController : ControllerBase
    {
        private readonly EComContext _context;

        public WishlistController(EComContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<WishlistDTO>>> GetWishlistItem([FromQuery] List<string> userId)
        {
            var products = await ProductService.GetProducts(_context, userId);
            var wishItems = await _context.WishLists.AsNoTracking().NotDeleted()
                .Where(x => userId.Contains(x.UserId))
                .ToListAsync();
            return Ok(from product in products
                      join wishitem in wishItems
                      on product.ProductId equals wishitem.ProductId
                      select new WishlistDTO
                      {
                          Product = product,
                          ProductId = product.ProductId,
                          UserId = wishitem.UserId
                      });
        }
        [HttpGet("{userId}")]
        public async Task<ActionResult<IEnumerable<WishlistDTO>>> GetWishlistItem(string userId)
        {
            var products = await ProductService.GetProducts(_context, userId);
            var wishItems = await _context.WishLists.AsNoTracking().NotDeleted()
                .Where(x => x.UserId == userId)
                .ToListAsync();
            return Ok(from product in products
                      join wishitem in wishItems 
                      on product.ProductId equals wishitem.ProductId
                      select new WishlistDTO
                      {
                          Product = product,
                          ProductId = product.ProductId,
                          UserId = wishitem.UserId
                      });
        }

        //POST: api/Wishlist

        [HttpPost]
        public async Task<ActionResult<WishlistItem>> PostWishlistItem(WishlistAddRequest request)
        {
            _context.WishLists.Add(new WishlistItem
            {
                ProductId = request.ProductId,
                UserId = request.UserId
            }.CreateAudit());
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (WishlistItemExists(request.UserId, request.ProductId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetWishlistItem", request);
        }

        [HttpDelete("{id}/{productId}")]
        public async Task<IActionResult> DeleteWishlistItem(string id, string productId)
        {
            var wishlistItem = await _context.WishLists
                .Where(x => x.UserId.Equals(id))
                .FirstOrDefaultAsync(x => x.ProductId.Equals(productId));
            if (wishlistItem == null)
            {
                return NotFound();
            }
            _context.Remove(wishlistItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool WishlistItemExists(string userid, string productid)
        {
            return _context.WishLists.Any(e => e.UserId.Equals(userid) && e.ProductId.Equals(productid));
        }
    }
}
