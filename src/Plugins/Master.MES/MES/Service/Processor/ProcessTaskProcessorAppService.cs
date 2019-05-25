using Abp.Authorization;
using Abp.Domain.Uow;
using Abp.Web.Models;
using Master.Dto;
using Master.Entity;
using Master.EntityFrameworkCore;
using Master.Extensions;
using Master.MES.Specifications;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.MES.Service
{
    /// <summary>
    /// 针对加工点账套的加工任务接口
    /// </summary>
    [AbpAuthorize]
    public class ProcessTaskProcessorAppService : MasterAppServiceBase<ProcessTask, int>
    {
        public MESUnitManager MESUnitManager { get; set; }
        public ProcessTaskManager ProcessTaskManager { get; set; }

        protected override async Task<IQueryable<ProcessTask>> BuildSearchQueryAsync(IDictionary<string, string> searchKeys, IQueryable<ProcessTask> query)
        {
            var result=await base.BuildSearchQueryAsync(searchKeys, query);

            if (searchKeys.ContainsKey("tenantId"))
            {
                //获取某个模具厂发过来的加工任务
                var tenantId =int.Parse( searchKeys["tenantId"]);
                result = result.Where(o => o.TenantId == tenantId);
            }

            return result;
        }

        #region 加工任务分页
        /// <summary>
        /// 获取当前账套被分配到的加工任务
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        protected override async Task<IQueryable<ProcessTask>> GetQueryable(RequestPageDto request)
        {
            var query = (await base.GetQueryable(request))
                    .Include(o => o.Tenant)
                    .Include(o => o.Part).ThenInclude(o => o.Project)
                    .Include(o => o.Equipment)
                    .Include(o => o.Supplier)
                    .Include(o => o.ProcessTaskReports)
                    .Include(o => o.ProcessType)
                    .Where(o => o.SupplierId != null)
                    .Where(o => o.ProcessTaskStatus != 0)
                    .Where(o => MESDbContext.GetJsonValueNumber(o.Supplier.Property, "$.TenantId") == AbpSession.TenantId.Value);
            return query;
        }

        [DontWrapResult]
        public override async Task<ResultPageDto> GetPageResult(RequestPageDto request)
        {
            CurrentUnitOfWork.DisableFilter(AbpDataFilters.MustHaveTenant,AbpDataFilters.MayHaveTenant);

            return await base.GetPageResult(request);

        }

        protected override object PageResultConverter(ProcessTask o)
        {
            //计算初始金额 
            decimal? jobFee = 0;
            if (o.FeeType == FeeType.承包)
            {
                jobFee = o.JobFee;
            }
            else if (o.FeeType == FeeType.按时间)
            {
                jobFee = o.Price * o.EstimateHours;
            }
            else
            {
                jobFee = o.Price * o.FeeFactor;
            }

            Func<decimal?,int, string> func = (d,i) =>
             {
                 if (d == null) { return ""; }
                 else {
                     return d.Value.IsInteger() ? d.Value.ToString("0") : Math.Round(d.Value, i).ToString();
                 }
             };

            return new
            {
                o.Id,
                TenantName = o.Tenant.Name,//客户名字
                o.ProcessSN,
                o.Part.PartName,
                o.Part.PartSN,
                o.Part.PartSpecification,
                o.Part.PartNum,
                o.Part.Project.ProjectSN,
                o.ProjectCharger,
                o.ProcessType?.ProcessTypeName,
                AppointDate = o.AppointDate?.ToString("yyyy-MM-dd"),//预约
                RequireDate = o.RequireDate?.ToString("yyyy-MM-dd"),//要求
                o.EstimateHours,//预计工时
                o.FeeType,
                o.Price,
                JobFee=func(o.JobFee,2),
                Fee = func(o.Fee, 2),
                o.ProcessTaskStatus,
                //JobFee = jobFee == null ? null : Math.Round(jobFee.Value, 2).ToString(),//初始金额
                //Fee = o.Fee == null ? null : Math.Round(o.Fee.Value, 2).ToString(),//实际金额
                o.FeeFactor,//计价单位
                o.ProcessTaskReports.LastOrDefault()?.Files,//报工图片
                StartDate = o.StartDate!=null? o.StartDate.Value.ToString("yyyy-MM-dd HH:mm"):"未报工",//实际上机
                EndDate = o.EndDate != null ? o.EndDate.Value.ToString("yyyy-MM-dd HH:mm") : "未报工",//实际下机
                ActualHours = func(o.ActualHours, 2),
                //ActualHours = o.ActualHours == null ? "未完成" : Math.Round(o.ActualHours.Value, 2).ToString(),//实际工时
                CreationTime = o.CreationTime.ToString("yyyy-MM-dd HH:mm"),
                KaiDate = o.KaiDate?.ToString("yyyy-MM-dd HH:mm"),
                o.SheetFile,//加工示意图
                o.TaskInfo,
                SubmitFeeFromProcessor = o.GetPropertyValue("SubmitFeeFromProcessorDto"),
            };
        }
        #endregion

        /// <summary>
        /// 获取当前加工点绑定了的所有模具厂
        /// </summary>
        /// <returns></returns>
        public virtual async Task<object> GetCustomers()
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MustHaveTenant))
            {
                var units = await MESUnitManager
                    .GetAll()
                    .Include(o => o.Tenant)
                    .Where(o => MasterDbContext.GetJsonValueNumber(o.Property, "$.TenantId") == AbpSession.TenantId.Value)
                    .Select(o => new { o.TenantId, o.Tenant.Name, o.Id })
                    .ToListAsync();
                var tenants=units
                    .Select(o=>new { o.TenantId,o.Name})
                    .Distinct();

                var result = new List<object>();

                foreach(var tenant in tenants)
                {
                    var unitIds = units.Where(o => o.TenantId == tenant.TenantId).Select(o => o.Id);
                    var baseQuery = ProcessTaskManager.GetAll().Where(p => p.TenantId == tenant.TenantId &&p.Status.Contains(ProcessTask.Status_SendProcessor) && p.SupplierId!=null && unitIds.Contains(p.SupplierId.Value));
                    //模具厂发过来的所有加工任务
                    var taskCount = baseQuery.Count();
                    //延期任务
                    var delayCount = baseQuery.Where(new DelayTaskSpecification()).Count();
                    result.Add( new
                    {
                        tenant.TenantId,
                        tenant.Name,
                        taskCount,
                        delayCount
                    });
                }


                return result;
            }
        }
    }
}
