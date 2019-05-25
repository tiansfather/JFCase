using Abp.Dependency;
using Abp.Domain.Repositories;
using Master.Authentication;
using Master.Authentication.External;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Master.Authentication
{
    public static class UserExtension
    {
        /// <summary>
        /// 获取用户的微信OpenId
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static string GetWechatOpenId(this User user)
        {
            using(var repositoryWrapper = IocManager.Instance.ResolveAsDisposable<IRepository<UserLogin, int>>())
            {
                return repositoryWrapper.Object.GetAll()
                    .IgnoreQueryFilters()
                    .Where(o=>o.TenantId==user.TenantId)
                    .Where(o => o.UserId == user.Id)
                    .Where(o => o.LoginProvider == WeChatAuthProviderApi.Name)
                    .FirstOrDefault()?.ProviderKey;
            }
        }
    }
}
