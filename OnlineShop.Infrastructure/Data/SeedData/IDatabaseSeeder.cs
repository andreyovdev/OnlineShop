namespace OnlineShop.Infrastructure.Data.SeedData
{
	public interface IDatabaseSeeder
    {
        Task SeedAsync();
        Task SeedProductsAsync();
		Task SeedUsersAsync();
		Task SeedRolesAsync();
		Task SeedUserRolesAsync();
		Task SeedUserProfiles();
		Task SeedAddressesAsync();
	}
}
