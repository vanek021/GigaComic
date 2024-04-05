using Microsoft.AspNetCore.Identity;
using IDKEY = System.Int64;

namespace GigaComic.Core.Entities
{
    public class BaseUserRole : IdentityUserRole<IDKEY> { }
    public class BaseUserClaim : IdentityUserClaim<IDKEY> { }
    public class BaseUserLogin : IdentityUserLogin<IDKEY> { }
    public class BaseRoleClaim : IdentityRoleClaim<IDKEY> { }
    public class BaseUserToken : IdentityUserToken<IDKEY> { }
}
