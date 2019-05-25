using Master.MultiTenancy;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Master.Authentication
{
    /// <summary>
    /// 登录结果
    /// </summary>
    public class LoginResult
    {
        public LoginResultType Result { get; private set; }

        public Tenant Tenant { get; private set; }

        public User User { get; private set; }

        public ClaimsIdentity Identity { get; private set; }

        public LoginResult(LoginResultType result, Tenant tenant = null, User user = null)
        {
            Result = result;
            Tenant = tenant;
            User = user;
        }

        public LoginResult(Tenant tenant, User user, ClaimsIdentity identity)
            : this(LoginResultType.Success, tenant)
        {
            User = user;
            Identity = identity;
        }
    }
}
