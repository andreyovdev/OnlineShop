namespace OnlineShop.Application.Services.Interfaces
{
	public interface IUserProfileService
	{
		Task<bool> CreateUserProfile(string name, Guid userId);
		Task<Guid> GetUserProfileId(Guid userId);
	}
}
