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
                .Where(o => o.IsActive && o.CaseInitial.CaseStatus == CaseStatus.展示中);
        }

        protected override object PageResultConverter(CaseCard entity)
        {
            return new
            {
                entity.Id,
                entity.CaseInitial.CaseSource.SourceSN,
                entity.Title,
                entity.Content,
                entity.CaseInitial.CaseSource.TrialPeople,
                entity.CaseInitial.CaseSource.ValidDate,
                entity.CreatorUserId,
                City =entity.CaseInitial.CaseSource.City?.DisplayName,
                AnYou = entity.CaseInitial.CaseSource.AnYou?.DisplayName,
                Court1 = entity.CaseInitial.CaseSource.Court1?.DisplayName,
                Court2 = entity.CaseInitial.CaseSource.Court2?.DisplayName
            };
        }
    }
}