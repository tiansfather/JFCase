using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Abp.Dependency;

namespace Master.Authentication
{
    public interface IClaimsPrincipalCreator:ITransientDependency
    {
        /// <summary>
        /// 建立ClaimsPrincipal
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<ClaimsPrincipal> CreateClaimsPrincipal(User user);
    }
}
