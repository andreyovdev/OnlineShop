using System.Reflection;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using OnlineShop.Application.Mapping;
using OnlineShop.Application.Services;
using OnlineShop.Application.ViewModels.Shop;
using OnlineShop.Domain.Entities;
using OnlineShop.Infrastructure.Data;
using OnlineShop.Infrastructure.Extensions;
using OnlineShop.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddUserSecrets<Program>();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

builder.Services.RegisterRepositories(Assembly.GetAssembly(typeof(Product)));
builder.Services.RegisterUserDefinedServices(Assembly.GetAssembly(typeof(ProductService)));


builder.Services.AddControllers();
builder.Services.AddRazorPages();

var app = builder.Build();

AutoMapperConfig.RegisterMappings(typeof(AddNewProductViewModel).Assembly);

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.ApplyMigrations(); //Custom extension method to update db on application start

app.Run();
