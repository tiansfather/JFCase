using Abp.Authorization;
using Abp.Extensions;
using Abp.UI;
using Master.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.Case
{
    [AbpAuthorize]
    public class CaseInitialAppService : ModuleDataAppServiceBase<CaseInitial, int>
    {
        protected override string ModuleKey()
        {
            return nameof(CaseInitial);
        }

        protected override async Task<IQueryable<CaseInitial>> BuildOrderQueryAsync(RequestPageDto request, IQueryable<CaseInitial> query)
        {
            //默认按推荐排序
            if (request.OrderField == "Id")
            {
                return query.OrderBy(o => o.Sort).ThenByDescending(o => o.Id);
                //return query.OrderBy(o => o.Sort).ThenByDescending(o => o.Id);
            }
            else
            {
                
            }
            return await base.BuildOrderQueryAsync(request, query);
        }

        /// <summary>
        /// 退回
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public virtual async Task Back(IEnumerable<int> ids)
        {
            var manager = Manager as CaseInitialManager;
            if(await manager.GetAll().CountAsync(o => ids.Contains(o.Id) && o.CaseStatus != CaseStatus.下架) > 0)
            {
                throw new UserFriendlyException("只有下架的案例可以退回");
            }
            var caseInitials = await Manager.GetAll().Include(o => o.CaseSource)
                .Where(o => ids.Contains(o.Id)).ToListAsync();
            foreach(var caseInitial in caseInitials)
            {
                caseInitial.CaseStatus = CaseStatus.退回;
                caseInitial.CaseSource.CaseSourceStatus = CaseSourceStatus.加工中;
            }

        }

        /// <summary>
        /// 下架
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public virtual async Task Down(IEnumerable<int> ids)
        {
            var caseInitials = await Manager.GetAll().Include(o => o.CaseSource)
                .Where(o => ids.Contains(o.Id)).ToListAsync();
            foreach (var caseInitial in caseInitials)
            {
                caseInitial.CaseStatus = CaseStatus.下架;
            }

        }
        /// <summary>
        /// 上架
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public virtual async Task Up(IEnumerable<int> ids)
        {
            var manager = Manager as CaseInitialManager;
            if (await manager.GetAll().CountAsync(o => ids.Contains(o.Id) && o.CaseStatus != CaseStatus.下架 && o.PublishDate!=null) > 0)
            {
                throw new UserFriendlyException("只有下架中的已发布案例可以上架");
            }
            var caseInitials = await Manager.GetAll().Include(o => o.CaseSource)
                .Where(o => ids.Contains(o.Id)).ToListAsync();
            foreach (var caseInitial in caseInitials)
            {
                caseInitial.CaseStatus = CaseStatus.展示中;
            }

        }
        /// <summary>
        /// 推荐
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public virtual async Task Recommand(IEnumerable<int> ids)
        {
            var caseInitials = await Manager.GetAll().Include(o => o.CaseSource)
                .Where(o => ids.Contains(o.Id)).ToListAsync();
            foreach (var caseInitial in caseInitials)
            {
                caseInitial.IsActive = true;
            }
        }
        /// <summary>
        /// 取消推荐
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public virtual async Task UnRecommand(IEnumerable<int> ids)
        {
            var caseInitials = await Manager.GetAll().Include(o => o.CaseSource)
                .Where(o => ids.Contains(o.Id)).ToListAsync();
            foreach (var caseInitial in caseInitials)
            {
                caseInitial.IsActive = false;
            }
        }
        public virtual async Task SetSort(int id,string sortStr)
        {
            int sort = 999999;
            if(int.TryParse(sortStr, out sort))
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
        /// <summary>
        /// 管理方清空判例的加工内容
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public virtual async Task ClearContent(int[] ids)
        {
            var caseSourceIds = await Manager.GetAll().Where(o => ids.Contains(o.Id)).Select(o => o.CaseSourceId).ToListAsync();
            foreach (var id in caseSourceIds)
            {
                await Resolve<CaseSourceManager>().ClearCaseContent(id, true);
            }
        }
    }
}
