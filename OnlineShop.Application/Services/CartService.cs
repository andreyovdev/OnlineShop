
using Microsoft.EntityFrameworkCore;

using OnlineShop.Application.Mapping;
using OnlineShop.Application.Services.Interfaces;
using OnlineShop.Application.ViewModels.Cart;
using OnlineShop.Application.ViewModels.Wishlist;
using OnlineShop.Domain.Entities;
using OnlineShop.Infrastructure.Data.Repository.Interfaces;

namespace OnlineShop.Application.Services
{
	public class CartService : BaseService, ICartService
	{
		private readonly IRepository<Cart> cartRepository;
		private readonly IRepository<Product> productRepository;

		public CartService(IRepository<Product> productRepository, IRepository<Cart> cartRepository)
		{
			this.productRepository = productRepository;
			this.cartRepository = cartRepository;
		}

		public async Task<bool> AddProductToCart(Guid userProfileId, Guid productId)
		{

			var existingCartItem = await cartRepository
				.GetAllAttached()
				.FirstOrDefaultAsync(w => w.UserProfileId == userProfileId && w.ProductId == productId);

			if (existingCartItem != null)
			{
				return false;
			}

			var selectedProduct = await productRepository
			.GetAllAttached()
			.FirstOrDefaultAsync(p => p.Id == productId);


			if (selectedProduct == null)
			{
				return false;
			}

			if (selectedProduct.Quantity == 0)
			{
				return false;
			}

			var cartItem = new Cart { 
				UserProfileId = userProfileId, 
				ProductId = productId,
				Quantity = 1,
			};

			cartRepository.Add(cartItem);
			await cartRepository.SaveAsync();

			return true;
		}

		public async Task<bool> RemoveProductFromCart(Guid userProfileId, Guid productId)
		{
			var existingCartItem = await cartRepository
			.GetAllAttached()
				.FirstOrDefaultAsync(w => w.UserProfileId == userProfileId && w.ProductId == productId);

			if (existingCartItem == null)
			{
				return false;
			}

			cartRepository.RemoveByEntity(existingCartItem);
			await cartRepository.SaveAsync();

			return true;
		}

		public async Task<bool> IsProductInCart(Guid userProfileId, Guid productId)
		{
			return await cartRepository
				.GetAllAttached()
				.AnyAsync(w => w.UserProfileId == userProfileId && w.ProductId == productId);
		}

		public async Task<IEnumerable<CartProductViewModel>> GetProductsInCart(Guid userProfileId)
		{
			var inCartProductIds = await cartRepository
				.GetAllAttached()
				.Where(w => w.UserProfileId == userProfileId)
				.Select(w => w.ProductId)
				.ToListAsync();

			if (!inCartProductIds.Any())
			{
				return Enumerable.Empty<CartProductViewModel>();
			}

			var products = await productRepository
				.GetAllAttached()
				.Where(p => inCartProductIds.Contains(p.Id))
				.To<CartProductViewModel>()
				.ToArrayAsync();

			return products;
		}
	}
}
