using GigaComic.Services;

namespace GigaComic.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAppServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<AccountService>();
            serviceCollection.AddScoped<JWTService>();
            serviceCollection.AddScoped<ComicService>();

            return serviceCollection;
        }
    }
}
