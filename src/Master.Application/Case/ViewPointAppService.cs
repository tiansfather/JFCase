using Abp.Authorization;
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
    /// 看观点接口
    /// </summary>
    [AbpAuthorize]
    public class ViewPointAppService:MasterAppServiceBase<CaseCard,int>
    {
        protected override async Task<IQueryable<CaseCard>> GetQueryable(RequestPageDto request)
        {
            var query=await  base.GetQueryable(request);
            return query.Include("CaseInitial.CaseSource.AnYou")
                .Include("CaseInitial.CaseSource.City")
                .Include("CaseInitial.CaseSource.Court1")
                .Include("CaseInitial.CaseSource.Court2")
                .Where(o => o.CaseStatus==CaseStatus.展示中 && o.CaseInitial.CaseStatus == CaseStatus.展示中);
        }
        protected override async Task<IQueryable<CaseCard>> BuildKeywordQueryAsync(string keyword, IQueryable<CaseCard> query)
        {
            return (await base.BuildKeywordQueryAsync(keyword, query))
                .Where(o=>o.Title.Contains(keyword) 
                ||o.Content.Contains(keyword)
                ||o.CaseInitial.CaseSource.SourceSN.Contains(keyword)
                || o.CaseInitial.CaseSource.City.DisplayName.Contains(keyword)
                || o.CaseInitial.CaseSource.Court1.DisplayName.Contains(keyword)
                || o.CaseInitial.CaseSource.Court2.DisplayName.Contains(keyword)
                );
        }
        protected override object PageResultConverter(CaseCard entity)
        {
            return new
            {
                entity.Id,
                entity.CaseInitial.CaseSource.SourceSN,
                entity.Title,
                entity.Content,
                TrialPeople = entity.CaseInitial.CaseSource.TrialPeople.Select(o => new { o.Name, TrialRole = o.TrialRole.ToString() }),
                entity.CaseInitial.CaseSource.LawyerFirms,
                entity.CaseInitial.CaseSource.ValidDate,
                entity.CaseInitial.CaseSource.SourceFile,
                entity.CreatorUserId,
                City =entity.CaseInitial.CaseSource.City?.DisplayName,
                AnYou = entity.CaseInitial.CaseSource.AnYou?.DisplayName,
                Court1 = entity.CaseInitial.CaseSource.Court1?.DisplayName,
                Court2 = entity.CaseInitial.CaseSource.Court2?.DisplayName
            };
        }
    }
}
