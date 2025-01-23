using System.Reflection;

using Microsoft.EntityFrameworkCore;

using OnlineShop.Application.Mapping;
using OnlineShop.Application.Services;
using OnlineShop.Application.ViewModels.Shop;
using OnlineShop.Domain.Entities;
using OnlineShop.Infrastructure.Data;
using OnlineShop.Infrastructure.Data.SeedData;
using OnlineShop.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddUserSecrets<Program>();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddScoped<DatabaseSeeder>();

builder.Services.AddConfiguredIdentity(builder.Configuration);

builder.Services.ConfigureApplicationCookie(cfg =>
{
    cfg.LoginPath = "/Identity/Account/Login";
	cfg.ExpireTimeSpan = TimeSpan.FromMinutes(60); 
	cfg.SlidingExpiration = false;
	cfg.Cookie.HttpOnly = true;
	cfg.Cookie.SecurePolicy = CookieSecurePolicy.Always; 
	cfg.Cookie.SameSite = SameSiteMode.Lax;
	cfg.Cookie.IsEssential = true;

});

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
	options.IdleTimeout = TimeSpan.FromMinutes(60); 
	options.Cookie.HttpOnly = true; 
	options.Cookie.IsEssential = true;
});

builder.Services.AddControllersWithViews();
builder.Services.RegisterRepositories(Assembly.GetAssembly(typeof(Product)));
builder.Services.RegisterUserDefinedServices(Assembly.GetAssembly(typeof(ProductService)));
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
    	options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });
builder.Services.AddRazorPages();

var app = builder.Build();

app.UseSession();

AutoMapperConfig.RegisterMappings(typeof(AddNewProductViewModel).Assembly);

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();

    //Reset and migrate database
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        dbContext.Database.EnsureDeleted();
        await dbContext.Database.MigrateAsync();
    }
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

// Apply any pending migrations
app.ApplyMigrations();

//Seed database
using (var scope = app.Services.CreateScope())
{
    var databaseSeeder = scope.ServiceProvider.GetRequiredService<DatabaseSeeder>();
	await databaseSeeder.SeedAsync();
}

app.Run();