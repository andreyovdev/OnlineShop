namespace OnlineShop.Application.ViewModels.Wishlist
{
	using AutoMapper;

	using Mapping;
	using Domain.Entities;

	public class WishlistProductViewModel : IMapFrom<Product>, IHaveCustomMappings
	{
		public string Id { get; set; } = null!;

		public string Name { get; set; } = null!;

		public decimal Price { get; set; }

		public string Category { get; set; } = null!;

		public string ImgUrl { get; set; } = null!;

		public void CreateMappings(IProfileExpression profile)
		{
			profile.CreateMap<Product, WishlistProductViewModel>()
				.ForMember(d => d.Category, x => x.MapFrom(s => s.Category.ToString()));
		}
	}
}
