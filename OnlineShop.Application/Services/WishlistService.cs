
using Microsoft.EntityFrameworkCore;

using OnlineShop.Application.Mapping;
using OnlineShop.Application.Services.Interfaces;
using OnlineShop.Application.ViewModels.Wishlist;
using OnlineShop.Domain.Entities;
using OnlineShop.Infrastructure.Data.Repository.Interfaces;

namespace OnlineShop.Application.Services
{
	public class WishlistService: BaseService, IWishlistService
	{
		private readonly IRepository<Wishlist> wishlistRepository;
		private readonly IRepository<Product> productRepository;

		public WishlistService(IRepository<Wishlist> wishlistRepository, IRepository<Product> productRepository)
		{
			this.wishlistRepository = wishlistRepository;
			this.productRepository = productRepository;
		}

		public async Task<bool> AddProductToWishlist(Guid userProfileId, Guid productId)
		{

			var existingWishlistItem = await wishlistRepository
				.GetAllAttached()
				.FirstOrDefaultAsync(w => w.UserProfileId == userProfileId && w.ProductId == productId);

			if (existingWishlistItem != null)
			{
				return false;
			}

			var wishlistItem = new Wishlist { UserProfileId = userProfileId, ProductId = productId };
			wishlistRepository.Add(wishlistItem);
			await wishlistRepository.SaveAsync();

			return true;
		}

		public async Task<bool> RemoveProductFromWishlist(Guid userProfileId, Guid productId)
		{
			var existingWishlistItem = await wishlistRepository
			.GetAllAttached()
				.FirstOrDefaultAsync(w => w.UserProfileId == userProfileId && w.ProductId == productId);

			if (existingWishlistItem == null)
			{
				return false;
			}

			wishlistRepository.RemoveByEntity(existingWishlistItem);
			await wishlistRepository.SaveAsync();

			return true;
		}

		public async Task<bool> IsProductInWishlist(Guid userProfileId, Guid productId)
		{
			return await wishlistRepository
				.GetAllAttached()
				.AnyAsync(w => w.UserProfileId == userProfileId && w.ProductId == productId);
		}

		public async Task<IEnumerable<WishlistProductViewModel>> GetProductsInWishlist(Guid userProfileId)
		{

			var wishlistedProductIds = await wishlistRepository
				.GetAllAttached()
				.Where(w => w.UserProfileId == userProfileId)
				.Select(w => w.ProductId)
				.ToListAsync();

			if (!wishlistedProductIds.Any())
			{
				return Enumerable.Empty<WishlistProductViewModel>();
			}

			var products = await productRepository
				.GetAllAttached()
				.Where(p => wishlistedProductIds.Contains(p.Id))
				.To<WishlistProductViewModel>()
				.ToArrayAsync();

			return products;
		}
	}
}
