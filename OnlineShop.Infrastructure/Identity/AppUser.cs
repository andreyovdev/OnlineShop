namespace OnlineShop.Infrastructure.Identity
{
    using Microsoft.AspNetCore.Identity;
	using OnlineShop.Domain.Entities;

	public class AppUser : IdentityUser<Guid>
    {
        public AppUser()
        {
            this.Id = Guid.NewGuid();
        }
	}
}
