using AutoMapper;
using OnlineShop.Application.Mapping;
using OnlineShop.Domain.Entities;

namespace OnlineShop.Application.ViewModels.Cart
{
	public class CartProductViewModel : IMapFrom<Product>, IHaveCustomMappings
	{
		public string Id { get; set; } = null!;

		public string Name { get; set; } = null!;

		public decimal Price { get; set; }

		public string Category { get; set; } = null!;

		public string ImgUrl { get; set; } = null!;

		public void CreateMappings(IProfileExpression profile)
		{
			profile.CreateMap<Product, CartProductViewModel>()
				.ForMember(d => d.Category, x => x.MapFrom(s => s.Category.ToString()));
		}
	}
}
