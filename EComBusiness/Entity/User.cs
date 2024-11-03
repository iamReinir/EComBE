using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using EComBusiness.HelperModel;
namespace EComBusiness.Entity
{
    [PrimaryKey(nameof(UserId))]
    public class User : EntityBase
    {
        public string UserId { get; set; } = string.Empty;
        [Required]
        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;
        [Required]
        [MaxLength(255)]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string PasswordHash { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public UserRole Role { get; set; } = UserRole.Customer; // Enum for user role

        // Navigation
        public virtual ICollection<WishlistItem>? WishList { get; set; }
        public virtual ICollection<RatingItem>? RatingList { get; set; }
    }
}
