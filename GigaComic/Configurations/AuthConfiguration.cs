using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace GigaComic.Configurations;

public class AuthConfiguration
{
    public static string ISSUER = "MyAuthServer";
    public static string AUDIENCE = "MyAuthClient";

    public static string SessionScheme = "SessionScheme";
    
    public static SymmetricSecurityKey GetSymmetricSecurityKey() => 
        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
    
    private const string KEY = "mysupersecret_secretsecretsecretkey!123";
}