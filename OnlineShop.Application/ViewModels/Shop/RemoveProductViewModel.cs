namespace OnlineShop.Application.ViewModels.Shop
{
    using AutoMapper;

    using Mapping;
    using Domain.Entities;

    public class RemoveProductViewModel : IHaveCustomMappings
    {
        public string Id { get; set; } = null!;

        public string Name { get; set; } = null!;

        public void CreateMappings(IProfileExpression profile)
        {

            profile
                .CreateMap<Product, RemoveProductViewModel>();

            profile
                .CreateMap<RemoveProductViewModel, Product>()
                    .ForMember(d => d.Id, x => x.MapFrom(s => Guid.Parse(s.Id)));
        }
    }
}
