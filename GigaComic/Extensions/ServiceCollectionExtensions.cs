using GigaComic.Configurations;
using GigaComic.Modules.ComicRenderer;
using GigaComic.Modules.GigaChat;
using GigaComic.Services;
using GigaComic.Services.Generation;
using Microsoft.Extensions.Options;

namespace GigaComic.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAppServices(this IServiceCollection serviceCollection,
            IConfiguration configuration)
        {
            serviceCollection.AddScoped<AccountService>();
            serviceCollection.AddScoped<JWTService>();
            serviceCollection.AddScoped<ComicService>();
            serviceCollection.AddScoped<ComicAbstractService>();
            serviceCollection.AddScoped<ComicImageGenerationService>();
            serviceCollection.AddScoped<ComicPageRenderer>();

            serviceCollection.AddAutoMapper(typeof(AppMappingProfile));

            serviceCollection.AddScoped<GigaChatClient>();
            serviceCollection.AddHttpClient<GigaChatClient>(opts =>
            {
                opts.BaseAddress = new Uri(configuration["GigaChat:ModelApi"]!);
            }).ConfigurePrimaryHttpMessageHandler(() =>
            {
                return new HttpClientHandler
                {
                    ClientCertificateOptions = ClientCertificateOption.Manual,
                    ServerCertificateCustomValidationCallback =
                        (httpRequestMessage, cert, certChain, policyErrors) => true
                };
            });

            var handler = new HttpClientHandler();

            handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, errors) => true;

            var client2 = new HttpClient(handler);
            client2.BaseAddress = new Uri(configuration["GigaChat:AuthApi"]!);

            var provider = new AccessTokenProvider(configuration["GigaChat:AuthToken"]!, client2, new DateTimeProvider());

            serviceCollection.AddSingleton(provider);

            return serviceCollection;
        }
    }
}
