namespace OnlineShop.Application.ViewModels.Shop
{
    using System.ComponentModel.DataAnnotations;

    using AutoMapper;

    using Domain.Entities;
    using Mapping;

    using static Validation.ViewModelValidationMessages;
    using static Domain.Common.EntityValidationConstants.Product;

    public class AddNewProductViewModel :IMapTo<Product>, IHaveCustomMappings
    {
        [Required(ErrorMessage = RequiredMessage)]
        [Length(1, NameMaxLength, ErrorMessage = InvalidStringLengthMessage)]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = RequiredMessage)]
        public string Category { get; set; } = null!;

        [Required(ErrorMessage = RequiredMessage)]
        //Add regex validation when images are stored on web
        //[RegularExpression(ValidUrlPattern, ErrorMessage = InvalidUrlMessage)]
        public string ImgUrl { get; set; } = null!;

        [Required(ErrorMessage = RequiredMessage)]
        [Range(0, int.MaxValue, ErrorMessage = InvalidNumberMessage)]
        public decimal Price { get; set; }

        [Required(ErrorMessage = RequiredMessage)]
        [Length(0, DescriptionMaxLength, ErrorMessage = InvalidStringLengthMessage)]
        public string Description { get; set; } = null!;

        [Required(ErrorMessage = RequiredMessage)]
        [Range(0, int.MaxValue, ErrorMessage = InvalidNumberMessage)]
        public int Quantity { get; set; }

        public void CreateMappings(IProfileExpression profile)
        {
            profile.CreateMap<AddNewProductViewModel, Product>()
           .ForMember(d => d.Category, x => x.Ignore());
        }
    }
}
