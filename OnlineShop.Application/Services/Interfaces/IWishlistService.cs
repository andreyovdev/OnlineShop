namespace OnlineShop.Application.Services.Interfaces
{
	using ViewModels.Wishlist;

	public interface IWishlistService
	{
		Task<bool> AddProductToWishlist(Guid userId, Guid productId);
		Task<bool> RemoveProductFromWishlist(Guid userId, Guid productId);
		Task<bool> IsProductInWishlist(Guid userId, Guid productId);
		Task<IEnumerable<WishlistProductViewModel>> GetProductsInWishlist(Guid userId);
		Task<int> GetProductsInWishlistCountAsync(Guid userProfileGuid);
	}
}
