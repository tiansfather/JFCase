using Abp.Authorization;
using Abp.AutoMapper;
using Abp.BackgroundJobs;
using Master.Authentication;
using Master.Case;
using Master.Core.Jobs;
using Master.Domain;
using Master.Dto;
using Master.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.Users
{
    public class NewMinerAppService : ModuleDataAppServiceBase<NewMiner, int>
    {
        protected override string ModuleKey()
        {
            return nameof(NewMiner);
        }
        protected override async Task<IQueryable<NewMiner>> BuildKeywordQueryAsync(string keyword, IQueryable<NewMiner> query)
        {
            return (await base.BuildKeywordQueryAsync(keyword, query))
                .Where(o=>o.Name.Contains(keyword));
        }
        public virtual async Task<object> GetSummary()
        {
            var manager = Manager as NewMinerManager;
            var normalCount = await manager.GetAll().IgnoreQueryFilters().Where(o => !o.IsDeleted).CountAsync();
            var rejectCount = await manager.GetAll().IgnoreQueryFilters().Where(o => o.IsDeleted && !o.Verified).CountAsync();

            return new
            {
                normalCount,
                rejectCount,
            };
        }
        protected override async Task<IQueryable<NewMiner>> GetQueryable(RequestPageDto request)
        {
            var query=await base.GetQueryable(request);
            //显示删除的
            return query.IgnoreQueryFilters();
        }
        public virtual async Task Register(NewMinerRegisterDto newMinerRegisterDto)
        {
            var newMiner = newMinerRegisterDto.MapTo<NewMiner>();
            await Manager.InsertAsync(newMiner);
        }
        [AbpAuthorize]
        public virtual async Task Verify(IEnumerable<int> ids)
        {
            var manager = Manager as NewMinerManager;
            var userManger = Resolve<UserManager>();
            var roleManager = Resolve<RoleManager>();

            var role = await roleManager.FindByNameAsync(StaticRoleNames.Tenants.Miner);

            var newMiners = await manager.GetListByIdsAsync(ids);
            foreach(var newMiner in newMiners)
            {
                var user = newMiner.MapTo<User>();
                user.Id = 0;
                user.SetPropertyValue("NickName", newMiner.NickName);
                user.SetPropertyValue("Avata", newMiner.Avata);
                await userManger.InsertAndGetIdAsync(user);
                await userManger.SetRoles(user, new int[] { role.Id });
                user.Logins = new List<UserLogin>() {
                new UserLogin(AbpSession.TenantId,user.Id,"Wechat",newMiner.OpenId)
            };
                newMiner.Verified = true;
                await CurrentUnitOfWork.SaveChangesAsync();
                await manager.DeleteAsync(newMiner);

                //发送邮件
                var emailLog = new EmailLog()
                {
                    Title = "您的账号已经审核通过",
                    Content = $"您可以通过微信登录简法案例系统",
                    ToEmail = user.Email
                };
                var emailLogId = await Resolve<EmailLogManager>().InsertAndGetIdAsync(emailLog);
                var jobArg = new SendEmailJobArgs() { EmailLogId = emailLogId };
                await Resolve<IBackgroundJobManager>().EnqueueAsync<SendEmailJob, SendEmailJobArgs>(jobArg);
            }
            
        }
    }
}
