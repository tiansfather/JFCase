using Abp.Authorization;
using Abp.Domain.Uow;
using Abp.Web.Models;
using Master.Dto;
using Master.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.MES.Service
{
    /// <summary>
    /// 加工点账套的报工记录
    /// </summary>
    [AbpAuthorize]
    public class ProcessTaskReportProcessorAppService : MasterAppServiceBase<ProcessTaskReport, int>
    {
        protected override async Task<IQueryable<ProcessTaskReport>> GetQueryable(RequestPageDto request)
        {
            var query = (await base.GetQueryable(request))
                .Where(o => !o.ProcessTask.IsDeleted)
                .Include(o => o.ProcessTask).ThenInclude(o => o.Part).ThenInclude(o => o.Project)
                .Include(o => o.ProcessTask).ThenInclude(o => o.Supplier)
                .Include(o => o.Reporter)
                .Include(o => o.ProcessTask).ThenInclude(o => o.ProcessType)
                .Include(o => o.Tenant)
                .Where(o => o.ProcessTask.SupplierId != null)
               .Where(o => MESDbContext.GetJsonValueNumber(o.ProcessTask.Supplier.Property, "$.TenantId") == AbpSession.TenantId.Value);

            return query;

        }
        protected override object PageResultConverter(ProcessTaskReport o)
        {

            return new
            {
                o.Id,
                TenantName = o.Tenant.Name,//客户名字
                o.ProcessTask?.Part?.Project?.ProjectSN,
                o.ProcessTask?.Part?.PartName,
                o.ProcessTask?.Part?.PartSN,
                o.ProcessTask?.Part?.PartSpecification,
                o.ProcessTask?.Part?.PartNum,
                o.ProcessTask?.ProcessSN,
                o.ProcessTask?.ProcessType?.ProcessTypeName,
                o.ProcessTask?.ProjectCharger,
                TaskId = o.ProcessTask?.Id,
                RequireDate = o.ProcessTask?.RequireDate?.ToString("yyyy-MM-dd"),
                AppointDate = o.ProcessTask?.AppointDate?.ToString("yyyy-MM-dd"),
                ReporterName = o.Reporter?.Name,
                ReportType = o.ReportType.ToString(),
                ReportTime = o.ReportTime.ToString("MM-dd HH:mm"),
                CreationTime = o.CreationTime.ToString("MM-dd HH:mm"),
                o.Remarks,
                o.Files,
                o.IsDeleted
            };
        }

        [DontWrapResult]
        public override async Task<ResultPageDto> GetPageResult(RequestPageDto request)
        {
            CurrentUnitOfWork.DisableFilter(AbpDataFilters.MustHaveTenant, AbpDataFilters.MayHaveTenant);

            return await base.GetPageResult(request);

        }
    }
}
