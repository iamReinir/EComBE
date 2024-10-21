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

        // GET: api/Wishlist
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WishlistItem>>> GetWishLists()
        {
            return await _context.WishLists
                .NotDeleted()
                .Include(x => x.Product)
                .ToListAsync();
        }

        // GET: api/Wishlist/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<WishlistItem>>> GetWishlistItem(string id)
        {
            var user = await _context.AppUsers
                .Include(x => x.WishList).AsNoTracking().NotDeleted()
                .FirstOrDefaultAsync(user => user.UserId.Equals(id));

            if (user == null)
            {
                return NotFound();
            }

            return user.WishList.ToList();
        }

        // PUT: api/Wishlist/5
        /*
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWishlistItem(string id, WishlistItem wishlistItem)
        {
            if (id != wishlistItem.UserId)
            {
                return BadRequest();
            }

            _context.Entry(wishlistItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WishlistItemExists(id))
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
        */

        // POST: api/Wishlist
        /*
        [HttpPost]
        public async Task<ActionResult<WishlistItem>> PostWishlistItem(WishlistItem wishlistItem)
        {
            _context.WishLists.Add(wishlistItem);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (WishlistItemExists(wishlistItem.UserId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetWishlistItem", new { id = wishlistItem.UserId }, wishlistItem);
        }
        */

        // DELETE: api/Wishlist/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWishlistItem(string id)
        {
            var wishlistItem = await _context.WishLists.FindAsync(id);
            if (wishlistItem == null)
            {
                return NotFound();
            }

            _context.WishLists.Remove(wishlistItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool WishlistItemExists(string id)
        {
            return _context.WishLists.Any(e => e.UserId == id);
        }
    }
}
