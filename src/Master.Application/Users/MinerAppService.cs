using Abp.Authorization;
using Abp.UI;
using Master.Authentication;
using Master.Case;
using Master.Domain;
using Master.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.Users
{
    [AbpAuthorize]
    public class MinerAppService : ModuleDataAppServiceBase<User, long>
    {
        protected override string ModuleKey()
        {
            return nameof(Miner);
        }
        protected override async Task<IQueryable<User>> GetQueryable(RequestPageDto request)
        {
            return (await base.GetQueryable(request)).IgnoreQueryFilters();
        }
        protected override async Task<IQueryable<User>> BuildKeywordQueryAsync(string keyword, IQueryable<User> query)
        {
            return (await base.BuildKeywordQueryAsync(keyword, query))
                .Where(o => o.Name.Contains(keyword));
        }
        protected override async Task<IQueryable<User>> BuildOrderQueryAsync(RequestPageDto request, IQueryable<User> query)
        {
            if (request.OrderField == "Id")
            {
                return query.OrderBy(o => o.Sort).ThenByDescending(o => o.Id);
                //return query.OrderBy(o => o.Sort).ThenByDescending(o => o.Id);
            }
            if (request.OrderField == "caseNumber")
            {
                if (request.OrderType == "asc")
                {
                    return query.OrderBy(o => Resolve<CaseSourceManager>().GetAll().Count(c => c.OwerId == o.Id && c.CaseSourceStatus == CaseSourceStatus.已加工));
                }
                else
                {
                    return query.OrderByDescending(o => Resolve<CaseSourceManager>().GetAll().Count(c => c.OwerId == o.Id && c.CaseSourceStatus == CaseSourceStatus.已加工));
                }
                
            }
            return await base.BuildOrderQueryAsync(request, query);
        }
        public virtual async Task<object> GetSummary()
        {
            var manager = Manager as UserManager;
            var query = manager.GetFilteredQuery(ModuleKey());
            var normalCount =await query.Where(o => o.IsActive && !o.IsDeleted).CountAsync();
            var freezeCount = await query.Where(o => !o.IsActive && !o.IsDeleted).CountAsync();
            var deleteCount= await query.Where(o => o.IsDeleted).CountAsync();

            return new
            {
                normalCount,
                freezeCount,
                deleteCount
            };
        }
        /// <summary>
        /// 清空某矿工所有成品
        /// </summary>
        /// <param name="userIds"></param>
        /// <returns></returns>
        public virtual async Task ClearUserContent(int[] userIds)
        {
            foreach(var userId in userIds)
            {
                await Resolve<CaseSourceManager>().ClearCaseContentByUserId(userId);
            }
            
        }

        public virtual async Task SetSort(int id, string sortStr)
        {
            int sort = 999999;
            if (int.TryParse(sortStr, out sort))
            {
                if (sort <= 0)
                {
                    throw new UserFriendlyException("排序值必须大于0");
                }
            }
            else
            {
                sort = 999999;
            }
            var caseInitial = await Manager.GetByIdAsync(id);
            caseInitial.Sort = sort;
        }
    }
}
