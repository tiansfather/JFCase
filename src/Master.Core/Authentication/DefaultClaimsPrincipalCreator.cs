using Abp.Runtime.Security;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Master.Authentication
{
    public class DefaultClaimsPrincipalCreator : IClaimsPrincipalCreator
    {
        public UserManager UserManager { get; set; }
        public async virtual Task<ClaimsPrincipal> CreateClaimsPrincipal(User user)
        {
            var identity = new ClaimsIdentity(new Claim[] {
                new Claim(AbpClaimTypes.UserId,user.Id.ToString()),
                new Claim("Name",user.Name)
            }, "Bearer");
            if (user.TenantId.HasValue)
            {
                identity.AddClaim(new Claim(AbpClaimTypes.TenantId, user.TenantId.ToString()));
            }
            //将用户是否独立加入到claim中
            //identity.AddClaim(new Claim("UserSeparate", user.IsSeparate ? "true" : ""));
            var roles = await UserManager.GetRolesAsync(user);
            identity.AddClaim(new Claim(AbpClaimTypes.Role, roles.Count > 0 ? roles[0].Name : ""));
            var principal = new ClaimsPrincipal(identity);
            return principal;
        }
    }
}
