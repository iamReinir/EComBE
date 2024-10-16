using EComBusiness.HelperModel;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace EComBusiness.Entity
{
    [PrimaryKey(nameof(ProductId), nameof(OrderId))]
    public class OrderItem : EntityBase
    {
        public string ProductId { get; set; } = string.Empty;
        public string OrderId { get; set; } = string.Empty;
        public int Quantity { get; set; } = 1;
        public decimal PriceAtPurchase { get; set; }


        // Navigation
        [ForeignKey(nameof(OrderId))]
        public virtual Order Order { get; set; }
        [ForeignKey(nameof(ProductId))]
        public virtual Product Product { get; set; }
    }
}
