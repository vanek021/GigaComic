using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;

namespace GigaComic.Authorization;

public class AuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly ILogger _logger;
    
    public AuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder) 
        : base(options, logger, encoder)
    {
        _logger = logger.CreateLogger("authHandler");
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var authState = await AuthenticateUser(Context);
        if (authState == null)
            return AuthenticateResult.Fail("Forbidden");
        _logger.Log(LogLevel.Information, "Authenticated user: " + authState.UserId);

        var sidIdentity = new ClaimsIdentity("authId");
        var principal = new ClaimsPrincipal(new List<ClaimsIdentity>(){sidIdentity});
        var ticket = new AuthenticationTicket(principal, Scheme.Name);
        return AuthenticateResult.Success(ticket);
    }

    private async Task<AuthState?> AuthenticateUser(HttpContext context)
    {
        var userAuth = await context.AuthenticateAsync(JwtBearerDefaults.AuthenticationScheme);
        if (userAuth.Succeeded)
        {
            var authState = new AuthState()
            {
                UserId = userAuth.Principal.FindFirst(ClaimTypes.NameIdentifier).Value
            };

            return authState;
        }

        return null;
    }
}