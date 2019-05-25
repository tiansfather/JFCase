using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Collections.Extensions;
using Abp.Web.Models;
using Master.Domain;
using Master.Dto;
using Master.Entity;
using Master.EntityFrameworkCore;
using Master.MES.Dtos;
using Master.Projects;
using Master.Scheduler.Domains;
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
    public class MESProjectAppService:ProjectAppService
    {
        public MESProjectManager MESProjectManager { get; set; }
        public ProcessTypeManager ProcessTypeManager { get; set; }
        public ProcessTaskManager ProcessTaskManager { get; set; }
        public ProcessTypeAppService ProcessTypeAppService { get; set; }
        public IDynamicQuery DynamicQuery { get; set; }

        //protected override async Task<IQueryable<Project>> BuildSearchQueryAsync(IDictionary<string, string> searchKeys, IQueryable<Project> query)
        //{
        //    //var myQuery = await base.BuildSearchQueryAsync(searchKeys, query);

        //    IQueryable<Project> myQuery;

        //    var mytask = ProcessTaskManager.GetAll();
        //    if (searchKeys.ContainsKey("inner"))
        //    {
        //         myQuery = from project in query
        //                  join procesTask in mytask on project.ProjectSN equals procesTask.Part.Project.ProjectSN
        //                   where
        //                  select porject
        //    }
        //    return myQuery;
        //}
        /// <summary>
        /// 获取模具组长信息
        /// </summary>
        /// <returns></returns>
        public virtual async Task<object> GetProjectChargers()
        {
            return await DynamicQuery.SelectAsync<dynamic>($"SELECT property->>\"$.ProjectCharger\" as Name,count(0) as ProjectCount from project where case when property->>\"$.ProjectCharger\"='null' then NULL else property->>\"$.ProjectCharger\" end   is not null and tenantId={AbpSession.TenantId.Value} and isdeleted=0 group by property->>\"$.ProjectCharger\"");
            

            //var projectChargers = await Manager.GetAll()
            //    .Where(o => o.Status.Contains("ProjectCharger"))
            //    .Select(o => o.Name)
            //    .ToListAsync();

            //var result = new List<object>();
            //foreach(var projectCharger in projectChargers)
            //{
            //    var projectCount = await MESProjectManager.GetAll()
            //        .Where(o => MESDbContext.GetJsonValueString(o.Property, "$.ProjectCharger") == projectCharger)
            //        .CountAsync();

            //    result.Add(new { Name = projectCharger, ProjectCount = projectCount });
            //}

            //return result;
        }
        /// <summary>
        /// 获取项目的生产报表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [DontWrapResult]
        public virtual async Task<ResultPageDto> GetProcessSummaryPageResult(RequestPageDto request)
        {
            var query = await GetPageResultQueryable(request);

            var processTypes = await ProcessTypeAppService.GetUsedProcessTypes();

            var projects = await query.Queryable.ToListAsync();

            //var data = projects.Select(o => {
            //    var dic = new Dictionary<string, object>()
            //    {
            //         {"Id",o.Id},
            //        { "ProjectSN",o.ProjectSN},
            //        {"ProjectName",o.ProjectName }
            //    };
            //    return dic;
            //});
            var data = new List<Dictionary<string, object>>();
            var manager = Manager as MESProjectManager;
            //是否强制回单
            //var mustReturnFile = await SettingManager.GetSettingValueAsync(Master.Configuration.MESSettingNames.MustReturnFileBeforeCheck) == "true";
            foreach (var project in projects)
            {
                var dic = await manager.GetProjectProcessSummary(project);
                #region 原有非缓存方式代码

                //var dic = new Dictionary<string, object>()
                //{
                //     {"Id",project.Id},
                //    { "ProjectSN",project.ProjectSN},
                //    {"ProjectName",project.ProjectName }
                //};
                ////var dic = data.Single(o => Convert.ToInt32(o["Id"]) == project.Id);
                //var feeTotal = 0M;
                //var innerFeeTotal = 0M;
                //var outerFeeTotal = 0M;
                //var hoursTotal = 0M;
                //var taskNumberTotal = 0;
                //var NGNumberTotal = 0;
                //var delayNumberTotal = 0M;
                //foreach (var processType in processTypes)
                //{
                //    var taskQuery = ProcessTaskManager.GetAll()
                //        .Where(t => t.Part.ProjectId == project.Id && t.ProcessTypeId == processType.Id && t.ProcessTaskStatus!=ProcessTaskStatus.Inputed);
                //    //强制回单的增加查询条件
                //    //if (mustReturnFile)
                //    //{
                //    //    taskQuery = taskQuery.Where($"Status!=null and Status.Object.Contains(\"{ProcessTask.Status_Verify}\")");
                //    //}
                //       // .WhereIf(mustReturnFile, t =>t.Status!=null && t.Status.Object!=null && t.Status.Object.Contains(ProcessTask.Status_Verify))
                //    //获取实际金额
                //    var fee = taskQuery.Sum(t => t.Fee);
                //    feeTotal += fee??0;
                //    dic.Add("ProcessType_" + processType.Id+"_Fee", fee?.ToString("0.00"));
                //    //厂内金额
                //    var innerFee = taskQuery.Where($"Status!=null and Status.Object.Contains(\"{ProcessTask.Status_Inner}\")").Sum(t => t.Fee);
                //    innerFeeTotal += innerFee ?? 0;
                //    dic.Add("ProcessType_" + processType.Id + "_InnerFee", innerFee?.ToString("0.00"));
                //    //厂外金额
                //    outerFeeTotal += ((fee ?? 0) - (innerFee ?? 0));
                //    dic.Add("ProcessType_" + processType.Id + "_OuterFee", ((fee ?? 0) - (innerFee ?? 0)).ToString("0.00"));
                //    //实际工时
                //    var hours = taskQuery.Sum(t => t.ActualHours);
                //    hoursTotal += hours ?? 0;
                //    dic.Add("ProcessType_" + processType.Id+"_ActualHours", hours?.ToString("0.00"));
                //    //总数
                //    var taskNumber = taskQuery.Count();
                //    taskNumberTotal += taskNumber;
                //    dic.Add("ProcessType_" + processType.Id + "_TaskNumber", taskNumber.ToString());
                //    //不合格数
                //    var NGNumber = taskQuery.Where(t => MESDbContext.GetJsonValueNumber(t.Property, "$.QuanlityType") == 2).Count();
                //    NGNumberTotal += NGNumber;                    
                //    dic.Add("ProcessType_" + processType.Id + "_NGNumber", NGNumber.ToString());
                //    //延迟数
                //    //var delayNumber = taskQuery.Where("(Convert.ToDateTime(RequireDate)-Convert.ToDateTime(EndDate==null?DateTime.Now.ToString():EndDate.ToString())).TotalDays<0  and RequireDate!=null").Count();
                //    //貌似不能直接用ef core查询
                //    var delayNumber =await DynamicQuery.SingleAsync<int?>($"select sum(datediff( case when enddate is null then NOW() else enddate end,requiredate)) from {nameof(ProcessTask)} where requiredate is not null and datediff( case when enddate is null then NOW() else enddate end,requiredate)>0 and ProcessTaskStatus!=0 and processTypeId={processType.Id} and partid in (select id from part where projectid={project.Id} and isdeleted=0) and isdeleted=0");
                //    //var delayNumber = taskQuery.Where("(Convert.ToDateTime(RequireDate)-Convert.ToDateTime(EndDate)).TotalDays<0  and RequireDate!=null and EndDate!=null").Sum(o=> (o.EndDate.Value-o.StartDate.Value).TotalDays);
                //    delayNumberTotal += delayNumber??0;
                //    dic.Add("ProcessType_" + processType.Id + "_DelayNumber", delayNumber.ToString());
                //}
                //dic.Add("FeeTotal", feeTotal.ToString("0.00"));
                //dic.Add("InnerFeeTotal", innerFeeTotal.ToString("0.00"));
                //dic.Add("OuterFeeTotal", outerFeeTotal.ToString("0.00"));
                //dic.Add("HoursTotal", hoursTotal.ToString("0.00"));
                //dic.Add("TaskNumberTotal", taskNumberTotal.ToString());
                //dic.Add("NGNumberTotal", NGNumberTotal.ToString());
                //dic.Add("DelayNumberTotal", delayNumberTotal.ToString("0.00"));

                #endregion
                data.Add(dic);
            }

            

            var result = new ResultPageDto()
            {
                code = 0,
                count = query.RowCount,
                data = data
            };

            return result;
        }

        protected override async Task<IQueryable<Project>> BuildKeywordQueryAsync(string keyword, IQueryable<Project> query)
        {
            return query.Where(o=>o.ProjectSN.Contains(keyword) || MasterDbContext.GetJsonValueString(o.Property,"$.ProjectCharger").Contains(keyword));
        }

        /// <summary>
        /// 通过模具编号获取模具信息
        /// </summary>
        /// <param name="projectSN"></param>
        /// <returns></returns>
        public virtual async Task<object> GetProjectInfo(string projectSN)
        {
            //任何人都可以查询此接口里的项目
            //不使用Manager.GetAll，以避免项目过滤造成的问题
            var project = await Manager.Repository.GetAll().Where(o => o.ProjectSN == projectSN).SingleOrDefaultAsync();
            return new
            {
                project.Id,
                project.ProjectSN,
                project.ProjectName,
                ProjectCharger=project.GetPropertyValue<string>("ProjectCharger"),
                ProjectTracker = project.GetPropertyValue<string>("ProjectTracker"),
                ProductDesign = project.GetPropertyValue<string>("ProductDesign"),
                MouldDesign = project.GetPropertyValue<string>("MouldDesign"),
                Salesman = project.GetPropertyValue<string>("Salesman"),
                T0Date= project.GetPropertyValue<string>("T0Date"),
            };
        }

        #region 高级查询


        #region 获取基础模板数据
        public  List<SearchData> GetBaseSearchData()
        {
            List<SearchData> list = new List<SearchData>();

            #region 构建查询内容

            //上机时间
            var StartDateSD = new SearchData()
            {
                Name = "创建时间",
                Keys = "CreationTime",
                Model = "MESProject",
                SearchType = SearchData.Date,
                CanAnd = true,
            };
            list.Add(StartDateSD);
            
            

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
            return "MESProject";
        }

        #endregion

        #endregion

        #region 进度
        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="renew"></param>
        /// <returns></returns>
        public virtual async Task SyncProcessSchedule(int projectId,bool renew=false)
        {
            await MESProjectManager.SyncProcessSchedule(projectId,renew);
        }
        #endregion
    }
}
