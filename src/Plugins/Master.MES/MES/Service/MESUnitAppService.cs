using Abp.Authorization;
using Master.Units;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Master.Entity;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Master.Configuration;
using Abp.Configuration;
using Abp.AutoMapper;
using Master.MES.Dtos;
using Master.EntityFrameworkCore;
using Z.EntityFramework.Plus;
using Abp.Linq.Extensions;
using Master.Notices;
using Master.MES.Jobs;
using Abp.Domain.Entities;
using Master.EntityFrameworkCore.Extensions;
using Abp.UI;

namespace Master.MES.Service
{
    [AbpAuthorize]
    public class MESUnitAppService : UnitAppService
    {
        public ProcessTaskManager ProcessTaskManager { get; set; }
        public ProcessTypeManager ProcessTypeManager { get; set; }
        public MESUnitManager MESUnitManager { get; set; }
        public RemindLogManager RemindLogManager { get; set; }
        public NoticeManager NoticeManager { get; set; }
        /// <summary>
        /// 解除加工点和账套的绑定
        /// </summary>
        /// <param name="unitId"></param>
        /// <returns></returns>
        public virtual async Task UnBindTenant(int[] unitIds)
        {
            var units = await Manager.GetListByIdsAsync(unitIds);
            foreach(var unit in units)
            {
                unit.RemoveProperty("TenantId");
                await Manager.UpdateAsync(unit);
            }
            await CurrentUnitOfWork.SaveChangesAsync();
        }
        /// <summary>
        /// 获取往来单位中供应商及对应的未核算加工单数
        /// </summary>
        /// <returns></returns>
        public virtual async Task<object> GetAllUnitUnCheckCount()
        {
            var query = Manager.GetAll().Where(o=>o.UnitNature==UnitNature.供应商 || o.UnitNature==UnitNature.客户及供应商);
            var units = await query.ToListAsync();
            var mustReturnFile = await SettingManager.GetSettingValueAsync<bool>(MESSettingNames.MustReturnFileBeforeCheck);

            var result = new List<object>();
            foreach(var unit in units)
            {
                var taskQuery = ProcessTaskManager.GetAll().Where(t => t.SupplierId == unit.Id && t.ProcessTaskStatus != ProcessTaskStatus.Inputed && !t.Status.Contains(ProcessTask.Status_Checked));
                if (mustReturnFile)
                {
                    taskQuery = taskQuery.Where(t => t.Status != null && t.Status.Contains(ProcessTask.Status_Verify));
                }
                result.Add(new
                {
                    unit.Id,
                    unit.UnitName,
                    UnCheckTaskCount = taskQuery.Count()
                });
            }            



            return result;
        }


        /// <summary>
        /// 通过工序获取加工点信息，开单数量多的排序在前
        /// </summary>
        /// <param name="processTypeName"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual async Task<List<UnitDto>> GetAllUnitByProcessTypeName(string processTypeName,string key)
        {
            var units = await (Manager as UnitManager).GetAll().Include(o=>o.UnitType).Where(o => o.UnitNature == UnitNature.供应商 || o.UnitNature == UnitNature.客户及供应商).Where(o=> o.SupplierType!=null && o.SupplierType.Contains("加工")).WhereIf(!string.IsNullOrEmpty(key),o=>o.UnitName.Contains(key)).ToListAsync();
            if (string.IsNullOrEmpty(processTypeName))
            {
                //Logger.Error(Newtonsoft.Json.JsonConvert.SerializeObject(units));
                return units.MapTo<List<UnitDto>>();
            }
            var processType = await ProcessTypeManager.GetAll().Where(o=>o.ProcessTypeName==processTypeName).FirstOrDefaultAsync();
            if (processType == null)
            {
                return units.MapTo<List<UnitDto>>();
            }
            var taskedSupplierDtos =await ProcessTaskManager.GetAll().Where(o => o.ProcessTypeId == processType.Id && o.SupplierId != null).GroupBy(o => o.SupplierId)
                .Select(o => new { Id=o.Key, Count = o.Count() }).OrderByDescending(o => o.Count).ToListAsync();

            var result = new List<UnitDto>();
            
            foreach(var supplier in taskedSupplierDtos)
            {
                var unitDto = units.FirstOrDefault(o => o.Id == supplier.Id);
                if (unitDto != null)
                {
                    result.Add(unitDto.MapTo<UnitDto>());
                }
            }
            result.AddRange(units.Where(o => !taskedSupplierDtos.Exists(t => t.Id == o.Id)).MapTo<List<UnitDto>>());

            return result;
        }

        /// <summary>
        /// 获取一段时间内加工点的报工数据对比报表
        /// </summary>
        /// <param name="processTypeId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public virtual async Task<List<MESUnitReportRankDto>> GetUnitRankReport(int? processTypeId,DateTime? startDate,DateTime? endDate)
        {
            var allUnits = await Manager.GetAllList();
            var result=new List<MESUnitReportRankDto>();
            //如果不传入时间，默认查询所有加工单
            var queryStartDate = startDate ?? DateTime.Parse("2018-01-01");
            var queryEndDate = endDate ?? DateTime.Now;

            var taskBaseQuery = ProcessTaskManager.GetAll()
                .Include(o=>o.ProcessTaskReports)
                .WhereIf(processTypeId!=null,o=>o.ProcessTypeId==processTypeId)
                .Where(o =>o.Status==null || !o.Status.Contains(ProcessTask.Status_Inner))//非厂内任务
                .Where(o => o.SupplierId != null && o.ProcessTaskStatus != ProcessTaskStatus.Inputed)//已选择加工点且已开单
                .Where(o=>o.KaiDate>=queryStartDate && o.KaiDate <= queryEndDate)
                //.Where(o => MESDbContext.GetJsonValueString(o.Property, "$.KaiDate") > queryStartDate.ToString("yyyy-MM-dd HH:mm:ss") && MESDbContext.GetJsonValueString(o.Property, "$.KaiDate") < queryEndDate.ToString("yyyy-MM-dd HH:mm:ss"))//开单日期在时间段内
                ;

            var units = (await taskBaseQuery.GroupBy(o => o.SupplierId).Select(o => new { UnitId = o.Key }).ToListAsync())
                .Where(o => allUnits.Exists(u => u.Id == o.UnitId));

            foreach(var unit in units)
            {
                var reportDto =await GetUnitReport(unit.UnitId.Value, taskBaseQuery);
                reportDto.UnitName = allUnits.Single(a => a.Id == unit.UnitId.Value).UnitName;
                result.Add(reportDto);
                //var taskQueryBySupplier = taskBaseQuery.Where(o => o.SupplierId == unit.UnitId);
                ////总任务数
                //var taskCount = await taskQueryBySupplier.CountAsync();
                ////总报工记录数
                //var reportCount = await taskQueryBySupplier.SelectMany(o => o.ProcessTaskReports).CountAsync();
                ////有报工的任务数
                //var reportTaskCount = await taskQueryBySupplier.Where(o => o.ProcessTaskReports.Count > 0).CountAsync();
                ////todo:完成报工的任务数量的计算方式 
                //var completedCount =await taskQueryBySupplier.Where(o => o.EndDate != null).CountAsync();
                ////延期上机任务数
                //var delayStartCount =await taskQueryBySupplier.Where(o => o.AppointDate != null && ((o.StartDate != null && o.StartDate > o.AppointDate) || (o.StartDate == null && DateTime.Now > o.AppointDate))).CountAsync();
                ////延期下机任务数
                //var delayEndCount =await taskQueryBySupplier.Where(o => o.RequireDate != null && ((o.EndDate != null && o.EndDate > o.RequireDate) || (o.EndDate == null && DateTime.Now > o.RequireDate))).CountAsync();
                ////超时任务数
                //var overHourCount=await taskQueryBySupplier.Where(o => o.EstimateHours != null && o.ActualHours != null && o.ActualHours > o.EstimateHours).CountAsync();
                ////不合格任务数
                //var NGCount =await taskQueryBySupplier.Where(o => MESDbContext.GetJsonValueNumber(o.Property, "$.QuanlityType") == (int)QuanlityType.不合格).CountAsync();
                //result.Add(new MESUnitReportRankDto()
                //{
                //    UnitId=unit.UnitId??0,
                //    UnitName=allUnits.Single(a=>a.Id==unit.UnitId.Value).UnitName,
                //    TaskCount= taskCount,
                //    ReportCount= reportCount,
                //    ReportTaskCount= reportTaskCount,
                //    CompletedCount= completedCount,
                //    DelayStartCount= delayStartCount,
                //    DelayEndCount= delayEndCount,
                //    OverHourCount= overHourCount,
                //    NGCount= NGCount
                //});
            }

            return result;
        }

        /// <summary>
        /// 获取某个加工点一段时间按月的报工报表
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="range"></param>
        /// <param name="unitId"></param>
        /// <returns></returns>
        public virtual async Task<object> GetUnitDateRangeReport(int unitId,int? year,int? month,int? range)
        {
            var startYear = year ?? DateTime.Now.AddMonths(-12).Year;
            var startMonth = month ?? DateTime.Now.AddMonths(-11).Month;
            var monthRange = range ?? 12;
            var startDate= DateTime.Parse($"{startYear}-{startMonth}-01");

            var result = new List<object>();

            for (var m = 0;m< monthRange; m++)
            {
                var queryStartDate = startDate.AddMonths(m);
                var queryEndDate = queryStartDate.AddMonths(1).AddDays(-1);

                var taskBaseQuery = ProcessTaskManager.GetAll()
                    .Include(o => o.ProcessTaskReports)
                    .Where(o => o.Status == null || !o.Status.Contains(ProcessTask.Status_Inner))//非厂内任务
                    .Where(o => o.SupplierId != null && o.ProcessTaskStatus != ProcessTaskStatus.Inputed)//已选择加工点且已开单
                    .Where(o => o.KaiDate >= queryStartDate && o.KaiDate <= queryEndDate);

                var report = await GetUnitReport(unitId, taskBaseQuery);

                result.Add(new
                {
                    Date=$"{queryStartDate.Year}-{queryStartDate.Month}",
                    Report=report,
                });
            }

            return result;
        }

        private async Task<MESUnitReportRankDto> GetUnitReport(int unitId,IQueryable<ProcessTask> taskBaseQuery)
        {
            var taskQueryBySupplier = taskBaseQuery.Where(o => o.SupplierId == unitId);
            //总任务数
            var taskCount = await taskQueryBySupplier.CountAsync();
            //总报工记录数
            var reportCount = await taskQueryBySupplier.SelectMany(o => o.ProcessTaskReports).CountAsync();
            //有报工的任务数
            var reportTaskCount = await taskQueryBySupplier.Where(o => o.ProcessTaskReports.Count > 0).CountAsync();
            //todo:完成报工的任务数量的计算方式 
            var completedCount = await taskQueryBySupplier.Where(o => o.EndDate != null).CountAsync();
            //延期上机任务数
            var delayStartCount = await taskQueryBySupplier.Where(o => o.AppointDate != null && ((o.StartDate != null && o.StartDate > o.AppointDate) || (o.StartDate == null && DateTime.Now > o.AppointDate))).CountAsync();
            //延期下机任务数
            var delayEndCount = await taskQueryBySupplier.Where(o => o.RequireDate != null && ((o.EndDate != null && o.EndDate > o.RequireDate) || (o.EndDate == null && DateTime.Now > o.RequireDate))).CountAsync();
            //超时任务数
            var overHourCount = await taskQueryBySupplier.Where(o => o.EstimateHours != null && o.ActualHours != null && o.ActualHours > o.EstimateHours).CountAsync();
            //不合格任务数
            var NGCount = await taskQueryBySupplier.Where(o => MESDbContext.GetJsonValueNumber(o.Property, "$.QuanlityType") == (int)QuanlityType.不合格).CountAsync();
            return new MESUnitReportRankDto()
            {
                UnitId = unitId,
                //UnitName = allUnits.Single(a => a.Id == unit.UnitId.Value).UnitName,
                TaskCount = taskCount,
                ReportCount = reportCount,
                ReportTaskCount = reportTaskCount,
                CompletedCount = completedCount,
                DelayStartCount = delayStartCount,
                DelayEndCount = delayEndCount,
                OverHourCount = overHourCount,
                NGCount = NGCount
            };
        }

        protected override async Task<IQueryable<Unit>> BuildSearchQueryAsync(IDictionary<string, string> searchKeys, IQueryable<Unit> query)
        {
            var result = await base.BuildSearchQueryAsync(searchKeys, query);

            if (searchKeys.ContainsKey("Binded"))
            {
                string binded = searchKeys["Binded"].ToString();
                //未绑定的
                if (binded == "0")
                {
                    result = result.Where(o => MasterDbContext.GetJsonValueNumber(o.Property, "$.TenantId") ==null);
                }
                else if (binded == "1")
                {
                    result = result.Where(o => MasterDbContext.GetJsonValueNumber(o.Property, "$.TenantId") > 0);
                }
            }

            return result;
        }

        #region 往来单位账套信息
        /// <summary>
        /// 获取往来单位有对应标记的接收openid
        /// </summary>
        /// <param name="unitId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public virtual async Task<List<string>> FindUnitOpenId(int unitId, string status = "")
        {

            var unit = await MESUnitManager.GetByIdAsync(unitId);
            return await MESUnitManager.FindUnitOpenId(unit, status);
        }

        /// <summary>
        /// 获取往来单位绑定的账套信息
        /// </summary>
        /// <param name="unitIds"></param>
        /// <returns></returns>
        public virtual async Task<object> GetUnitTenantInfos(int[] unitIds)
        {
            var units = await Manager.GetListByIdsAsync(unitIds);
            return units.Select(o => GetUnitTenantInfo(o));
        }
        /// <summary>
        /// 通过往来单位名称获取账套信息
        /// </summary>
        /// <param name="unitName"></param>
        /// <returns></returns>
        public virtual async Task<object> GetUnitTenantInfoByName(string unitName)
        {
            var unit = await Manager.GetAll().Where(o => o.UnitName == unitName).SingleOrDefaultAsync();
            if (unit == null)
            {
                throw new UserFriendlyException(L("往来单位不存在名称") + unitName);
            }
            else
            {
                return GetUnitTenantInfo(unit);
            }
        }

        private object GetUnitTenantInfo(Unit unit)
        {
            return new
            {
                unit.Id,
                unit.UnitName,
                TenantBinded = unit.IsTenantBinded()
            };
        }
        /// <summary>
        /// 获取往来单位绑定的提醒人
        /// </summary>
        /// <param name="unitId"></param>
        /// <returns></returns>
        public virtual async Task<List<object>> GetUnitBindedReminders(int unitId)
        {
            var result = new List<object>();
            var unit = await Manager.GetByIdAsync(unitId);
            var bindedPersonIds = unit.GetData<List<int>>("BindedPersonIds");
            if (bindedPersonIds != null && bindedPersonIds.Count > 0)
            {
                var persons = await Resolve<PersonManager>().GetListByIdsAsync(bindedPersonIds);
                result.AddRange(persons.Select(o => new { o.Id, o.Name }));
            }
            return result;
        }

        /// <summary>
        /// 移除往来单位提醒人
        /// </summary>
        /// <param name="unitId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task RemoveUnitReminder(int unitId, int id)
        {
            var unit = await Manager.GetByIdAsync(unitId);
            var bindedPersonIds = unit.GetData<List<int>>("BindedPersonIds");
            if (bindedPersonIds != null && bindedPersonIds.Contains(id))
            {
                bindedPersonIds.Remove(id);
                unit.SetData("BindedPersonIds", bindedPersonIds);
            }
        }
        #endregion

        /// <summary>
        /// 发送往来单位公告
        /// </summary>
        /// <param name="sendTenantNoticeDto"></param>
        /// <returns></returns>
        public virtual async Task SendNotice(SendTenantNoticeDto sendTenantNoticeDto)
        {
            //先产生一条公告
            var notice = new Notice()
            {
                NoticeTitle = sendTenantNoticeDto.Title,
                NoticeContent = sendTenantNoticeDto.Content,
                NoticeType = "往来单位公告",
                TenantId=AbpSession.TenantId
            };

            var noticeId=await NoticeManager.InsertAndGetIdAsync(notice);
            var units = await MESUnitManager.GetListByIdsAsync(sendTenantNoticeDto.UnitIds);

            await MESUnitManager.SendUnitsNotice(units, notice);
        }

        
    }
}
