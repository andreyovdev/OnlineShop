using OnlineShop.Application.ViewModels.Cart;
using OnlineShop.Application.ViewModels.Wishlist;

namespace OnlineShop.Application.Services.Interfaces
{
	public interface ICartService
	{
		Task<bool> AddProductToCart(Guid userId, Guid productId);
		Task<bool> RemoveProductFromCart(Guid userId, Guid productId);
		Task<bool> IsProductInCart(Guid userId, Guid productId);
		Task<IEnumerable<CartProductViewModel>> GetProductsInCart(Guid userId);
		Task<int> UpdateCartProductCount(Guid userProfileId, Guid productId, int quantity);
	}
}
