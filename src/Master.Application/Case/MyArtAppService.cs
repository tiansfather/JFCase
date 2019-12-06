using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Runtime.Security;
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
            return query.Include("CaseInitial.CaseSource.AnYou")
                .Where(o => o.CaseInitial.CaseSource.OwerId == AbpSession.UserId)
                .Where(o=>o.CaseStatus==CaseStatus.展示中||o.CaseStatus==CaseStatus.下架);
            ;
        }
        protected override object PageResultConverter(CaseFine entity)
        {
            return new
            {
                entity.Id,
                EncrypedId = SimpleStringCipher.Instance.Encrypt(entity.CaseInitial.CaseSource.Id.ToString(), null, null),//加密后的案源id
                entity.CaseStatus,
                entity.CaseInitial.CaseSource.SourceSN,
                entity.CaseInitial.CaseSource.SourceFile,
                AnYou=entity.CaseInitial.CaseSource.AnYou?.DisplayName,
                PublishDate = entity.PublishDate?.ToString("yyyy/MM/dd"),
                entity.Title,
                entity.Content,
                entity.MediaPath,
                entity.Remarks,
                UserModifyTime = entity.UserModifyTime.ToString("yyyy/MM/dd"),
            };
        }
        #endregion

        #region 汇总
        public virtual async Task<object> GetSummary()
        {
            //精加工数量
            var caseFineCount = await Manager.GetAll().CountAsync(o => o.CreatorUserId == AbpSession.UserId && o.CaseStatus == CaseStatus.展示中 || o.CaseStatus == CaseStatus.下架);

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
            caseFine.CaseStatus = CaseStatus.下架;
        }
        public virtual async Task UnFreeze(int caseFineId)
        {
            var caseFine = await Manager.GetByIdAsync(caseFineId);
            caseFine.CaseStatus = CaseStatus.展示中;
        }
        #endregion
    }
}
