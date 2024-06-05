using GigaComic.Core.Data;
using GigaComic.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using IDKEY = System.Int64;

namespace GigaComic.Core.Internal
{
    internal class BaseUserStore<TContext, TUser, TRole>
            : UserStore<TUser, TRole, TContext, IDKEY, BaseUserClaim, BaseUserRole, BaseUserLogin, BaseUserToken, BaseRoleClaim>
            where TContext : BaseDbContext<TUser, TRole>
            where TUser : BaseUser
            where TRole : BaseRole
    {
        public BaseUserStore(TContext context)
            : base(context)
        {
        }

        protected override BaseUserClaim CreateUserClaim(TUser user, Claim claim)
        {
            return new BaseUserClaim()
            {
                UserId = user.Id,
                ClaimType = claim.Type,
                ClaimValue = claim.Value
            };
        }

        protected override BaseUserLogin CreateUserLogin(TUser user, UserLoginInfo login)
        {
            return new BaseUserLogin()
            {
                UserId = user.Id,
                LoginProvider = login.LoginProvider,
                ProviderDisplayName = login.ProviderDisplayName,
                ProviderKey = login.ProviderKey
            };
        }

        protected override BaseUserRole CreateUserRole(TUser user, TRole role)
        {
            return new BaseUserRole()
            {
                UserId = user.Id,
                RoleId = role.Id,
            };
        }

        protected override BaseUserToken CreateUserToken(TUser user, string loginProvider, string name, string? value)
        {
            return new BaseUserToken()
            {
                UserId = user.Id,
                LoginProvider = loginProvider,
                Name = name,
                Value = value
            };
        }
    }

    internal class BaseRoleStore<TContext, TUser, TRole>
        : RoleStore<TRole, TContext, IDKEY, BaseUserRole, BaseRoleClaim>
        where TContext : BaseDbContext<TUser, TRole>
        where TUser : BaseUser
        where TRole : BaseRole
    {
        public BaseRoleStore(TContext context)
            : base(context)
        {
        }

        protected override BaseRoleClaim CreateRoleClaim(TRole role, Claim claim)
        {
            return new BaseRoleClaim()
            {
                RoleId = role.Id,
                ClaimType = claim.Type,
                ClaimValue = claim.Value
            };
        }
    }
}
