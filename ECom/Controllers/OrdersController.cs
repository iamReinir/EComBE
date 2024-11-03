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
using ECom.ViewModels.Order;

namespace ECom.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly EComContext _context;

        public OrdersController(EComContext context)
        {
            _context = context;
        }

        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            return await _context.Orders.ToListAsync();
        }

        // GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(string id)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }

        // PUT: api/Orders/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(string id, Order order)
        {
            if (id != order.OrderId)
            {
                return BadRequest();
            }

            _context.Entry(order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
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

        // POST: api/Orders
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder(Order order)
        {
            _context.Orders.Add(order);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (OrderExists(order.OrderId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetOrder", new { id = order.OrderId }, order);
        }

        // DELETE: api/Orders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(string id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        [HttpGet("GetOrders")]
        public async Task<ActionResult<IEnumerable<object>>> GetOrders([FromQuery] string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest(new { Message = "User ID is required" });
            }

            var orders = await _context.Set<Order>()
                .Where(o => o.UserId == userId)
                .Select(o => new
                {
                    o.OrderId,
                    o.TotalAmount,
                    o.Status,
                    o.ShippingAddress,
                    PaymentMethod = o.PaymentMethod.ToString() 
                })
                .ToListAsync();

            if (!orders.Any())
            {
                return NotFound(new { Message = "No orders found" });
            }

            return Ok(orders);
        }

        [HttpPost("checkout")]
        public async Task<IActionResult> Checkout(string userId, [FromBody] CheckoutRequest request)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest(new { Message = "User ID is required" });
            }
            var cart = await _context.Carts
                                     .Include(c => c.Items)
                                     .FirstOrDefaultAsync(c => c.UserId == userId);
            if (cart == null || !cart.Items.Any())
            {
                return BadRequest("Cart is empty or does not exist.");
            }

            var order = new Order
            {   OrderId=Guid.NewGuid().ToString(),
                UserId = userId,
                TotalAmount = cart.TotalAmount,
                ShippingAddress = request.Address,
                Status = OrderStatus.Pending,
                PaymentMethod = request.PaymentMethod
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            foreach (var cartItem in cart.Items)
            {
                var orderItem = new OrderItem
                {
                    ProductId = cartItem.ProductId,
                    OrderId = order.OrderId,
                    Quantity = cartItem.Quantity,
                    PriceAtPurchase = cartItem.PriceAtAddition 
                };

                _context.OrderItems.Add(orderItem);
            }

            await _context.SaveChangesAsync();
            _context.CartItems.RemoveRange(cart.Items);
            cart.TotalAmount = 0;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Order created successfully", orderId = order.OrderId });
        }



        private bool OrderExists(string id)
        {
            return _context.Orders.Any(e => e.OrderId == id);
        }
    }
}
