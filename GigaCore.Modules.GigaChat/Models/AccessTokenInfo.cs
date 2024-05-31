using System.Text.Json.Serialization;

namespace GigaComic.Modules.GigaChat.Models
{
    public record AccessTokenInfo
    {
        public string AccessToken { get; set; }
        public long ExpiresAt { get; set;}
    }
}
