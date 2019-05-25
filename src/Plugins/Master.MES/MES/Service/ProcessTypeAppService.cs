using Abp.AspNetCore.Mvc.Authorization;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.UI;
using Abp.Linq.Extensions;
using Master.MES.Dtos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Master.MES.Service
{
    [AbpAuthorize]
    public class ProcessTypeAppService : MasterAppServiceBase<ProcessType, int>
    {
        public ProcessTaskManager ProcessTaskManager { get; set; }
        public IRepository<EquipmentProcessType,int> EquipmentProcessTypeRepository { get; set; }
        /// <summary>
        /// 返回所有工艺类型
        /// </summary>
        /// <returns></returns>
        public virtual async Task<List<ProcessTypeDto>> GetAll(string key)
        {
            var query = Manager.GetAll();
            if (!key.IsNullOrEmpty())
            {
                query = query.Where(o => o.ProcessTypeName.Contains(key)).OrderBy(o=>o.Sort);
                return (await query.ToListAsync()).MapTo<List<ProcessTypeDto>>();
            }
            else
            {
                var processTypes = (await (Manager as ProcessTypeManager).GetAllList()).OrderBy(o=>o.Sort);
                return processTypes.MapTo<List<ProcessTypeDto>>();
            }
        }
        /// <summary>
        /// 获取所有工艺类型及对应的设备数
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual async Task<object> GetAllWithEquipmentCount(string key)
        {
            var result = new List<object>();

            var processTypes = (await (Manager as ProcessTypeManager).GetAllList()).OrderBy(o=>o.Sort).ToList();
            if (!key.IsNullOrEmpty())
            {
                processTypes = processTypes.Where(o => o.ProcessTypeName.Contains(key)).ToList();
            }
            foreach(var processType in processTypes)
            {
                var equipmentCount =await EquipmentProcessTypeRepository.GetAll().Where(o => o.ProcessTypeId == processType.Id).CountAsync();
                var dto = new
                {
                    processType.Id,
                    processType.ProcessTypeName,
                    EquipmentCount=equipmentCount
                };
                result.Add(dto);
            }

            return result;
        }
        /// <summary>
        /// 获取所有工艺类型及对应的未排机任务数
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual async Task<object> GetAllWithUnArrangeTaskCount(string key)
        {
            var result = new List<object>();
            var processTaskManager = Resolve<ProcessTaskManager>();

            var processTypes = (await (Manager as ProcessTypeManager).GetAllList()).OrderBy(o => o.Sort).ToList();
            if (!key.IsNullOrEmpty())
            {
                processTypes = processTypes.Where(o => o.ProcessTypeName.Contains(key)).ToList();
            }
            foreach (var processType in processTypes)
            {
                var taskCount = await processTaskManager.GetAll().Where(o => o.ProcessTypeId == processType.Id && o.Status.Contains(ProcessTask.Status_Inner) && o.EquipmentId==null).CountAsync();
                var dto = new
                {
                    processType.Id,
                    processType.ProcessTypeName,
                    UnArrangeTaskCount = taskCount
                };
                result.Add(dto);
            }

            return result;
        }

        protected override async Task<IQueryable<ProcessType>> BuildKeywordQueryAsync(string keyword, IQueryable<ProcessType> query)
        {
            return (await base.BuildKeywordQueryAsync(keyword, query))
                .Where(o=>o.ProcessTypeName.Contains(keyword));
        }
        /// <summary>
        /// 获取已经使用的工序类型
        /// </summary>
        /// <returns></returns>
        public virtual async Task<List<ProcessTypeDto>> GetUsedProcessTypes(int? projectId=null)
        {
            var processTypes = await ProcessTaskManager.GetAll()
                .WhereIf(projectId.HasValue,o=>o.Part.ProjectId==projectId)
                .Include(o => o.ProcessType).GroupBy(o => o.ProcessType).Select(o => o.Key).OrderBy(o=>o.Sort).ToListAsync();
            return processTypes.MapTo<List<ProcessTypeDto>>();
        }
        /// <summary>
        /// 添加工艺
        /// </summary>
        /// <param name="processTypeName"></param>
        /// <param name="price"></param>
        /// <returns></returns>
        public virtual async Task<ProcessTypeDto> AddProcessType(string processTypeName)
        {
            //if(await Repository.CountAsync(o => o.ProcessTypeName == processTypeName) > 0)
            //{
            //    throw new UserFriendlyException("此加工工艺已存在");
            //}

            var processType = new ProcessType()
            {
                ProcessTypeName = processTypeName
            };

            await Manager.InsertAsync(processType);
            await CurrentUnitOfWork.SaveChangesAsync();

            return processType.MapTo<ProcessTypeDto>();
        }
        /// <summary>
        /// 修改工序
        /// </summary>
        /// <param name="processTypeDto"></param>
        /// <returns></returns>
        public virtual async Task UpdateProcessType(ProcessTypeDto processTypeDto)
        {
            var processType = await Manager.GetByIdAsync(processTypeDto.Id);
            processTypeDto.MapTo(processType);
            await Manager.UpdateAsync(processType);

        }
        /// <summary>
        /// 批量更新工序设定
        /// </summary>
        /// <param name="processTypeDtos"></param>
        /// <returns></returns>
        public virtual async Task UpdateProcessTypes(List<ProcessTypeDto> processTypeDtos)
        {
            if (processTypeDtos.Select(o => o.ProcessTypeName).Distinct().Count() != processTypeDtos.Count)
            {
                throw new UserFriendlyException(L("工序不能重复,请检查输入"));
            }

            var manager = Manager;
            //先获取原有工序设定
            var oriProcessTypes = await manager.GetAll()
                .ToListAsync();

            //删除
            var deltemp = oriProcessTypes.Where(o => !processTypeDtos.Exists(t => t.Id == o.Id)).ToList();
            foreach (var temp in deltemp)
            {
                await manager.DeleteAsync(temp);
            }
            //编辑
            foreach (var temp in processTypeDtos)
            {
                var t = oriProcessTypes.Where(o => o.Id == temp.Id).SingleOrDefault();
                string type = "0";
                //增加
                if (t == null)
                {
                    type = "1";
                    t = new ProcessType()
                    {

                    };
                }
                temp.MapTo(t);
                //增加
                if (type == "1")
                {
                    await manager.InsertAsync(t);
                }
                else
                {
                    await manager.UpdateAsync(t);
                }
            }
        }
    }
}
