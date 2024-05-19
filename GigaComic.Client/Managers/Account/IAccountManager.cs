using GigaComic.Shared.Requests.Account;
using GigaComic.Shared.Responses;

namespace GigaComic.Client.Managers.Account
{
    public interface IAccountManager : IManager
    {
        public Task<IResult> SignInAsync(SignInRequest request);
        public Task<IResult> RegisterAsync(RegisterRequest request);
        public Task<IResult> SignOutAsync();
    }
}
