using EComBusiness.HelperModel;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace EComBusiness.Entity
{
    [PrimaryKey(nameof(UserId), nameof(ProductId))]
    public class RatingItem : EntityBase
    {
        public string UserId { get; set; } = string.Empty;
        public string ProductId { get; set; } = string.Empty;
        public int Rating { get; set; } = 0;

        // Navigation
        [ForeignKey(nameof(ProductId))]
        public virtual Product Product { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }
    }
}
