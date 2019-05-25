using Abp.Dependency;
using Abp.Runtime.Security;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Master.Authentication
{
    public static class AbpSessionExtension
    {

        public static bool IsSeparateUser(this IAbpSession abpSession)
        {
            return GetClaimValue(abpSession,"UserSeparate") == "true";
        }
        
        /// <summary>
        /// 登录用户的名称
        /// </summary>
        /// <param name="abpSession"></param>
        /// <returns></returns>
        public static string Name(this IAbpSession abpSession)
        {
            return GetClaimValue(abpSession,"Name");
        }
        public static string GetRoleName(this IAbpSession abpSession)
        {
            var userid = abpSession.UserId;
            return GetClaimValue(abpSession,AbpClaimTypes.Role);
        }


        public static IEnumerable<Claim> GetClaims()
        {
            using (var principalAccessor = IocManager.Instance.ResolveAsDisposable<IPrincipalAccessor>())
            {
                var claimsPrincipal = principalAccessor.Object.Principal;
                var claims = claimsPrincipal?.Claims;

                return claims;
            }
        }

        public static string GetClaimValue(this IAbpSession abpSession,string claimType)
        {
            var claims = GetClaims();

            var claim = claims?.FirstOrDefault(c => c.Type == claimType);
            if (string.IsNullOrEmpty(claim?.Value))
                return null;

            return claim.Value;
        }

    }
}
