using Abp.Domain.Repositories;
using Master.Domain;
using Master.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Runtime.Caching;
using Abp.Events.Bus.Handlers;
using Abp.Events.Bus.Entities;

namespace Master.Case
{
    public class LabelManager:DomainServiceBase<Label,int>,IEventHandler<EntityChangedEventData<TreeLabel>>
    {
        /// <summary>
        /// 获取标签对应的树节点集合
        /// </summary>
        /// <param name="labelId"></param>
        /// <returns></returns>
        public virtual async Task<List<BaseTree>> GetRelativeTreeNodes(int labelId)
        {
            return await Resolve<IRepository<TreeLabel, int>>().GetAll().Include(o => o.BaseTree)
                .Where(o => o.LabelId == labelId)
                .Select(o=>o.BaseTree)
                .ToListAsync();
        }

        public override async Task<List<Label>> GetAllList()
        {
            var tenantId = CurrentUnitOfWork.GetTenantId();

            var key = typeof(Label).FullName + "@" + (tenantId ?? 0);
            var result = await CacheManager.GetCache<string, List<Label>>(typeof(Label).FullName)
                .GetAsync(key, async () => { return await Repository.GetAll().Include(o=>o.TreeLabels).ToListAsync(); });
            //if(result==null || result.Count == 0)
            //{
            //    await CacheManager.GetCache<string, List<TEntity>>(typeof(TEntity).FullName).RemoveAsync(key);
            //    result= await GetAll().ToListAsync();
            //}

            return result;
        }

        public void HandleEvent(EntityChangedEventData<TreeLabel> eventData)
        {
            var key = typeof(Label).FullName + "@" + AbpSession.TenantId;
            CacheManager.GetCache<string, List<Label>>(typeof(Label).FullName).Remove(key);
        }
    }
}
