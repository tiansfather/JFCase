using Abp.Authorization;
using Abp.Runtime.Security;
using Master.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.Case
{
    /// <summary>
    /// 我的案例接口
    /// </summary>
    [AbpAuthorize]
    public class MyCaseAppService : MasterAppServiceBase<CaseInitial, int>
    {
        #region 分页
        protected override async Task<IQueryable<CaseInitial>> GetQueryable(RequestPageDto request)
        {
            var query = await base.GetQueryable(request);

            return query.Include("CaseSource.AnYou")
                .Include(o=>o.Subject)
                .Include(o => o.CaseFines)
                .Include(o => o.CaseCards)
                .Include(o => o.CaseNodes)
                .Where(o => o.CaseSource.OwerId == AbpSession.UserId)
                .Where(o=>o.CaseStatus==CaseStatus.展示中||o.CaseStatus==CaseStatus.下架)
                //.OrderByDescending(o=>o.CaseSource.ValidDate)
                .OrderByDescending(o=>o.PublishDate)
                ;

        }

        protected override async Task<IQueryable<CaseInitial>> BuildKeywordQueryAsync(string keyword, IQueryable<CaseInitial> query)
        {
            return (await base.BuildKeywordQueryAsync(keyword, query))
                  .Where(o => o.Title.Contains(keyword)
                || o.Introduction.Contains(keyword)
                || o.CaseSource.SourceSN.Contains(keyword)
                || o.CaseSource.City.DisplayName.Contains(keyword)
                || o.CaseSource.Court1.DisplayName.Contains(keyword)
                || o.CaseSource.Court2.DisplayName.Contains(keyword)
                || o.CaseSource.TrialPeopleField.Contains(keyword)
                || o.CaseSource.LawyerFirmsField.Contains(keyword)
                || o.Subject.DisplayName.Contains(keyword)
                || o.Law.Contains(keyword)
                || o.LawyerOpinion.Contains(keyword)
                || o.Experience.Contains(keyword)
                || o.Property.Json.Contains(keyword)
                || o.CaseCards.Count(c => c.Title.Contains(keyword) || c.Content.Contains(keyword)) > 0
                || o.Remarks.Contains(keyword)//包含备注的查询
                );
        }
        protected override object PageResultConverter(CaseInitial entity)
        {
            return new
            {
                entity.Id,
                EncrypedId = SimpleStringCipher.Instance.Encrypt(entity.CaseSource.Id.ToString(), null, null),//加密后的案源id
                entity.CaseSource.SourceSN,
                SourceId=entity.CaseSource.Id,
                entity.CaseSource.SourceFile,
                AnYou = entity.CaseSource.AnYou.DisplayName,
                entity.CaseSource.AnYouId,
                entity.Remarks,
                entity.ReadNumber,
                PublishDate = entity.PublishDate?.ToString("yyyy/MM/dd"),
                CaseFineCount = entity.CaseFines.Count,
                CaseCardCount = entity.CaseCards.Count,
                entity.CaseStatus,
                entity.SubjectId,
                entity.Subject?.DisplayName
            };
        }
        #endregion

        #region 汇总
        public virtual async Task<object> GetSummary()
        {
            //已发布的案例数量
            var caseCount = await Manager.GetAll().CountAsync(o => o.CreatorUserId == AbpSession.UserId && (o.CaseStatus==CaseStatus.展示中 ||o.CaseStatus==CaseStatus.下架));
            //案例卡数量
            var caseCardCount = await Resolve<CaseCardManager>().GetAll().Where(o => o.CreatorUserId == AbpSession.UserId && (o.CaseStatus == CaseStatus.展示中 || o.CaseStatus == CaseStatus.下架)).CountAsync();
            //精加工数量
            var caseFineCount= await Resolve<CaseFineManager>().GetAll().Where(o => o.CreatorUserId == AbpSession.UserId && (o.CaseStatus == CaseStatus.展示中 || o.CaseStatus == CaseStatus.下架)).CountAsync();
            return new
            {
                caseCount,
                caseCardCount,
                caseFineCount
            };
        }
        #endregion

        #region 设置专题
        public virtual async Task SetSubject(int caseInitialId,int subjectId)
        {
            var caseInitial = await Manager.GetByIdAsync(caseInitialId);
            caseInitial.SubjectId = subjectId;
        }
        #endregion

        #region 上下架
        public virtual async Task Freeze(int caseInitialId)
        {
            var caseInitial = await Manager.GetByIdAsync(caseInitialId);
            caseInitial.CaseStatus = CaseStatus.下架;
        }
        public virtual async Task UnFreeze(int caseInitialId)
        {
            var caseInitial = await Manager.GetByIdAsync(caseInitialId);
            caseInitial.CaseStatus = CaseStatus.展示中;
        }
        #endregion
    }
}
