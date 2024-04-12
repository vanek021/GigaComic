using System.ComponentModel.DataAnnotations.Schema;
using GigaComic.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using IDKEY = System.Int64;

namespace GigaComic.Core.Entities
{
    [Table("Users")]
    public class BaseUser : IdentityUser<IDKEY>, IEntity
    {
        public virtual ICollection<BaseUserRole> Roles { get; } = new List<BaseUserRole>();
        public virtual ICollection<BaseUserClaim> Claims { get; } = new List<BaseUserClaim>();
        public virtual ICollection<BaseUserLogin> Logins { get; } = new List<BaseUserLogin>();
    }
}
