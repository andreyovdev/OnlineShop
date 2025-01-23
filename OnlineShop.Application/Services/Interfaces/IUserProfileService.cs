using OnlineShop.Application.ViewModels.Profile;

namespace OnlineShop.Application.Services.Interfaces
{
	public interface IUserProfileService
	{
		Task<bool> CreateUserProfile(string name, string email, Guid userId);
		Task<Guid> GetUserProfileId(Guid userId);
		Task<UserProfileViewModel> GetUserProfileById(Guid userProfileId);
    }
}
