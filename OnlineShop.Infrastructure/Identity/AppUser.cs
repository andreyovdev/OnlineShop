namespace OnlineShop.Infrastructure.Identity
{
    using Microsoft.AspNetCore.Identity;

	public class AppUser : IdentityUser<Guid>
    {
        public AppUser()
        {
            this.Id = Guid.NewGuid();
            this.UserName = Guid.NewGuid().ToString();
        }

        public string FullName { get; set; }
    }
}
