using EComBusiness.HelperModel;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EComBusiness.Entity
{
    [PrimaryKey(nameof(CartId))]
    public class Cart : EntityBase
    {
        public string CartId { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }

        // Navigation
        public virtual ICollection<CartItem> Items { get; set; } = new List<CartItem>();
        [ForeignKey(nameof(UserId))]
        [JsonIgnore]
        public virtual User AppUser { get; set; }
    }
}
