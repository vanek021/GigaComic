using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigaComic.Shared.Responses.Account
{
    public class TokenResponse : IResponse
    {
        public string AccessToken { get; set; }
        public DateTime ExpiringAt { get; set; }
    }
}
