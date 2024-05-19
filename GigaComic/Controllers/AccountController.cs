using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using GigaComic.Configurations;
using GigaComic.Core.Entities;
using GigaComic.Models.Entities;
using GigaComic.Services;
using GigaComic.Shared.Requests.Account;
using GigaComic.Shared.Responses.Account;
using GigaComic.Shared.Routes;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace GigaComic.Controllers;

public class AccountController : BaseApiController
{
    private readonly AccountService _accountService;

    public AccountController(AccountService accountService)
    {
        _accountService = accountService;
    }

    [Route(AccountEndpoints.Register)]
    [HttpPost]
    public async Task<IActionResult> Register([FromBody] RegisterRequest model)
    {
        if (!ModelState.IsValid)
            return BlazorBadRequest(GetModelErrors());

        var createResult = await _accountService.CreateUser(model);

        if (!createResult.Success)
            return BlazorBadRequest(createResult.Errors);

        return await ProcessSignIn(model.Username, model.Password);
    }

    [Route(AccountEndpoints.SignIn)]
    [HttpPost]
    public async Task<IActionResult> SignIn([FromBody] SignInRequest model)
    {
        if (!ModelState.IsValid)
            return BlazorBadRequest(GetModelErrors());

        return await ProcessSignIn(model.Username, model.Password);
    }

    private async Task<IActionResult> ProcessSignIn(string username, string password)
    {
        var user = await _accountService.GetUserBy(username, password);

        if (user == null)
            return BlazorBadRequest("Неверное имя пользователя или пароль");

        var tokenResponse = _accountService.GenerateAccessTokenFor(user);

        return BlazorOk(tokenResponse);
    }
}