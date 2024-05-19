using System.Text;
using GigaComic.Core.Attributes;
using Microsoft.IdentityModel.Tokens;

namespace GigaComic.Configurations;

public class JWTConfiguration
{
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public string SecretKey { get; set; }
    public int AccessTokenLifeTimeHours { get; set; }
    public int RefreshTokenLifeTimeDays { get; set; }
}