namespace OnlineShop.Infrastructure.Data.SeedData
{
    public interface IDatabaseSeeder
    {
        Task SeedAsync();
        Task SeedProductsAsync();
    }
}
