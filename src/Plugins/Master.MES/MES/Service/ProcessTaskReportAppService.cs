using Abp.Authorization;
using Abp.Domain.Uow;
using Abp.UI;
using Abp.Web.Models;
using Master.Dto;
using Master.MES.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.MES.Service
{
    public class ProcessTaskReportAppService : MESAppServiceBase<ProcessTaskReport, int>
    {
        public PersonManager PersonManager { get; set; }
        protected override async  Task<IQueryable<ProcessTaskReport>> GetQueryable(RequestPageDto request)
        {
            var query =(await base.GetQueryable(request))
                .Where(o=>!o.ProcessTask.IsDeleted)
                .Include(o => o.ProcessTask).ThenInclude(o => o.Part).ThenInclude(o => o.Project)
                .Include(o => o.ProcessTask).ThenInclude(o => o.Supplier)
                .Include(o => o.Reporter)
                .Include(o => o.ProcessTask).ThenInclude(o => o.ProcessType);

            return query;
        }
        /// <summary>
        /// 分页返回报工记录
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public override async Task<ResultPageDto> GetPageResult(RequestPageDto request)
        {
            //显示已删除的记录
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.SoftDelete))
            {
                var query = await GetPageResultQueryable(request);
                var reports = await query.Queryable.ToListAsync();

                var data = reports.Select(o => {
                    return new
                    {
                        o.Id,
                        o.ProcessTask?.Part?.Project?.ProjectSN,
                        o.ProcessTask?.Part?.PartName,
                        o.ProcessTask?.Part?.PartSN,
                        o.ProcessTask?.Part?.PartSpecification,
                        o.ProcessTask?.Part?.PartNum,
                        o.ProcessTask?.ProcessSN,
                        o.ProcessTask?.Supplier?.UnitName,
                        o.ProcessTask?.ProcessType?.ProcessTypeName,
                        o.ProcessTask?.ProjectCharger,
                        TaskId = o.ProcessTask?.Id,
                        RequireDate = o.ProcessTask?.RequireDate?.ToString("yyyy-MM-dd"),
                        AppointDate = o.ProcessTask?.AppointDate?.ToString("yyyy-MM-dd"),
                        ReporterName = o.Reporter?.Name,
                        ReportType = o.ReportType.ToString(),
                        ReportTime = o.ReportTime.ToString("yyyy-MM-dd HH:mm:ss"),
                        CreationTime = o.CreationTime.ToString("yyyy-MM-dd HH:mm:ss"),
                        o.Remarks,
                        o.Files,
                        o.IsDeleted
                    };
                });

                var result = new ResultPageDto()
                {
                    code = 0,
                    count = query.RowCount,
                    data = data
                };

                return result;
            }
            

            
        }

        /// <summary>
        /// 获取报工详情，用于微信端显示报工详情
        /// </summary>
        /// <param name="reportId"></param>
        /// <returns></returns>
        [AbpAllowAnonymous]
        public virtual async Task<object> GetReportInfoById(int reportId)
        {
            using (UnitOfWorkManager.Current.DisableFilter(AbpDataFilters.MustHaveTenant,AbpDataFilters.SoftDelete))
            {
                var report = await Repository.GetAll().Where(o => o.Id == reportId)
                    .Include(o=>o.Tenant)
                    .Include(o=>o.Reporter)
                    .Include(o=>o.ProcessTask).ThenInclude(o=>o.Equipment)
                    .Include(o=>o.ProcessTask).ThenInclude(o=>o.ProcessTaskReports)
                    .Include(o => o.ProcessTask).ThenInclude(o => o.Part).ThenInclude(o => o.Project)
                    .SingleAsync();
                //var report = await Repository.GetAllIncluding(o => o.Tenant).Include(o => o.Reporter).Include(o => o.ProcessTask).ThenInclude(o => o.Part).ThenInclude(o => o.Project)
                //.Where(o => o.Id == reportId).SingleAsync();

                return new
                {
                    report.ProcessTask.PartId,
                    report.ProcessTaskId,
                    report.ProcessTask.ProcessSN,
                    report.ProcessTask.Part.PartName,
                    report.ProcessTask.Part.PartSpecification,
                    report.ProcessTask.Part.PartNum,
                    report.ProcessTask.ProcessType.ProcessTypeName,
                    report.ProcessTask.Tenant.TenancyName,
                    report.ProcessTask.Part.Project.ProjectSN,
                    report.ProcessTask.ProcessTaskStatus,
                    report.Reporter.Name,
                    report.ReportType,
                    report.Remarks,
                    ReportTime = report.ReportTime.ToString("yyyy-MM-dd HH:mm"),
                    report.Files,
                    report.ProcessTask.Part.PartImg,
                    CreationTime=report.CreationTime.ToString("yyyy-MM-dd HH:mm"),
                    report.ProcessTask.Poster,
                    report.ProcessTask.ProjectCharger,
                    report.ProcessTask.Supplier?.UnitName,
                    report.ProcessTask.Equipment?.EquipmentSN
                };
            }
            


            
        }

        #region 高级查询


        #region 获取基础模板数据
        public override List<SearchData> GetBaseSearchData()
        {
            List<SearchData> list = new List<SearchData>();

            #region 构建查询内容
            //零件
            var partSD = new SearchData()
            {
                Name = "零件",
                Keys = "PartSN,PartName",
                Model = "ProcessTask.Part",
                SearchType = SearchData.Like,
                CanAnd = false,
            };
            list.Add(partSD);

            //模具编号
            var mouldsnSD = new SearchData()
            {
                Name = "模具编号",
                Keys = "ProjectSN",
                Model = "ProcessTask.Part.Project",
                SearchType = SearchData.Search,
                CanAnd = false,
            };
            list.Add(mouldsnSD);

            //加工点
            var UnitSD = new SearchData()
            {
                Name = "加工点",
                Keys = "UnitName",
                Model = "ProcessTask.Supplier",
                SearchType = SearchData.Search,
                CanAnd = false,
            };
            list.Add(UnitSD);

            //工序
            var ProcessTypeSD = new SearchData()
            {
                Name = "工序",
                Keys = "ProcessTypeName",
                Model = "ProcessTask.ProcessType",
                SearchType = SearchData.Search,
                CanAnd = false,
            };
            list.Add(ProcessTypeSD);

            //计价方式
            var FeeTypeSD = new SearchData()
            {
                Name = "计价方式",
                Keys = "FeeType",
                Model = "ProcessTask",
                SearchType = SearchData.Search,
                CanAnd = false,
            };
            list.Add(FeeTypeSD);

            //上机时间
            var StartDateSD = new SearchData()
            {
                Name = "上机时间",
                Keys = "StartDate",
                Model = "ProcessTask",
                SearchType = SearchData.Date,
                CanAnd = true,
            };
            list.Add(StartDateSD);

            //下机时间
            var EndDateSD = new SearchData()
            {
                Name = "下机时间",
                Keys = "EndDate",
                Model = "ProcessTask",
                SearchType = SearchData.Date,
                CanAnd = true,
            };
            list.Add(EndDateSD);

            //工时
            var ActualHoursSD = new SearchData()
            {
                Name = "工时",
                Keys = "ActualHours",
                Model = "ProcessTask",
                SearchType = SearchData.Array,
                CanAnd = false,
                ArrayData = new List<string>() { "有", "无" },
            };
            list.Add(ActualHoursSD);

            //创建时间
            var CreationTimeSD = new SearchData()
            {
                Name = "创建时间",
                Keys = "CreationTime",
                Model = "ProcessTask",
                SearchType = SearchData.Date,
                CanAnd = true,
            };
            list.Add(CreationTimeSD);

            //开单人
            var PosterSD = new SearchData()
            {
                Name = "开单人",
                Keys = "Poster",
                Model = "ProcessTask",
                SearchType = SearchData.Search,
                CanAnd = false,
            };
            list.Add(PosterSD);

            //模具组长
            var ProjectChargerSD = new SearchData()
            {
                Name = "模具组长",
                Keys = "ProjectCharger",
                Model = "ProcessTask",
                SearchType = SearchData.Search,
                CanAnd = false,
            };
            list.Add(ProjectChargerSD);

            //工艺师
            var CraftsManSD = new SearchData()
            {
                Name = "工艺师",
                Keys = "CraftsMan",
                Model = "ProcessTask",
                SearchType = SearchData.Search,
                CanAnd = false,
            };
            list.Add(CraftsManSD);

            //审核
            var VerifierSD = new SearchData()
            {
                Name = "审核人",
                Keys = "Verifier",
                Model = "ProcessTask",
                SearchType = SearchData.Search,
                CanAnd = false,
            };
            list.Add(VerifierSD);

            //检验
            var CheckerSD = new SearchData()
            {
                Name = "检验人",
                Keys = "Checker",
                Model = "ProcessTask",
                SearchType = SearchData.Search,
                CanAnd = false,
            };
            list.Add(CheckerSD);

            #endregion

            return list;
        }
        #endregion


        #region 获取模块名称（model|key）
        /// <summary>
        /// 得到当前模块标记
        /// </summary>
        /// <returns></returns>
        public virtual string GetModelName()
        {
            return "ProcessTaskReport";
        }

        #endregion

        #endregion

        public virtual async Task UpdateField(int processTaskReportId, string field, string value)
        {
            var report = await Manager.GetAll()
                .Include(o=>o.Reporter)
                .Where(o=>o.Id==processTaskReportId).SingleOrDefaultAsync();

            if (report == null)
            {
                throw new UserFriendlyException(L("失效数据不能修改"));
            }

            switch (field)
            {
                case "reporterName":
                    report.Reporter.Name = value;
                    break;
            }
        }

        public async override Task DeleteEntity(IEnumerable<int> ids)
        {
            if (ids.Count() > 1)
            {
                throw new UserFriendlyException(L("一次只允许删除一条报工记录"));
            }
            await base.DeleteEntity(ids);
        }
        [DontWrapResult]
        [HttpGet]
        public async Task<ResultPageDto> ReportNum(RequestPageDto requestPageDto)
        {
            DateTime startDate = Convert.ToDateTime("2018-01-01");
            DateTime endDate = DateTime.Now;
            try
            {
                var searchKeys = Newtonsoft.Json.JsonConvert.DeserializeObject<IDictionary<string, string>>(requestPageDto.SearchKeys);
                startDate = Convert.ToDateTime(searchKeys["startDate"]);
                endDate = Convert.ToDateTime(searchKeys["endDate"]);
            }
            catch
            {
               
            }
           
            var a = Manager.GetAll()
                .Where(o=>o.ReportTime > startDate)
                 .Where(o => o.ReportTime < endDate)
                  .GroupBy(o =>  o.Reporter.Id );
            
            var b = a.Select(o => new {
                nameid =o.Key,
                count =o.Count(),
                report1Count =o.Where(r=>r.ReportType==ReportType.到料).Count(),
                report2Count = o.Where(r => r.ReportType == ReportType.上机).Count(),
                report3Count = o.Where(r => r.ReportType == ReportType.加工).Count(),
                report4Count = o.Where(r => r.ReportType == ReportType.暂停).Count(),
                report5Count = o.Where(r => r.ReportType == ReportType.下机).Count(),
                report6Count = o.Where(r => r.ReportType == ReportType.重新开始).Count(),
            })
                //.OrderByDescending(o => o.count)
                .Skip((requestPageDto.Page - 1) * requestPageDto.Limit)
                .Take(requestPageDto.Limit)
                .ToList();

            var result = new List<object>();

            foreach(var obj in b)
            {
                var name = (await PersonManager.GetByIdFromCacheAsync(obj.nameid)).Name;
                var lastReport =await Manager.GetAll()
                    .Include(o=>o.Tenant)
                    .Where(o => o.ReporterId == obj.nameid).LastAsync();
                result.Add(new
                {
                    name,
                    obj.count,
                    obj.report1Count,
                    obj.report2Count,
                    obj.report3Count,
                    obj.report4Count,
                    obj.report5Count,
                    obj.report6Count,
                    tenantName = lastReport.Tenant.Name
                });
            }

            return new ResultPageDto()
            {
                code = 0,
                msg = "",
                data = result

            };
        }
    }
}
