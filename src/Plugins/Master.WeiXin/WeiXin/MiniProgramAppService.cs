using Abp.Domain.Repositories;
using Abp.Runtime.Security;
using Abp.UI;
using Master.Authentication;
using Master.Authentication.External;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.WeiXin
{
    public class MiniProgramAppService : MasterAppServiceBase
    {
        public IRepository<UserLogin, int> UserLoginRepository { get; set; }
        public IRepository<User, long> UserRepository { get; set; }

        /// <summary>
        /// 绑定小程序至账号
        /// </summary>
        /// <param name="encrpytedUserId"></param>
        /// <returns></returns>
        public virtual async Task BindLogin(string encryptedUserId,string openId)
        {

            if (!int.TryParse(SimpleStringCipher.Instance.Decrypt(encryptedUserId, AppConsts.DefaultPassPhrase), out var userid))
            {
                throw new UserFriendlyException("未找到对应用户");
            }
            var user = await UserRepository.GetAll().IgnoreQueryFilters().Where(o => o.Id == userid).SingleAsync();

            await UserLoginRepository.DeleteAsync(o => o.UserId == userid && o.LoginProvider == MiniProgramAuthProviderApi.Name);
            var userlogin = new UserLogin()
            {
                UserId = userid,
                LoginProvider = MiniProgramAuthProviderApi.Name,
                TenantId = user.TenantId,
                ProviderKey = openId
            };
            await UserLoginRepository.InsertAsync(userlogin);
        }

        public virtual async Task<object> GetUserInfo(string openId)
        {
            var query=from userLogin in UserLoginRepository.GetAll().IgnoreQueryFilters()
                join user in UserRepository.GetAll().IgnoreQueryFilters() on userLogin.UserId equals user.Id
                where userLogin.LoginProvider == "MiniProgram" && userLogin.ProviderKey == openId
                select user;

            var userInfo = await query.Include(o=>o.Tenant).SingleOrDefaultAsync();
            if (userInfo == null)
            {
                return null;
            }
            else
            {
                return new { userInfo.Name,userInfo.Tenant?.TenancyName};
            }

        }
    }
}
