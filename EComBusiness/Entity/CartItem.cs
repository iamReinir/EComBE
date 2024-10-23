using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using EComBusiness.HelperModel;
using Microsoft.EntityFrameworkCore;

namespace EComBusiness.Entity
{
    [PrimaryKey(nameof(ProductId), nameof(CartId))]
    public class CartItem : EntityBase
    {
        public string ProductId { get; set; } = string.Empty;

        public string CartId { get; set; } = string.Empty;
        public int Quantity { get; set; } = 1;
        public decimal PriceAtAddition { get; set; }
        [JsonIgnore]
        public virtual Cart Cart { get; set; }
        [JsonIgnore]
        public virtual Product Product { get; set; }

    }
}
