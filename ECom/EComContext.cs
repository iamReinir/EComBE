using EComBusiness.Entity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace ECom
{
    public class EComContext : IdentityDbContext
    {
        public EComContext(DbContextOptions<EComContext> options)
            : base(options)
        {
        }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<User> AppUsers { get; set; }
        public DbSet<WishlistItem> WishLists { get; set; }

        // Todo: integrate AppUser with Identity's User
    }
}
