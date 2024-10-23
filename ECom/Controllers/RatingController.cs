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
using Microsoft.CodeAnalysis;

namespace ECom.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingController : ControllerBase
    {
        private readonly EComContext _context;

        public RatingController(EComContext context)
        {
            _context = context;
        }

        // GET: api/Rating/{userid}
        [HttpGet("{userid}")]
        public async Task<ActionResult<List<RatingItem>>> GetRatingItem(string userid)
        {
            return await _context.Ratings
                .NotDeleted()
                .Where(rate => rate.UserId.Equals(userid))
                .ToListAsync();
        }

        // POST: api/Rating
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<RatingItem>> PostRatingItem([FromBody]RatingRequest ratingItem)
        {
            var rating = await _context.Ratings
                    .FirstOrDefaultAsync(e 
                    => e.UserId == ratingItem.UserId
                    && e.ProductId == ratingItem.ProductId);
            var product = await _context.Products
                .NotDeleted()
                .FirstOrDefaultAsync(p => p.ProductId == ratingItem.ProductId);
            if(product == null)
            {
                return NotFound();
            }
            if(rating == null)
            {
                _context.Ratings.Add(new RatingItem
                {
                    UserId = ratingItem.UserId,
                    ProductId = ratingItem.ProductId,
                    Rating = ratingItem.Rating
                }
                .CreateAudit());
                // Meth
                product.Rating =
                    (ratingItem.Rating + product.Rating * product.RatingCount) 
                    / (product.RatingCount + 1);
                product.RatingCount += 1;
            }
            else
            {
                // Meth
                product.Rating = 
                    (ratingItem.Rating + product.Rating * product.RatingCount - rating.Rating) 
                    / (product.RatingCount);
                rating.Rating = ratingItem.Rating;
                rating.UpdateAudit();                
            }
            
            product.UpdateAudit();
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(PostRatingItem), ratingItem);
        }

        [HttpDelete("{userid}/{productid}")]
        public async Task<IActionResult> DeleteRatingItem(string userid, string productid)
        {
            var ratingItem = await _context.Ratings
                    .FirstOrDefaultAsync(e => e.UserId == userid && e.ProductId == productid);
            var product = await _context.Products
                .NotDeleted()
                .FirstOrDefaultAsync(p => p.ProductId == ratingItem.ProductId);
            if (product == null)
            {
                return NotFound();
            }
            if (ratingItem == null)
            {
                return NotFound();
            }
            // Meth
            if (product.RatingCount - 1 == 0)
            {
                product.Rating = 0;
            }
            else
            {
                product.Rating =
                    (product.Rating * product.RatingCount - ratingItem.Rating)
                    / (product.RatingCount - 1);
            }
            product.RatingCount -= 1;
            product.UpdateAudit();
            _context.Remove(ratingItem);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
