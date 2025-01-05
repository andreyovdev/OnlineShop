namespace OnlineShop.Application.Services.Interfaces
{
	public interface IUserProfileService
	{
		Task AddProductToWishlist(Guid userId, Guid productId);
	}
}
