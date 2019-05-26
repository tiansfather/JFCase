using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Authorization;
using Master.Dto;
using Microsoft.EntityFrameworkCore;

namespace Master.Case
{
    /// <summary>
    /// 我的精品接口
    /// </summary>
    [AbpAuthorize]
    public class MyArtAppService:MasterAppServiceBase<CaseFine,int>
    {
        #region 分页
        protected override async Task<IQueryable<CaseFine>> GetQueryable(RequestPageDto request)
        {
            var query = await base.GetQueryable(request);
            return query.Include(o => "CaseInitial.CaseSource.AnYou");
        }
        protected override object PageResultConverter(CaseFine entity)
        {
            return new
            {
                entity.Id,
                entity.IsActive,
                entity.CaseInitial.CaseSource.SourceSN,
                entity.CaseInitial.CaseSource.AnYou?.DisplayName,
                entity.Title,
                entity.Remarks,
                UserModifyTime=entity.UserModifyTime.ToString("yyyy/MM/dd")
            };
        }
        #endregion

        #region 汇总
        public virtual async Task<object> GetSummary()
        {
            //精加工数量
            var caseFineCount = await Manager.GetAll().CountAsync(o => o.CreatorUserId == AbpSession.UserId );

            return new
            {
                caseFineCount,
            };
        }
        #endregion

        #region 上下架
        public virtual async Task Freeze(int caseFineId)
        {
            var caseFine = await Manager.GetByIdAsync(caseFineId);
            caseFine.IsActive=false;
        }
        public virtual async Task UnFreeze(int caseFineId)
        {
            var caseFine = await Manager.GetByIdAsync(caseFineId);
            caseFine.IsActive = true;
        }
        #endregion
    }
}
