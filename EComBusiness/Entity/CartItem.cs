using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EComBusiness.HelperModel;
using Microsoft.EntityFrameworkCore;

namespace EComBusiness.Entity
{
    [PrimaryKey(nameof(ProductId),nameof(CartId))]
    public class CartItem : EntityBase
    {
        public string ProductId { get; set; } = string.Empty;
        public string CartId { get; set; } = string.Empty;
        public int Quantity { get; set; } = 1;
        public decimal PriceAtAddition { get; set; }
        public virtual Cart Cart { get; set; }
    }
}
