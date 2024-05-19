using Blazored.LocalStorage;
using GigaComic.Client;
using GigaComic.Client.Auth;
using GigaComic.Client.Extensions;
using GigaComic.Client.MessageHandlers;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Globalization;

const string apiClientName = "GigaComic.API";

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var apiUrl = builder.Configuration["ApiUrl"]!;

builder.Services
    .AddAuthorizationCore()
    .AddBlazoredLocalStorage()
    .AddCascadingAuthenticationState()
    .AddAntDesign()
    .AddScoped<GigaComicAuthStateProvider>()
    .AddScoped<AuthenticationStateProvider, GigaComicAuthStateProvider>();

builder.Services
    .AddManagers();

// Add http client features.
builder.Services
    .AddScoped<AuthenticationMessageHandler>()
    .AddScoped(sp => sp
        .GetRequiredService<IHttpClientFactory>()
        .CreateClient(apiClientName))
    .AddHttpClient(apiClientName, client =>
    {
        client.DefaultRequestHeaders.AcceptLanguage.Clear();
        client.DefaultRequestHeaders.AcceptLanguage.ParseAdd(CultureInfo.DefaultThreadCurrentCulture?.TwoLetterISOLanguageName);
        client.BaseAddress = new Uri(apiUrl);
    })
    .AddHttpMessageHandler(sp => sp.GetRequiredService<AuthenticationMessageHandler>());

await builder.Build().RunAsync();
