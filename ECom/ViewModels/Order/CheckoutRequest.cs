using EComBusiness.Entity;

namespace ECom.ViewModels.Order
{
    public class CheckoutRequest
    {
        public string Address { get; set; } = string.Empty;
        public PaymentMethod PaymentMethod { get; set; }
    }
}
