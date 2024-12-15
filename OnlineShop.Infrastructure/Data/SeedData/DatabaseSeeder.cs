namespace OnlineShop.Infrastructure.Data.SeedData
{
    using Newtonsoft.Json;

    using Domain.Entities;

    public class DatabaseSeeder : IDatabaseSeeder
    {
        private readonly ApplicationDbContext _context;
        private const string jsonsPath = "D:\\OnlineShop\\OnlineShop.Infrastructure\\Data\\SeedData";

        public DatabaseSeeder(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            await SeedProductsAsync();
        }

        public async Task SeedProductsAsync()
        {
            if (!_context.Products.Any())
            {
                var jsonPath = Path.Combine(jsonsPath, "products.json");
                var productsJson = File.ReadAllText(jsonPath);
                var products = JsonConvert.DeserializeObject<List<Product>>(productsJson);

                if (products != null)
                {
                    await _context.Products.AddRangeAsync(products);
                    await _context.SaveChangesAsync();
                }
            }
        }
    }

}
