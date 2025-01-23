using AutoMapper;
using OnlineShop.Application.Mapping;
using OnlineShop.Domain.Entities;

namespace OnlineShop.Application.ViewModels.Cart
{
	public class CartProductViewModel
	{
		public string ProductId { get; set; } = null!;

		public string UserProfileId { get; set; } = null!;

		public string Name { get; set; } = null!;

		public decimal Price { get; set; }

		public string Category { get; set; } = null!;

		public string ImgUrl { get; set; } = null!;

		public int QuantitySelected { get; set; }
	}
}
