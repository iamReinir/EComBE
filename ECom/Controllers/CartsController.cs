using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ECom;
using EComBusiness.Entity;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using ECom.ViewModels.CartItem;

namespace ECom.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //  [Authorize]
    public class CartsController : ControllerBase
    {
        private readonly EComContext _context;

        public CartsController(EComContext context)
        {
            _context = context;
        }

        // GET: api/Carts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cart>>> GetCarts()
        {
            return await _context.Carts.ToListAsync();
        }

        // GET: api/Carts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Cart>> GetCart(string id)
        {
            var cart = await _context.Carts.FindAsync(id);

            if (cart == null)
            {
                return NotFound();
            }

            return cart;
        }

        // PUT: api/Carts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCart(string id, Cart cart)
        {
            if (id != cart.CartId)
            {
                return BadRequest();
            }

            _context.Entry(cart).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CartExists(id))
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

        // POST: api/Carts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Cart>> PostCart(Cart cart)
        {
            _context.Carts.Add(cart);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (CartExists(cart.CartId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetCart", new { id = cart.CartId }, cart);
        }

        // DELETE: api/Carts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCart(string id)
        {
            var cart = await _context.Carts.FindAsync(id);
            if (cart == null)
            {
                return NotFound();
            }

            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CartExists(string id)
        {
            return _context.Carts.Any(e => e.CartId == id);
        }


        [HttpPost("AddCartItem")]
        public async Task<ActionResult<CartItem>> AddCartItem([FromQuery] string userId, [FromBody] CartItemModel model)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest(new { Message = "User ID is required" });
            }

            var cart = await _context.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                cart = new Cart
                {
                    CartId = Guid.NewGuid().ToString(),
                    UserId = userId,
                    TotalAmount = 0,
                    Items = new List<CartItem>()
                };

                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
            }

            var existingCartItem = cart.Items.FirstOrDefault(ci => ci.ProductId == model.ProductId);

            if (existingCartItem != null)
            {
                existingCartItem.Quantity += model.Quantity;
                cart.TotalAmount += model.PriceAtAddition * model.Quantity;
                existingCartItem.PriceAtAddition = model.PriceAtAddition;
            }
            else
            {
                var newCartItem = new CartItem
                {
                    ProductId = model.ProductId,
                    CartId = cart.CartId,
                    Quantity = model.Quantity,
                    PriceAtAddition = model.PriceAtAddition
                };

                cart.Items.Add(newCartItem);
                cart.TotalAmount += newCartItem.PriceAtAddition * newCartItem.Quantity;
            }
            await _context.SaveChangesAsync();

            return Ok(cart.Items);
        }

        [HttpGet("GetCartItems")]
        public async Task<ActionResult<IEnumerable<object>>> GetCartItems([FromQuery] string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest(new { Message = "User ID is required" });
            }

            var cartItems = await _context.CartItems
                .Include(ci => ci.Product)
                .Where(ci => ci.Cart.UserId == userId)
                .ToListAsync();

            if (cartItems == null || !cartItems.Any())
            {
                return NotFound(new { Message = "No items found in the cart." });
            }

            var result = cartItems.Select(cartItem => new
            {
                cartItem.CartId,
                cartItem.Quantity,
                Product = new
                {
                    cartItem.Product.ProductId,
                    cartItem.Product.Name,
                    cartItem.Product.ImageUrl,
                    cartItem.Product.Price,
                    cartItem.Product.Description,
                    cartItem.Product.QuantityAvailable
                }
            }).ToList();

            return Ok(result);
        }

        [HttpDelete("DeleteCartItem/{productId}")]
        public async Task<ActionResult> DeleteCartItem([FromQuery] string userId, string productId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest(new { Message = "User ID is required" });
            }

            var cartItem = await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.ProductId == productId && ci.Cart.UserId == userId);

            if (cartItem == null)
            {
                return NotFound(new { Message = "Cart item not found." });
            }

            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}