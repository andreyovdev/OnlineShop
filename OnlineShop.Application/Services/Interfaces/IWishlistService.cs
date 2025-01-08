using OnlineShop.Application.ViewModels.Wishlist;

namespace OnlineShop.Application.Services.Interfaces
{
	public interface IWishlistService
	{
		Task<bool> AddProductToWishlist(Guid userId, Guid productId);
		Task<bool> RemoveProductFromWishlist(Guid userId, Guid productId);
		Task<bool> IsProductInWishlist(Guid userId, Guid productId);
		Task<IEnumerable<WishlistProductViewModel>> GetProductsInWishlist(Guid userId);
	}
}
