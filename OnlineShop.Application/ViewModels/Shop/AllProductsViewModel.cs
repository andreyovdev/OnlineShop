namespace OnlineShop.Application.ViewModels.Shop
{
    using AutoMapper;

    using Application.Mapping;
    using Domain.Entities;

    public class AllProductsViewModel : IMapFrom<Product>, IHaveCustomMappings
    {
        public string Name { get; set; } = null!;

        public string Category { get; set; } = null!;

        public string ImgUrl { get; set; } = null!;

        public bool InStock { get; set; } = false;

        public void CreateMappings(IProfileExpression profile)
        {
            profile.CreateMap<Product, AllProductsViewModel > ()
                .ForMember(d => d.Category, x => x.MapFrom(s => s.Category.ToString()));
        }
    }
}
