namespace OnlineShop.Infrastructure.Extensions
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Configuration;

    using Infrastructure.Data;
    using Infrastructure.Identity;

    public static class ExtensionMethods
    {
        public static IApplicationBuilder ApplyMigrations(this IApplicationBuilder app)
        {
            using IServiceScope serviceScope = app.ApplicationServices.CreateScope();

            ApplicationDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            dbContext.Database.Migrate();

            return app;
        }

        public static IServiceCollection AddConfiguredIdentity(this IServiceCollection services, IConfiguration config)
        {
            services
                 .AddIdentity<AppUser, IdentityRole<Guid>>(options =>
                 {
                     options.Password.RequireDigit = config.GetValue<bool>("Identity:Password:RequireDigits");
                     options.Password.RequireLowercase = config.GetValue<bool>("Identity:Password:RequireLowercase");
                     options.Password.RequireUppercase = config.GetValue<bool>("Identity:Password:RequireUppercase");
                     options.Password.RequireNonAlphanumeric = config.GetValue<bool>("Identity:Password:RequireNonAlphanumerical");
                     options.Password.RequiredLength = config.GetValue<int>("Identity:Password:RequiredLength");
                     options.Password.RequiredUniqueChars = config.GetValue<int>("Identity:Password:RequiredUniqueCharacters");

                     options.SignIn.RequireConfirmedAccount = config.GetValue<bool>("Identity:SignIn:RequireConfirmedAccount");
                     options.SignIn.RequireConfirmedEmail = config.GetValue<bool>("Identity:SignIn:RequireConfirmedEmail");
                     options.SignIn.RequireConfirmedPhoneNumber = config.GetValue<bool>("Identity:SignIn:RequireConfirmedPhoneNumber");

                     options.User.RequireUniqueEmail = config.GetValue<bool>("Identity:User:RequireUniqueEmail");
                 })
                 .AddEntityFrameworkStores<ApplicationDbContext>()
                 .AddRoles<IdentityRole<Guid>>()
                 .AddSignInManager<SignInManager<AppUser>>()
                 .AddRoleManager<RoleManager<IdentityRole<Guid>>>();
            //TODO: .AddDefaultTokeProviders()

            return services;
        }
    }
}
