using Abp.BackgroundJobs;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.UI;
using Master.Authentication;
using Master.Authentication.External;
using Master.Configuration;
using Master.Domain;
using Master.Entity;
using Master.MES.Jobs;
using Master.MultiTenancy;
using Master.Notices;
using Master.Units;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.MES
{
    public class MESUnitManager:UnitManager
    {
        //public TenantManager TenantManager { get; set; }
        //public UserManager UserManager
        //{
        //    get; set;
        //}
        //public MESTenantManager MESTenantManager { get; set; }
        //public RemindLogManager RemindLogManager { get; set; }
        public IBackgroundJobManager BackgroundJobManager { get; set; }
        public IRepository<User,long> UserRepository { get; set; }
        /// <summary>
        /// 根据加工点名称寻找对应的接收者微信openid
        /// </summary>
        /// <param name="unitName"></param>
        /// <returns></returns>
        public virtual async Task<List<string>> FindUnitOpenId(string unitName,string status="")
        {
            var unit = await GetAll().Where(o => o.UnitName == unitName).FirstOrDefaultAsync();

            return await FindUnitOpenId(unit, status);
        }
        /// <summary>
        /// 通过加工点获取接收者微信openid
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        public virtual async Task<List<string>> FindUnitOpenId(Unit unit, string status="")
        {
            var tenantManager = Resolve<MESTenantManager>();
            var userManager = Resolve<UserManager>();
            if (unit == null)
            {
                throw new UserFriendlyException($"未找到往来单位");
            }
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                Tenant tenant = null;
                //与此单位绑定的账套id
                var unitTenantId = unit.GetPropertyValue<int?>("TenantId");
                if (unitTenantId.HasValue)
                {
                    tenant = await tenantManager.GetByIdAsync(unitTenantId.Value);
                }
                //else
                //{
                //    //如果此加工点是未绑定账套的，则通过名称去获取账套
                //    tenant = await tenantManager.FindByTenancyNameAsync(unit.UnitName);
                //}

                if (tenant == null)
                {
                    throw new UserFriendlyException(-1,$"请先邀请{unit.UnitName}加入模来模往");
                }
                if (!tenant.IsActive)
                {
                    throw new UserFriendlyException($"账套{unit.UnitName}未被激活");
                }
                var openids = new List<string>();
                if (!string.IsNullOrEmpty(status))
                {
                    //获取账套内设置了对应用户标记且绑定了微信的用户的openid
                    var users = await userManager.GetAll().Include(o => o.Logins).Where(o => o.TenantId == tenant.Id && o.Status != null && o.Status.Contains(status)).ToListAsync();
                    openids = users.Where(o=>o.Logins.Count(l => l.LoginProvider == WeChatAuthProviderApi.Name) > 0).Select(o => o.Logins.First(l => l.LoginProvider == WeChatAuthProviderApi.Name).ProviderKey).ToList();
                }
                else
                {
                    //默认获取管理用户
                    //获取账套的管理用户
                    var adminUser = await tenantManager.GetTenantAdminUser(tenant);
                    if (adminUser == null)
                    {
                        //管理用户是通过注册手机号找的
                        throw new UserFriendlyException($"未能找到账套{unit.UnitName}的管理用户");
                    }
                    //获取用户的登录微信openid信息
                    await UserRepository.EnsureCollectionLoadedAsync(adminUser, o => o.Logins);
                    var wechatLogin = adminUser.Logins.FirstOrDefault(o => o.LoginProvider == WeChatAuthProviderApi.Name);
                    if (wechatLogin == null)
                    {
                        throw new UserFriendlyException($"账套{unit.UnitName}的管理用户尚未绑定微信");
                    }
                    openids.Add(wechatLogin.ProviderKey);
                }


                if (openids.Count == 0)
                {
                    throw new UserFriendlyException($"账套{unit.UnitName}中并未设置对应的接收用户");
                }

                
                return openids;
            }
        }

        /// <summary>
        /// 发送往来单位公告
        /// </summary>
        /// <param name="units"></param>
        /// <param name="notice"></param>
        /// <returns></returns>
        public virtual async Task SendUnitsNotice(IEnumerable<Unit> units,Notice notice)
        {
            var remindLogManager = Resolve<RemindLogManager>();
            foreach (var unit in units)
            {
                //被提醒人微信openid
                var openId = "";
                try
                {
                    openId = (await FindUnitOpenId(unit))[0];
                }
                catch (Exception ex)
                {

                }
                if (!string.IsNullOrEmpty(openId))
                {
                    //进行发送提醒
                    //先产生一条提醒记录
                    var remindLog = new RemindLog()
                    {
                        RemindType = "往来单位公告提醒",
                        Name = unit.UnitName,
                        Message=notice.NoticeTitle,
                        TenantId = AbpSession.TenantId
                    };
                    var remindLogId = await remindLogManager.InsertAndGetIdAsync(remindLog);
                    var arg = new SendWeiXinMessageJobArgs()
                    {
                        OpenId = openId,
                        DataId = notice.Id,
                        RemindLogId = remindLogId
                    };

                    BackgroundJobManager.Enqueue<SendUnitNoticeMessageJob, SendWeiXinMessageJobArgs>(arg);
                }
            }
        }
    }
}
