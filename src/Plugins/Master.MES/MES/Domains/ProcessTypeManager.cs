using Master.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Runtime.Caching;
using Abp.Events.Bus.Handlers;
using Abp.Events.Bus.Entities;
using Abp.UI;

namespace Master.MES
{
    public class ProcessTypeManager : DomainServiceBase<ProcessType, int>
    {
        //public override async Task ValidateEntity(ProcessType entity)
        //{
        //    //不允许相同零件名称存在
        //    if (entity.Id > 0 && await Repository.CountAsync(o =>  o.ProcessTypeName == entity.ProcessTypeName && o.Id != entity.Id) > 0)
        //    {
        //        throw new UserFriendlyException(L("相同工艺名称已存在"));
        //    }
        //    if (entity.Id == 0 && await Repository.CountAsync(o => o.ProcessTypeName == entity.ProcessTypeName) > 0)
        //    {
        //        throw new UserFriendlyException(L("相同工艺名称已存在"));
        //    }
        //    await base.ValidateEntity(entity);
        //}
        /// <summary>
        /// 通过缓存获取所有工序信息
        /// </summary>
        /// <returns></returns>
        //public virtual async Task<List<ProcessType>> GetAllList()
        //{
        //    var tenantId = CurrentUnitOfWork.GetTenantId();
        //    var key = "ProcessType" + "@" + (tenantId ?? 0);
        //    return await CacheManager.GetCache<string, List<ProcessType>>("ProcessType")
        //        .GetAsync(key, async () => { return await GetAll().ToListAsync(); });
        //}
        //public async override Task ValidateEntity(ProcessType entity)
        //{            

        //    //不允许相同工序名称存在
        //    if (entity.Id > 0 && await Repository.CountAsync(o => o.ProcessTypeName == entity.ProcessTypeName && o.Id != entity.Id) > 0)
        //    {
        //        throw new UserFriendlyException(L("相同工艺名称已存在"));
        //    }
        //    if (entity.Id == 0 && await Repository.CountAsync(o => o.ProcessTypeName == entity.ProcessTypeName) > 0)
        //    {
        //        throw new UserFriendlyException(L("相同工艺名称已存在"));
        //    }
        //    await base.ValidateEntity(entity);
        //}
        /// <summary>
        /// 通过工艺名称获取工艺实体，若不存在，则新增
        /// </summary>
        /// <param name="supplierName"></param>
        /// <returns></returns>
        public virtual async Task<ProcessType> GetByNameOrInsert(string processTypeName)
        {
            var processType = await Repository.GetAll().Where(o => o.ProcessTypeName == processTypeName).FirstOrDefaultAsync();
            if (processType == null)
            {
                processType = new ProcessType()
                {
                    ProcessTypeName = processTypeName
                };
                await Repository.InsertAsync(processType);
                await CurrentUnitOfWork.SaveChangesAsync();
            }
            return processType;
        }

        //public void HandleEvent(EntityChangedEventData<ProcessType> eventData)
        //{
        //    var key = "ProcessType" + "@" + eventData.Entity.TenantId;
        //    CacheManager.GetCache<string, List<ProcessType>>("ProcessType").Remove(key);
        //}
    }
}
