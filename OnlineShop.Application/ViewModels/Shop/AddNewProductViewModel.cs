
namespace OnlineShop.Application.ViewModels.Shop
{
    using System.ComponentModel.DataAnnotations;
    
    using Domain.Enums;

    using static Domain.Validation.EntityValidationConstants.Product;

    public class AddNewProductViewModel
    {
        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string Name { get; set; } = null!;

        [Required]
        [StringLength(CategoryMaxLength, MinimumLength = CategoryMinLength)]
        public Category Category { get; set; }

        [Required]
        [Url]
        public string ImgUrl { get; set; } = null!;

        [Required]
        [StringLength(DescriptionMaxLength)]
        public string Description { get; set; } = null!;

        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }
    }
}
