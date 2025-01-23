using OnlineShop.Application.Mapping;
using OnlineShop.Domain.Entities;

namespace OnlineShop.Application.ViewModels.Profile
{
    public class UserProfileViewModel : IMapFrom<UserProfile>
    {
        public string Id { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
    }
}
