using System.Text;
using GigaComic.Core.Attributes;
using Microsoft.IdentityModel.Tokens;

namespace GigaComic.Configurations;

public class AuthConfiguration
{
    public readonly string Issuer = "MyAuthServer";
    public readonly string Audience = "MyAuthClient";

    public readonly string SessionScheme = "SessionScheme";

    public AuthConfiguration(string secretKey)
    {
        _key = secretKey;
    }
    
    public SymmetricSecurityKey GetSymmetricSecurityKey() => 
        new (Encoding.UTF8.GetBytes(_key));
    
    private readonly string _key;
}