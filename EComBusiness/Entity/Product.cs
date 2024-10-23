using EComBusiness.HelperModel;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EComBusiness.Entity
{
    [PrimaryKey(nameof(ProductId))]
    public class Product : EntityBase
    {
        public string ProductId { get; set; } = string.Empty;
        [Required]
        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; } // Use decimal for currency
        public int QuantityAvailable { get; set; }        
        public string CategoryId { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public decimal Rating { get; set; } = 0;
        public int RatingCount { get; set;} = 0;

        // Navigation
        [ForeignKey(nameof(CategoryId))]
        public virtual Category Category { get; set; }
    }

}
