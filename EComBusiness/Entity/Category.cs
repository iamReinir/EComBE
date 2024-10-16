using EComBusiness.HelperModel;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EComBusiness.Entity
{
    [PrimaryKey(nameof(CategoryId))]
    public class Category : EntityBase
    {
        public string CategoryId { get; set; } = string.Empty;
        [Required]
        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        

        // Navigation
        public string? ParentCategoryId { get; set; }
        [ForeignKey(nameof(ParentCategoryId))]
        public virtual Category? ParentCategory { get; set; }
    }

}
