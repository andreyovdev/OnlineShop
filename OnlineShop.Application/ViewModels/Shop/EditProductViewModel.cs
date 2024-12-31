namespace OnlineShop.Application.ViewModels.Shop
{
    using System.ComponentModel.DataAnnotations;

    using AutoMapper;

    using Domain.Entities;
    using Mapping;

    using static Validation.ViewModelValidationMessages;
    using static Domain.Common.EntityValidationConstants.Product;

    public class EditProductViewModel : IHaveCustomMappings
    {
        [Required]
        public string Id { get; set; } = null!;

        [Required(ErrorMessage = RequiredMessage)]
        [Length(1, NameMaxLength, ErrorMessage = InvalidStringLengthMessage)]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = RequiredMessage)]
        public string Category { get; set; } = null!;

        [Required(ErrorMessage = RequiredMessage)]
        public string ImgUrl { get; set; } = null!;

        [Required(ErrorMessage = RequiredMessage)]
        [Range(0, double.MaxValue, ErrorMessage = InvalidNumberMessage)]
        public decimal Price { get; set; }

        [Required(ErrorMessage = RequiredMessage)]
        [Length(0, DescriptionMaxLength, ErrorMessage = InvalidStringLengthMessage)]
        public string Description { get; set; } = null!;

        [Required(ErrorMessage = RequiredMessage)]
        [Range(0, int.MaxValue, ErrorMessage = InvalidNumberMessage)]
        public int Quantity { get; set; }

        public void CreateMappings(IProfileExpression profile)
        {
            profile
                .CreateMap<Product, EditProductViewModel>();

            profile
                .CreateMap<EditProductViewModel, Product>()
                    .ForMember(d => d.Category, x => x.Ignore())
                    .ForMember(d => d.Id, x => x.MapFrom(s => Guid.Parse(s.Id)));
        }
    }
}
