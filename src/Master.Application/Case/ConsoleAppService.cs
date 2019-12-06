using Abp.Authorization;
using Abp.Domain.Repositories;
using Master.Authentication;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.Case
{
    [AbpAuthorize]
    public  class ConsoleAppService:MasterAppServiceBase
    {
        public virtual async Task<object> GetSummary()
        {
            //判例数
            var sourceCount=await Resolve<CaseSourceManager>().GetAll().CountAsync();
            var minerQuery = from user in Resolve<UserManager>().GetAll()
                            join userrole in Resolve<IRepository<UserRole, int>>().GetAll() on user.Id equals userrole.UserId
                            join role in Resolve<RoleManager>().GetAll() on userrole.RoleId equals role.Id
                            where role.Name == StaticRoleNames.Tenants.Miner
                            select user;
            //矿工数
            var minerCount = await minerQuery.CountAsync();
            //初加工数
            var initialCount =await Resolve<CaseInitialManager>().GetAll().Where(o=>o.CaseStatus==CaseStatus.展示中).CountAsync();
            //案例卡数
            var cardCount =await Resolve<CaseCardManager>().GetAll().Where(o => o.CaseStatus == CaseStatus.展示中).CountAsync();
            return new
            {
                sourceCount,
                minerCount,
                initialCount,
                cardCount
            };
        }

        public virtual async Task<object> GetSourceSummary()
        {
            var sourceManager = Resolve<CaseSourceManager>();
            var result = new List<int>();
            for(var i = 0; i < 7; i++)
            {
                var date = DateTime.Now.AddDays(i-6);
                var count=await sourceManager.GetAll().Where(o => o.CreationTime.Year == date.Year && o.CreationTime.Month == date.Month && o.CreationTime.Day == date.Day).CountAsync();
                result.Add(count);
            }
            return result;
        }
    }
}
