namespace OnlineShop.Application.Services
{
	using Interfaces;
	using Domain.Entities;
	using Infrastructure.Data.Repository.Interfaces;
	using Microsoft.EntityFrameworkCore;
    using OnlineShop.Application.ViewModels.Profile;
    using OnlineShop.Application.Mapping;

    public class UserProfileService : BaseService, IUserProfileService
	{
		private readonly IRepository<UserProfile> userProfileRepository;

		public UserProfileService(IRepository<UserProfile> userProfileRepository)
		{
			this.userProfileRepository = userProfileRepository;
		}

		public async Task<bool> CreateUserProfile(string fullName, string email, Guid userId)
		{
			var userProfile = new UserProfile
			{
                FullName = fullName,
				Email = email,
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

        public async Task<UserProfileViewModel> GetUserProfileById(Guid userProfileId)
        {
            return await this.userProfileRepository
            .GetAllAttached()
            .To<UserProfileViewModel>()
            .FirstOrDefaultAsync(up => up.Id == userProfileId.ToString());
        }

        public async Task<Guid> GetUserProfileId(Guid userId)
		{
			var userProfile = await userProfileRepository
				.GetAllAttached()
				.FirstOrDefaultAsync(up => up.AppUserId == userId);

			if(userProfile == null)
			{
				return Guid.Empty;
			}

			return userProfile.Id;
		}
	}
}
