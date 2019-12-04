using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.UI;
using Master.Dto;
using Master.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.Case
{
    public class ViewCaseAppService:MasterAppServiceBase<CaseInitial,int>
    {
        protected override async Task<IQueryable<CaseInitial>> GetQueryable(RequestPageDto request)
        {
            var query=await base.GetQueryable(request);
            return query
                .IgnoreQueryFilters()
                .Where(o=>!o.IsDeleted)
                .Include(o=>o.CaseSource).ThenInclude(o=>o.AnYou)
                .Include(o=>o.CreatorUser)
                .Where(o=>o.CaseStatus==CaseStatus.展示中)
                .OrderByDescending(o=>o.IsActive)
                .ThenByDescending(o=>o.PublishDate);
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
                );
        }
        protected override async Task<IQueryable<CaseInitial>> BuildSearchQueryAsync(IDictionary<string, string> searchKeys, IQueryable<CaseInitial> query)
        {
            if (searchKeys.ContainsKey("typeIds") && !string.IsNullOrEmpty(searchKeys["typeIds"]))
            {
                var typeIds = searchKeys["typeIds"].Split(',');
                foreach(var typeId in typeIds)
                {
                    if (!string.IsNullOrEmpty(typeId))
                    {
                        query = query.Where(o => o.CaseNodes.Count(n => n.BaseTreeId == int.Parse(typeId)) > 0);
                    }
                }
            }
            if (searchKeys.ContainsKey("labelIds") && !string.IsNullOrEmpty(searchKeys["labelIds"]))
            {
                var labelIds = searchKeys["labelIds"].Split(',');
                foreach (var labelId in labelIds)
                {
                    if (!string.IsNullOrEmpty(labelId))
                    {
                        query = query.Where(o => o.CaseLabels.Count(n => n.LabelId == int.Parse(labelId)) > 0);
                    }
                        
                }
            }
            return query;
        }
        protected override object PageResultConverter(CaseInitial entity)
        {
            var caseCount = Manager.GetAll()
                .Count(o => o.CreatorUserId == entity.CreatorUserId && (o.CaseStatus == CaseStatus.展示中));
            return new
            {
                entity.Id,
                entity.Title,
                entity.Introduction,
                entity.ReadNumber,
                PublishDate=entity.PublishDate?.ToString("yyyy-MM-dd"),
                Avata= entity.CreatorUser?.GetPropertyValue("Avata"),
                CreatorAnYou = entity.CreatorUser?.GetPropertyValue<string>("AnYou"),
                CreatorName =entity.CreatorUser?.Name,
                entity.CreatorUser?.PhoneNumber,
                WorkYear=entity.CreatorUser?.GetPropertyValue<string>("WorkYear"),
                entity.CreatorUser?.WorkLocation,
                caseCount,
                entity.CaseSourceId,
                entity.CaseSource.SourceSN,
                AnYou=entity.CaseSource.AnYou?.DisplayName
            };
        }

        /// <summary>
        /// 查看某案例
        /// </summary>
        /// <param name="caseInitialId"></param>
        /// <returns></returns>
        public virtual async Task<int> View(int caseInitialId)
        {
            var caseInitial = await Manager.GetByIdAsync(caseInitialId);
            if (caseInitial.CaseStatus != CaseStatus.展示中)
            {
                throw new UserFriendlyException("该案例已临时下架，敬请谅解，请查看其它案例。");
            }
            var readHistoryRepository = Resolve<IRepository<CaseReadHistory, int>>();
            if(await readHistoryRepository.CountAsync(o=>o.CaseInitialId==caseInitialId && o.CreatorUserId == AbpSession.UserId) == 0)
            {
                caseInitial.ReadNumber++;
                await readHistoryRepository.InsertAsync(new CaseReadHistory()
                {
                    CaseInitialId = caseInitialId
                }
                );
            }
            
            return caseInitial.CaseSourceId;
        }
    }
}
