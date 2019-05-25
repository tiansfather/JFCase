using Abp.Authorization;
using Abp.Domain.Uow;
using Abp.Web.Models;
using Master.Dto;
using Master.EntityFrameworkCore;
using Master.Projects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace Master.MES.Service
{
    [AbpAuthorize]
    public class MESProjectCustomerAppService : MasterAppServiceBase<Project,int>
    {

        #region 分页
        /// <summary>
        /// 获取当前账套发放出去的模具
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        protected override async Task<IQueryable<Project>> GetQueryable(RequestPageDto request)
        {
            var query = (await base.GetQueryable(request))
                .Include(o=>o.Tenant)
                    .Where(o => MasterDbContext.GetJsonValueNumber(o.Unit.Property, "$.TenantId") == AbpSession.TenantId.Value);
            return query;
        }

        [DontWrapResult]
        public override async Task<ResultPageDto> GetPageResult(RequestPageDto request)
        {
            CurrentUnitOfWork.DisableFilter(AbpDataFilters.MustHaveTenant, AbpDataFilters.MayHaveTenant);

            return await base.GetPageResult(request);

        }

        protected override object PageResultConverter(Project entity)
        {
            //获取项目状态
            var reportCount = Resolve<ProcessTaskReportManager>().GetAll().Count(o => o.ProcessTask.Part.ProjectId == entity.Id);//项目的报工数量
            var taskCount = Resolve<ProcessTaskManager>().GetAll().Count(o => o.Part.ProjectId == entity.Id);//项目的任务数
            var completeTaskCount = Resolve<ProcessTaskManager>().GetAll().Count(o => o.Part.ProjectId == entity.Id && o.ProcessTaskStatus == ProcessTaskStatus.Completed);//项目已完成任务数
            return new
            {
                entity.Id,
                entity.Tenant.TenancyName,
                entity.ProjectName,
                entity.ProjectSN,
                entity.CustomerProjectSN,
                entity.OrderDate,
                Status=reportCount==0?"未开始":(taskCount==completeTaskCount?"已完成":"加工中")
            };
        }
        #endregion

        #region 模具零件列表
        /// <summary>
        /// 获取零件加工信息
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public virtual async Task<object> GetProjectPartInfos(int projectId)
        {
            var parts = await Resolve<PartManager>().GetAll().IgnoreQueryFilters()
                .Where(o=>!o.IsDeleted)
                    .Include("ProcessTasks.ProcessTaskReports")
                    .Include("ProcessTasks.ProcessType")
                    .Where(o => o.EnableProcess)
                    .Where(o => MESDbContext.GetJsonValueNumber(o.Project.Unit.Property, "$.TenantId") == AbpSession.TenantId.Value)
                    .Where(o => o.ProjectId == projectId)
                    .ToListAsync()
                    ;

            return parts.Select(o =>
            {
                //零件的最后一次报工
                var lastReport = o.ProcessTasks.SelectMany(p => p.ProcessTaskReports).OrderBy(r => r.CreationTime).LastOrDefault();
                return new
                {
                    o.Id,
                    o.PartSN,
                    o.PartName,
                    o.PartImg,
                    LastReportTime = lastReport?.CreationTime,
                    lastReport?.ProcessTask.ProcessType.ProcessTypeName,
                    lastReport?.Files
                };
            });
        }
        #endregion

        #region 零件加工报工明细
        /// <summary>
        /// 零件的报工明细
        /// </summary>
        /// <param name="partId"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public virtual async Task<object> GetPartReportInfos(int partId,string where)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MustHaveTenant))
            {
                var query = Resolve<ProcessTaskReportManager>().GetAll()
                    .Include(o=>o.ProcessTask).ThenInclude(o=>o.ProcessType)
                    .Where(o => MESDbContext.GetJsonValueNumber(o.ProcessTask.Part.Project.Unit.Property, "$.TenantId") == AbpSession.TenantId.Value)
                    .Where(o => o.ProcessTask.PartId == partId);

                if (!string.IsNullOrEmpty(where))
                {
                    query = query.Where(where);
                }

                return await query.Select(o => new
                {
                    o.ReportType,
                    o.ProcessTask.ProcessType.ProcessTypeName,
                    o.ReportTime,
                    o.Files
                }).ToListAsync();

                //return await query.ToListAsync();
            }
           
        }
        #endregion

        /// <summary>
        /// 获取当前客户绑定了的所有模具厂
        /// </summary>
        /// <returns></returns>
        public virtual async Task<object> GetManufactures()
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MustHaveTenant))
            {
                var projectManager = Resolve<MESProjectManager>();

                var units = await Resolve<MESUnitManager>()
                    .GetAll()
                    .Include(o => o.Tenant)
                    .Where(o => MasterDbContext.GetJsonValueNumber(o.Property, "$.TenantId") == AbpSession.TenantId.Value)
                    .Select(o => new { o.TenantId, o.Tenant.Name, o.Id })
                    .ToListAsync();
                var tenants = units
                    .Select(o => new { o.TenantId, o.Name })
                    .Distinct();

                var result = new List<object>();

                foreach (var tenant in tenants)
                {
                    var unitIds = units.Where(o => o.TenantId == tenant.TenantId).Select(o => o.Id);
                    var baseQuery = projectManager.GetAll().Where(p => p.TenantId == tenant.TenantId && p.UnitId.HasValue && unitIds.Contains(p.UnitId.Value));
                    //在对应模具厂做的所有项目
                    var projectCount = baseQuery.Count();
                    result.Add(new
                    {
                        tenant.TenantId,
                        tenant.Name,
                        projectCount
                    });
                }


                return result;
            }
        }
    }
}
