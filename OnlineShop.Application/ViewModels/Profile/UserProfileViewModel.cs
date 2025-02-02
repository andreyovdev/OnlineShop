namespace OnlineShop.Application.ViewModels.Profile
{
	using Mapping;
	using Domain.Entities;

	public class UserProfileViewModel : IMapFrom<UserProfile>
    {
        public string Id { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
    }
}
