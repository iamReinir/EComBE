using EComBusiness.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ECom.Controllers
{
    public class ProductDTO
    {
        public string ProductId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; } // Use decimal for currency
        public int QuantityAvailable { get; set; }        
        public string ImageUrl { get; set; } = string.Empty;
        public decimal Rating { get; set; } = 0;
        public int RatingCount { get; set; } = 0;
        public bool IsWishlisted { get; set; } = false;
        public bool IsRated { get; set; } = false;
        public CategoryDTO Category { get; set; }
    }
}
