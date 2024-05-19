using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Headers;
using GigaComic.Shared.Constants;

namespace GigaComic.Client.MessageHandlers
{
    public class AuthenticationMessageHandler : DelegatingHandler
    {
        private readonly ILocalStorageService _storageService;
        private readonly NavigationManager _navigationManager;

        public AuthenticationMessageHandler(ILocalStorageService localStorageService, NavigationManager navigationManager)
        {
            _storageService = localStorageService;
            _navigationManager = navigationManager;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.Headers.Authorization?.Scheme != "Bearer")
            {
                var savedToken = await _storageService.GetItemAsync<string>(StorageConstants.AccessToken);

                if (!string.IsNullOrWhiteSpace(savedToken))
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", savedToken);
            }

            var response = await base.SendAsync(request, cancellationToken);

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                _navigationManager.NavigateTo("/account/signin");

            return response;
        }
    }
}
