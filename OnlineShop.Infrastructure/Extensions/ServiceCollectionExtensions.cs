namespace OnlineShop.Infrastructure.Extensions
{
    using System.Reflection;

    using Microsoft.Extensions.DependencyInjection;

    using Domain.Entities;
    using Data.Repository;
    using Data.Repository.Interfaces;
    using System.Linq;

    public static class ServiceCollectionExtensions
    {
        public static void RegisterRepositories(this IServiceCollection services, Assembly modelsAssembly)
        {
            string entitiesNamespace = "OnlineShop.Domain.Entities";

            Type[] modelTypes = modelsAssembly.GetTypes()
                .Where(t =>
                    !t.IsAbstract &&
                    !t.IsInterface &&
                    !t.Name.ToLower().EndsWith("attribute") &&
                    (t.Namespace != null && t.Namespace.StartsWith(entitiesNamespace))
                )
                .ToArray();

            foreach (Type type in modelTypes)
            {
                Type repositoryInterface = typeof(IRepository<>).MakeGenericType(type);
                Type repositoryImplementation = typeof(BaseRepository<>).MakeGenericType(type);

                services.AddScoped(repositoryInterface, repositoryImplementation);
            }

            services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
        }

        public static void RegisterUserDefinedServices(this IServiceCollection services, Assembly serviceAssembly)
        {
            if (serviceAssembly == null)
            {
                throw new InvalidOperationException("Invalid service type provided!");
            }

            Type[] implementationTypes = serviceAssembly
                .GetTypes()
                .Where(t => t.Name.EndsWith("Service") && !t.IsInterface)
                .ToArray();
            foreach (Type implementationType in implementationTypes)
            {
                Type? interfaceType = implementationType
                    .GetInterface($"I{implementationType.Name}");
                if (interfaceType == null)
                {
                    throw new InvalidOperationException(
                        $"No interface is provided for the service with name: {implementationType.Name}");
                }

                services.AddScoped(interfaceType, implementationType);
            }

        }
    }
}
