using Abp.UI;
using Master.Configuration;
using Master.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.Case
{
    public class WorkbenchAppService : MasterAppServiceBase<CaseSource, int>
    {
        #region 分页
        protected override async Task<IQueryable<CaseSource>> GetQueryable(RequestPageDto request)
        {
            var query = await base.GetQueryable(request);
            return query
                .Include(o=>o.AnYou)
                .Include(o=>o.City)
                .Include(o=>o.Court1)
                .Include(o=>o.Court2)
                .Include(o=>o.CaseSourceHistories)
                .Where(o => o.CaseSourceStatus == CaseSourceStatus.初始 && o.OwerId == null);
        }
        protected override async Task<IQueryable<CaseSource>> BuildKeywordQueryAsync(string keyword, IQueryable<CaseSource> query)
        {
            return (await base.BuildKeywordQueryAsync(keyword, query))
                .Where(o=>o.SourceSN.Contains(keyword));
        }
        protected override object PageResultConverter(CaseSource entity)
        {
            return new
            {
                entity.Id,
                AnYou = entity.AnYou != null ? entity.AnYou.DisplayName : "",
                City = entity.City != null ? entity.City.DisplayName : "",
                Court1 = entity.Court1 != null ? entity.Court1.DisplayName : "",
                Court2 = entity.Court2 != null ? entity.Court2.DisplayName : "",
                ValidDate=entity.ValidDate.ToString("yyyy/MM/dd"),
                entity.SourceSN,
                entity.LawyerFirms,
                History = entity.CaseSourceHistories
            };
        }
        #endregion

        #region 工作台
        /// <summary>
        /// 获取当前用户工作台案例数量
        /// </summary>
        /// <returns></returns>
        public virtual async Task<int> GetMyProcessingCount()
        {
            return await Manager.GetAll()
                .Where(o => o.OwerId == AbpSession.UserId)
                .Where(o => o.CaseSourceStatus == CaseSourceStatus.加工中 || o.CaseSourceStatus == CaseSourceStatus.被选中)
                .CountAsync();
        }
        /// <summary>
        /// 获取当前用户所有工作台案例
        /// </summary>
        /// <returns></returns>
        public virtual async Task<object> GetMyProcessingCaseSource()
        {
            var caseSources = await Manager.GetAll()
                .Include(o => o.AnYou)
                .Include(o => o.Court1)
                .Include(o => o.Court2)
                .Where(o => o.OwerId == AbpSession.UserId)
                .Where(o => o.CaseSourceStatus == CaseSourceStatus.加工中 || o.CaseSourceStatus == CaseSourceStatus.被选中)
                .Select(o => new {
                    o.Id,
                    AnYou = o.AnYou.DisplayName,
                    Court1 = o.Court1 != null ? o.Court1.DisplayName : "",
                    Court2 = o.Court2 != null ? o.Court2.DisplayName : "",
                    ValidDate = o.ValidDate.ToString("yyyy-MM-dd"),
                    o.CaseSourceStatus
                })
                .ToListAsync();

            return caseSources;
        }
        /// <summary>
        /// 选中判例
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task Choose(int id)
        {
            var caseSource = await Manager.GetByIdAsync(id);
            if(caseSource==null || caseSource.CaseSourceStatus == CaseSourceStatus.下架)
            {
                throw new UserFriendlyException("十分抱歉！该判例存在问题，正在勘误，请先挑选其他的看看哦！");
            }
            if (caseSource.OwerId != null)
            {
                throw new UserFriendlyException("十分抱歉！该判例已被别人捷足先登了，下次动作要快哦！");
            }
            var processingCount = await GetMyProcessingCount();
            var maxCount = int.Parse(await SettingManager.GetSettingValueAsync(SettingNames.maxWorkbenchCaseCount));
            if (processingCount + 1 > maxCount)
            {
                throw new UserFriendlyException("您的工作台中已经有多个案例待加工，请完成后并发布，然后再添加新的判例。");
            }
            caseSource.OwerId = AbpSession.UserId;
            caseSource.CaseSourceStatus = CaseSourceStatus.被选中;
        }
        /// <summary>
        /// 退还判例
        /// </summary>
        /// <param name="id"></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        public virtual async Task GiveBack(int id, string reason)
        {
            var manager = Manager as CaseSourceManager;
            var caseSource = await manager.GetByIdAsync(id);
            //设置案源状态
            caseSource.OwerId = null;
            caseSource.CaseSourceStatus = CaseSourceStatus.初始;
            //清空当前用户针对此案源加工的所有数据
            await manager.ClearCaseContent(id);
            //增加判例记录
            var caseHistory = new CaseSourceHistory()
            {
                CaseSourceId = id,
                Reason = reason
            };
            await Resolve<CaseSourceHistoryManager>().InsertAsync(caseHistory);
        }
        #endregion
    }
}
