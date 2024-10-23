using System.ComponentModel.DataAnnotations;

namespace ECom.ViewModels.CartItem
{
    public class CartItemModel
    {
        [Required]
        public string ProductId { get; set; }
        //[Required]

        //public string CartId { get; set; }
        [Required]

        public int Quantity { get; set; }
        [Required]

        public decimal PriceAtAddition{ get; set; }
    }
}
