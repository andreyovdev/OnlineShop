namespace OnlineShop.Application.Services
{
	using Interfaces;
	using Domain.Entities;
	using Infrastructure.Data.Repository.Interfaces;
	using Microsoft.EntityFrameworkCore;

	public class UserProfileService : BaseService, IUserProfileService
	{
		private readonly IRepository<UserProfile> userProfileRepository;

		public UserProfileService(IRepository<UserProfile> userProfileRepository)
		{
			this.userProfileRepository = userProfileRepository;
		}

		public async Task<bool> CreateUserProfile(string name, Guid userId)
		{
			var userProfile = new UserProfile
			{
				Name = name,
				AppUserId = userId,
				Wishlist = new List<Wishlist>(),
				Cart = new List<Cart>(),
				Purchases = new List<Purchase>()
			};

			if (userProfile == null)
			{
				return false;
			}

			userProfileRepository.Add(userProfile);
			await userProfileRepository.SaveAsync();

			return true;
		}

		public async Task<Guid> GetUserProfileId(Guid userId)
		{
			var userProfile = await userProfileRepository
				.GetAllAttached()
				.FirstOrDefaultAsync(up => up.AppUserId == userId);

			return userProfile.Id;
		}
	}
}
