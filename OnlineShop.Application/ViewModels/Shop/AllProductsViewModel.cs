namespace OnlineShop.Application.ViewModels.Shop
{
    using AutoMapper;
    using Application.Mapping;
    using Domain.Entities;

    public class AllProductsViewModel : IMapFrom<Product>, IHaveCustomMappings
    {
        public string Id { get; set; } = null!;

        public string Name { get; set; } = null!;

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public string Category { get; set; } = null!;

        public string ImgUrl { get; set; } = null!;

        public bool IsDeleted { get; set; }

        //public string SearchProductInput { get; set; } = null!;

        public void CreateMappings(IProfileExpression profile)
        {
            profile.CreateMap<Product, AllProductsViewModel > ()
                .ForMember(d => d.Category, x => x.MapFrom(s => s.Category.ToString()));
        }
    }
}
