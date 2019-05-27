﻿using Abp.Authorization;
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
                .Include(o => o.CaseFines)
                .Include(o => o.CaseCards)
                .Include(o => o.CaseKeys)
                .Where(o => o.CaseSource.OwerId == AbpSession.UserId);

        }
        protected override object PageResultConverter(CaseInitial entity)
        {
            var subjectCaseKey = entity.CaseKeys.Where(o => o.KeyName == "专题").FirstOrDefault();
            return new
            {
                entity.Id,
                entity.CaseSource.SourceSN,
                AnYou = entity.CaseSource.AnYou.DisplayName,
                entity.Remarks,
                entity.ReadNumber,
                PublisDate = entity.PublisDate?.ToString("yyyy/MM/dd"),
                CaseFineCount = entity.CaseFines.Count,
                CaseCardCount = entity.CaseCards.Count,
                entity.CaseStatus,
                subjectId = subjectCaseKey?.KeyNodeId,//专题Id
                subjectName = subjectCaseKey?.KeyValue//专题名称
            };
        }
        #endregion

        #region 汇总
        public virtual async Task<object> GetSummary()
        {
            //已发布的案例数量
            var caseCount = await Manager.GetAll().CountAsync(o => o.CreatorUserId == AbpSession.UserId && o.PublisDate != null);
            //案例卡数量
            var caseCardCount = await Resolve<CaseCardManager>().GetAll().Where(o => o.CreatorUserId == AbpSession.UserId && o.IsActive).CountAsync();

            return new
            {
                caseCount,
                caseCardCount
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