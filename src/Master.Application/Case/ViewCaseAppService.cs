using Abp.Authorization;
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
    [AbpAuthorize]
    public class ViewCaseAppService:MasterAppServiceBase<CaseInitial,int>
    {
        protected override async Task<IQueryable<CaseInitial>> GetQueryable(RequestPageDto request)
        {
            var query=await base.GetQueryable(request);
            return query
                .Include(o=>o.CreatorUser)
                .Where(o=>o.CaseStatus==CaseStatus.展示中);
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
            return new
            {
                entity.Id,
                entity.Title,
                entity.Introduction,
                entity.ReadNumber,
                Avata= entity.CreatorUser.GetPropertyValue("Avata")
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
            caseInitial.ReadNumber++;
            return caseInitial.CaseSourceId;
        }
    }
}
