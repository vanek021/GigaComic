using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using GigaComic.Configurations;
using GigaComic.Core.Entities;
using GigaComic.Models.Entities;
using GigaComic.Models.Requests;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace GigaComic.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly AuthConfiguration _authConfiguration;

    public AuthController(UserManager<User> userManager, SignInManager<User> signInManager, AuthConfiguration authConfiguration)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _authConfiguration = authConfiguration;
    }

    [Route("register")]
    [HttpPost]
    public async Task<IActionResult> Register(RegisterRequest registerRequest, CancellationToken cancellationToken)
    {
        // Создаём пользователя
        // var user = await _userService.Create(registerRequest, cancellationToken);

        // var claims = await GetUserClaims(user);
        // var token = GetToken(claims);
        //
        // return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token), expiration = token.ValidTo });
        throw new NotImplementedException();
    }

    [Route("login")]
    [HttpPost]
    public async Task<IActionResult> Login(LoginRequest loginRequest)
    {
        var user = await _userManager.FindByNameAsync(loginRequest.Login);
        if (user == null)
            return BadRequest("Wrong credentials");
        if (!await _userManager.CheckPasswordAsync(user, loginRequest.Password))
            return BadRequest("Wrong credentials");

        var claims = await GetUserClaims(user);
        var token = GetToken(claims);
        return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token), expiration = token.ValidTo });
    }

    [Route("logout")]
    [HttpGet]
    public async Task Logout()
    {
        await _signInManager.SignOutAsync();
    }

    [Route("whoami")]
    [HttpGet]
    public async Task WhoAmI()
    {
        var whoami = HttpContext.User.Identity?.Name ?? "Anonymous";
        await HttpContext.Response.WriteAsync(whoami);
    }

    private JwtSecurityToken GetToken(IEnumerable<Claim> authClaims)
    {
        var authSigningKey = _authConfiguration.GetSymmetricSecurityKey();

        var token = new JwtSecurityToken(
            _authConfiguration.Issuer,
            _authConfiguration.Audience,
            expires: DateTime.Now.AddHours(12),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );

        return token;
    }

    private async Task<List<Claim>> GetUserClaims(User user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.UserName),
            new(ClaimTypes.NameIdentifier, user.Id.ToString())
        };

        return claims.Concat(await _userManager.GetClaimsAsync(user)).ToList();
    }
}