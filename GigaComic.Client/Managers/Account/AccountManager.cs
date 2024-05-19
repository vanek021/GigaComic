using Blazored.LocalStorage;
using GigaComic.Client.Auth;
using GigaComic.Shared.Constants;
using GigaComic.Shared.Extensions;
using GigaComic.Shared.Requests.Account;
using GigaComic.Shared.Responses;
using GigaComic.Shared.Responses.Account;
using GigaComic.Shared.Routes;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace GigaComic.Client.Managers.Account
{
    public class AccountManager : IAccountManager
    {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly ILocalStorageService _localStorageService;

        public AccountManager(HttpClient httpClient, AuthenticationStateProvider authenticationStateProvider,
            ILocalStorageService localStorageService)
        {
            _httpClient = httpClient;
            _authenticationStateProvider = authenticationStateProvider;
            _localStorageService = localStorageService;
        }

        public async Task<IResult> SignInAsync(SignInRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync(AccountEndpoints.SignIn, request);
            var result = await response.ToResultAsync<TokenResponse>();

            if (result.Succeeded)
            {
                await ProcessSignIn(result);
                return Result.Success();
            }

            return Result.Fail(result.Messages);
        }

        public async Task<IResult> RegisterAsync(RegisterRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync(AccountEndpoints.Register, request);
            var result = await response.ToResultAsync<TokenResponse>();

            if (result.Succeeded)
            {
                await ProcessSignIn(result);
                return Result.Success();
            }

            return Result.Fail(result.Messages);
        }

        private async Task ProcessSignIn(IResult<TokenResponse> result)
        {
            var token = result.Data.AccessToken;
            await _localStorageService.SetItemAsync(StorageConstants.AccessToken, token);

            await ((GigaComicAuthStateProvider)this._authenticationStateProvider).StateChangedAsync();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public async Task<IResult> SignOutAsync()
        {
            await _localStorageService.ClearAsync();
            await ((GigaComicAuthStateProvider)this._authenticationStateProvider).StateChangedAsync();
            return Result.Success();
        }
    }
}
