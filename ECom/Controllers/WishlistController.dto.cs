using EComBusiness.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECom.Controllers
{
    public class WishlistDTO
    {
        public string UserId { get; set; } = string.Empty;
        public string ProductId { get; set; } = string.Empty;
        public Product Product { get; set; }
    }

    public class WishlistAddRequest
    {
        public string UserId { get; set; } = string.Empty;
        public string ProductId { get; set; } = string.Empty;
    }
}
