using OnlineShop.Application.Services.Interfaces;
using OnlineShop.Application.ViewModels.Shop;
using OnlineShop.Domain.Entities;
using OnlineShop.Infrastructure.Data.Repository.Interfaces;

namespace OnlineShop.Application.Services
{
	public class UserProfileService : BaseService, IUserProfileService
	{
		private IRepository<UserProfile> userProfileRepository;

		public UserProfileService(IRepository<UserProfile> userProfileRepository)
		{
			this.userProfileRepository = userProfileRepository;
		}

		public async Task AddProductToWishlist(Guid userId, Guid productId)
		{
			//return await this.userProfileRepository
			//  .GetAllAttached()
			//  .Where(u => u.Wishlist == false)
			//  .ToArrayAsync();
		}
	}
}
