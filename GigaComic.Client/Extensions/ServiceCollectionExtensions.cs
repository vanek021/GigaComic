using GigaComic.Client.Managers;

namespace GigaComic.Client.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddManagers(this IServiceCollection serviceCollection)
        {
            var managers = typeof(IManager);

            var types = managers
                .Assembly
                .GetExportedTypes()
                .Where(t => t.IsClass && !t.IsAbstract)
                .Select(t => new
                {
                    Service = t.GetInterface($"I{t.Name}"),
                    Implementation = t
                })
                .Where(t => t.Service != null);

            foreach (var type in types)
                if (managers.IsAssignableFrom(type.Service))
                    serviceCollection.AddScoped(type.Service, type.Implementation);

            return serviceCollection;
        }
    }
}
