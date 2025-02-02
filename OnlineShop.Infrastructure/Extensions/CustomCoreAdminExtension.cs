namespace OnlineShop.Infrastructure.Extensions
{
	using Microsoft.Extensions.DependencyInjection;

	using DotNetEd.CoreAdmin;

	using Infrastructure.Data;
	using Domain.Entities;

	public static class CustomCoreAdminExtension
	{
		public static IServiceCollection AddCustomCoreAdmin(this IServiceCollection services)
		{
			var dbContext = services.BuildServiceProvider().GetRequiredService<ApplicationDbContext>();
			var entityTypes = dbContext.Model.GetEntityTypes();

			var ignoreEntities = new List<Type>();

			foreach (var entityType in entityTypes)
			{
				var clrType = entityType.ClrType;
				ignoreEntities.Add(clrType);
			}

			var keepEntities = new List<Type>
			{
				typeof(Product),
			};

			foreach (var entityType in ignoreEntities.ToList()) 
			{
				if (keepEntities.Contains(entityType))
				{
					ignoreEntities.Remove(entityType);
				}
			}


			services.AddCoreAdmin("Admin");
			services.AddCoreAdmin(new CoreAdminOptions
			{
				IgnoreEntityTypes = ignoreEntities
			});

			return services;
		}
	}
}
