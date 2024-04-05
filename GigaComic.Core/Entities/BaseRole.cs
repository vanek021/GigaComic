using GigaComic.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using IDKEY = System.Int64;

namespace GigaComic.Core.Entities
{
    [Table("Roles")]
    public class BaseRole : IdentityRole<IDKEY>, IEntity
    {
        public BaseRole() { }

        public BaseRole(string roleName)
        {
            Name = roleName;
        }
    }
}
