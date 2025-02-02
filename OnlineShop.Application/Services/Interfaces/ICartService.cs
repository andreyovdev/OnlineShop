namespace OnlineShop.Application.Services.Interfaces
{
	using ViewModels.Cart;

	public interface ICartService
	{
		Task<bool> AddProductToCart(Guid userId, Guid productId);
		Task<bool> RemoveProductFromCart(Guid userId, Guid productId);
		Task<bool> IsProductInCart(Guid userId, Guid productId);
		Task<IEnumerable<CartProductViewModel>> GetProductsInCart(Guid userId);
		Task<int> GetProductsInCartCountAsync(Guid userProfileGuid);
		Task<int> UpdateCartProductCount(Guid userProfileId, Guid productId, int quantity);
	}
}
