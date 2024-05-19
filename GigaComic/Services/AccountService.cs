using GigaComic.Models.Entities;
using GigaComic.Shared.Requests.Account;
using GigaComic.Shared.Responses.Account;
using Microsoft.AspNetCore.Identity;

namespace GigaComic.Services
{
    public class AccountService
    {
        private readonly UserManager<User> _userManager;
        private readonly JWTService _jwtService;

        public AccountService(UserManager<User> userManager, JWTService jwtService)
        {
            _userManager = userManager;
            _jwtService = jwtService;
        }

        public async Task<User?> GetUserBy(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user == null)
                return null;

            var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, password);

            if (!isPasswordCorrect)
                return null;

            return user;
        }

        public async Task<(bool Success, List<string> Errors)> CreateUser(RegisterRequest registerRequest)
        {
            var user = new User()
            {
                UserName = registerRequest.Username,
                Email = registerRequest.Email,
                EmailConfirmed = true,
            };

            var result = await _userManager.CreateAsync(user, registerRequest.Password);

            if (result.Succeeded)
                return (true, new List<string>());

            return (false, result.Errors.Select(x => x.Description).ToList());
        }

        public TokenResponse GenerateAccessTokenFor(User user)
        {
            var accessToken = _jwtService.GenerateAccessToken(user);

            return new TokenResponse()
            {
                AccessToken = accessToken.AccessToken,
                ExpiringAt = accessToken.TokenOptions.ValidTo
            };
        }
    }
}
