using Abp.Authorization;
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
    public class ViewCaseAppService:MasterAppServiceBase<CaseInitial,int>
    {
        protected override async Task<IQueryable<CaseInitial>> GetQueryable(RequestPageDto request)
        {
            var query=await base.GetQueryable(request);
            return query.Where(o=>o.CaseStatus==CaseStatus.展示中);
        }

        protected override object PageResultConverter(CaseInitial entity)
        {
            return new
            {
                entity.Id,
                entity.Title,
                entity.Introduction,
                entity.ReadNumber
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
