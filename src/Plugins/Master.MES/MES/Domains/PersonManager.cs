using Master.Authentication;
using Master.Authentication.External;
using Master.Domain;
using Master.Entity;
using Master.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Senparc.Weixin.MP.AdvancedAPIs.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.MES
{
    /// <summary>
    /// 附属人员管理
    /// </summary>
    public class PersonManager:DomainServiceBase<Person, int>
    {
        //public UserManager UserManager { get; set; }
        /// <summary>
        /// 通过公众号信息获取人员实体，若不存在则新增
        /// </summary>
        /// <param name="oAuthUserInfo"></param>
        /// <returns></returns>
        public virtual async Task<Person> GetPersonByWeUserOrInsert(OAuthUserInfo oAuthUserInfo)
        {
            //寻找对应的用户信息
            var user = await Resolve<UserManager>().FindAsync(new Microsoft.AspNetCore.Identity.UserLoginInfo(WeChatAuthProviderApi.Name, oAuthUserInfo.openid, ""));
            //先判断是否已有此人员
            var reporter = await Repository.GetAll().Where(o => MESDbContext.GetJsonValueString(o.Property, "$.OpenId") == oAuthUserInfo.openid && o.PersonSource == PersonSource.MLMW).FirstOrDefaultAsync();
            if (reporter == null)
            {
                
                reporter = new Person()
                {
                    Name=oAuthUserInfo.nickname
                };
                
                await Repository.InsertAsync(reporter);
                await CurrentUnitOfWork.SaveChangesAsync();
            }
            //如果已经有用户了,则取用户的名称
            if (user != null)
            {
                reporter.Name = user.Name;
            }
            reporter.SetPropertyValue("OpenId", oAuthUserInfo.openid);
            reporter.SetPropertyValue("HeadImgUrl", oAuthUserInfo.headimgurl);
            reporter.SetPropertyValue("NickName", oAuthUserInfo.nickname);

            await Repository.UpdateAsync(reporter);
            await CurrentUnitOfWork.SaveChangesAsync();

            return reporter;
        }

        /// <summary>
        /// 通过来源获取第一个人员,若不存在则新
        /// </summary>
        /// <param name="personSource"></param>
        /// <returns></returns>
        public virtual async Task<Person> GetDefaultPersonBySourceOrInsert(PersonSource personSource)
        {
            //先判断是否已有此人员
            var reporter = await Repository.GetAll().Where(o =>  o.PersonSource == personSource).FirstOrDefaultAsync();
            if (reporter == null)
            {
                reporter = new Person()
                {
                    Name = personSource.ToString(),
                    PersonSource=personSource
                };
                await Repository.InsertAsync(reporter);
                await CurrentUnitOfWork.SaveChangesAsync();
            }

            await Repository.UpdateAsync(reporter);
            await CurrentUnitOfWork.SaveChangesAsync();

            return reporter;
        }

        /// <summary>
        /// 通过微信openid获取person信息
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        public virtual async Task<Person> FindByWeOpenId(string openId)
        {
            return await GetAll().Where(o => MESDbContext.GetJsonValueString(o.Property, "$.OpenId") == openId && o.PersonSource == PersonSource.MLMW)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// 通过用户姓名获取提醒person
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual async Task<Person> FindByName(string name)
        {
            var user = await Resolve<UserManager>().GetAll().Include(o => o.Logins).Where(o => o.Name == name && o.IsActive).FirstOrDefaultAsync();
            
            if (user != null)
            {
                var openId = user.Logins.Where(o => o.LoginProvider == WeChatAuthProviderApi.Name).SingleOrDefault()?.ProviderKey;
                if (!string.IsNullOrEmpty(openId))
                {
                    return await FindByWeOpenId(openId);
                }
            }

            return null;
        }
    }
}
