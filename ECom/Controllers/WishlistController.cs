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
            return await _context.WishLists
                .AsNoTracking()
                .NotDeleted()
                .Where(x => !userId.Any() || userId.Contains(x.UserId))
                .Include(x => x.Product)
                .Select(x => new WishlistDTO
                {
                    Product = x.Product,
                    ProductId = x.ProductId,
                    UserId = x.UserId
                })
                .ToListAsync();
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<IEnumerable<WishlistDTO>>> GetWishlistItem(string userId)
        {
            return await _context.WishLists
                .AsNoTracking()
                .NotDeleted()
                .Where(x => x.UserId.Equals(userId))
                .Include(x => x.Product)
                .Select(x => new WishlistDTO
                {
                    Product = x.Product,
                    ProductId = x.ProductId,
                    UserId = x.UserId
                })
                .ToListAsync();
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
                .NotDeleted()
                .Where(x => x.UserId.Equals(id))
                .FirstOrDefaultAsync(x => x.ProductId.Equals(productId));
            if (wishlistItem == null)
            {
                return NotFound();
            }
            wishlistItem.SoftDelete();
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool WishlistItemExists(string userid, string productid)
        {
            return _context.WishLists.Any(e => e.UserId.Equals(userid) && e.ProductId.Equals(productid));
        }
    }
}
