using Abp.UI;
using Master.Entity;
using Master.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Runtime.Caching;
using System.Linq.Dynamic.Core;
using Master.EntityFrameworkCore;
using Master.Domain;
using Master.Search;
using Microsoft.EntityFrameworkCore;
using Master.Scheduler.Domains;
using Abp.Domain.Repositories;
using Z.EntityFramework.Plus;
using Master.Authentication;

namespace Master.MES
{
    /// <summary>
    /// MES的项目管理类，重写底层项目管理
    /// </summary>
    public class MESProjectManager : ProjectManager
    {
        //public UserManager UserManager { get; set; }
        //public PartManager PartManager { get; set; }
        //public ProcessTaskManager ProcessTaskManager { get; set; }
        //public ProcessTypeManager ProcessTypeManager { get; set; }
        //public ISchedulerTaskManager SchedulerTaskManager { get; set; }
        public IDynamicQuery DynamicQuery { get; set; }
        public IDynamicSearchParser DynamicSearchParser { get; set; }

        #region 重写删除方法
        public async override Task DeleteAsync(IEnumerable<int> ids)
        {
            //已被使用的项目无法删除
            if (await Resolve<PartManager>().Repository.CountAsync(o => ids.Contains(o.ProjectId)) > 0)
            {
                throw new UserFriendlyException("有项目已被生产使用,无法删除");
            }
            await base.DeleteAsync(ids);
        }

        public async override Task DeleteAsync(Project entity)
        {
            //已被使用的项目无法删除
            if (await Resolve<PartManager>().Repository.CountAsync(o => o.ProjectId == entity.Id) > 0)
            {
                throw new UserFriendlyException("此项目已被生产使用,无法删除");
            }
            await base.DeleteAsync(entity);
        }
        #endregion

        #region 项目统计汇总
        /// <summary>
        /// 缓存方式获取项目的生产汇总数据
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public async  Task<Dictionary<string,object>> GetProjectProcessSummary(Project project)
        {
            var cacheKey = $"{project.Id}@{project.TenantId}";
            return await CacheManager.GetCache<string, Dictionary<string, object>>("ProjectProcessSummary").GetAsync(cacheKey,async o => {
                var processTypeManager = Resolve<ProcessTypeManager>();
                var processTaskManager = Resolve<ProcessTaskManager>();
                var dic = new Dictionary<string, object>()
                {
                     {"Id",project.Id},
                    { "ProjectSN",project.ProjectSN},
                    { "ProjectCharger",project.GetPropertyValue<string>("ProjectCharger")},
                    {"ProjectName",project.ProjectName }
                };
                var processTypes = await processTypeManager.GetAllList();

                var feeTotal = 0M;
                var innerFeeTotal = 0M;
                var outerFeeTotal = 0M;
                var hoursTotal = 0M;
                var taskNumberTotal = 0;
                var NGNumberTotal = 0;
                var delayNumberTotal = 0;
                foreach (var processType in processTypes)
                {
                    var taskQuery = processTaskManager.GetAll()
                        .Where(t => t.Part.ProjectId == project.Id && t.ProcessTypeId == processType.Id && t.ProcessTaskStatus != ProcessTaskStatus.Inputed);
                    //强制回单的增加查询条件
                    //if (mustReturnFile)
                    //{
                    //    taskQuery = taskQuery.Where($"Status!=null and Status.Object.Contains(\"{ProcessTask.Status_Verify}\")");
                    //}
                    // .WhereIf(mustReturnFile, t =>t.Status!=null && t.Status.Object!=null && t.Status.Object.Contains(ProcessTask.Status_Verify))
                    //获取实际金额
                    var fee = taskQuery.Sum(t => t.Fee);
                    feeTotal += fee ?? 0;
                    dic.Add("ProcessType_" + processType.Id + "_Fee", fee?.ToString("0.00"));
                    //厂内金额
                    var innerFee = taskQuery.Where($"Status.Contains(\"{ProcessTask.Status_Inner}\")").Sum(t => t.Fee);
                    innerFeeTotal += innerFee ?? 0;
                    dic.Add("ProcessType_" + processType.Id + "_InnerFee", innerFee?.ToString("0.00"));
                    //厂外金额
                    outerFeeTotal += ((fee ?? 0) - (innerFee ?? 0));
                    dic.Add("ProcessType_" + processType.Id + "_OuterFee", ((fee ?? 0) - (innerFee ?? 0)).ToString("0.00"));
                    //实际工时
                    var hours = taskQuery.Sum(t => t.ActualHours);
                    hoursTotal += hours ?? 0;
                    dic.Add("ProcessType_" + processType.Id + "_ActualHours", hours?.ToString("0.00"));
                    //总数
                    var taskNumber = taskQuery.Count();
                    taskNumberTotal += taskNumber;
                    dic.Add("ProcessType_" + processType.Id + "_TaskNumber", taskNumber.ToString());
                    //不合格数
                    var NGNumber = taskQuery.Where(t => MESDbContext.GetJsonValueNumber(t.Property, "$.QuanlityType") == 2).Count();
                    NGNumberTotal += NGNumber;
                    dic.Add("ProcessType_" + processType.Id + "_NGNumber", NGNumber.ToString());
                    //延迟数
                    //var delayNumber = taskQuery.Where("(Convert.ToDateTime(RequireDate)-Convert.ToDateTime(EndDate==null?DateTime.Now.ToString():EndDate.ToString())).TotalDays<0  and RequireDate!=null").Count();
                    //貌似不能直接用ef core查询
                    var delayNumber = await DynamicQuery.SingleAsync<int?>($"select sum(datediff( case when enddate is null then NOW() else enddate end,requiredate)) from {nameof(ProcessTask)} where requiredate is not null and datediff( case when enddate is null then NOW() else enddate end,requiredate)>0 and ProcessTaskStatus!=0 and processTypeId={processType.Id} and partid in (select id from part where projectid={project.Id} and isdeleted=0) and isdeleted=0");
                    //var delayNumber = taskQuery.Where("(Convert.ToDateTime(RequireDate)-Convert.ToDateTime(EndDate)).TotalDays<0  and RequireDate!=null and EndDate!=null").Sum(o=> (o.EndDate.Value-o.StartDate.Value).TotalDays);
                    delayNumberTotal += delayNumber ?? 0;
                    dic.Add("ProcessType_" + processType.Id + "_DelayNumber", delayNumber.ToString());
                }
                dic.Add("FeeTotal", feeTotal.ToString("0.00"));
                dic.Add("InnerFeeTotal", innerFeeTotal.ToString("0.00"));
                dic.Add("OuterFeeTotal", outerFeeTotal.ToString("0.00"));
                dic.Add("HoursTotal", hoursTotal.ToString("0.00"));
                dic.Add("TaskNumberTotal", taskNumberTotal.ToString());
                dic.Add("NGNumberTotal", NGNumberTotal.ToString());
                dic.Add("DelayNumberTotal", delayNumberTotal.ToString("0"));

                return dic;
            });
        }
        #endregion

        #region 项目数据过滤
        public override IQueryable<Project> GetAll()
        {
            var query = base.GetAll();
            //如果没有查看全部权限
            if (!PermissionChecker.IsGrantedAsync("Module.Project.Button.ShowAll").Result)
            {
                var user = Resolve<UserManager>().GetByIdAsync(AbpSession.UserId.Value).Result;
                var statuses = new string[] { "ProjectCharger", "ProjectTracker", "ProductDesign", "MouldDesign", "Salesman" };
                var conditionStr = new List<string>();
                conditionStr.Add(" 1>2 ");//先插入查询条件,即如果没有任何标记看不到任何模具
                var funcList = new List<object>();
                for (var i = 0; i < statuses.Length; i++)
                {                    
                    if (user.HasStatus(statuses[i]))
                    {
                        conditionStr.Add($" @{funcList.Count()}(it)=\"{user.Name}\" ");
                        var dbLamda = DynamicSearchParser.GeneratePropertyLamda<Project>(Module.ColumnTypes.Text, statuses[i]);
                        funcList.Add(dbLamda);
                    }
                }
                query = query.Where(string.Join(" or ", conditionStr), funcList.ToArray());
            }

            return query;
        }
        #endregion

        #region 同步进度数据
        /// <summary>
        /// 从生产任务中同步数据至项目进度
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="renew"></param>
        /// <returns></returns>
        public virtual async Task SyncProcessSchedule(int projectId,bool renew=false)
        {
            var project = await GetByIdFromCacheAsync(projectId);
            var schedulerTaskManager = Resolve<SchedulerTaskManager>();
            var partManager = Resolve<PartManager>();

            using (CurrentUnitOfWork.SetTenantId(project.TenantId))
            {
                if (renew)
                {
                    //删除原有生产进度
                    await schedulerTaskManager.Repository.HardDeleteAsync(o => o.ProjectId == projectId && o.RelGroup == "Process");
                    await CurrentUnitOfWork.SaveChangesAsync();
                }
                //获取项目下的所有生产零件
                var parts = await partManager.GetAll()
                    .Where(o => o.ProjectId == projectId)
                    .Where(o => o.EnableProcess)
                    .OrderBy(o => o.Sort)
                    .Include("ProcessTasks.ProcessType")
                    .ToListAsync();
                //项目下的所有任务
                var oriTasks = await schedulerTaskManager.GetProjectSchedulerTasks(projectId);

                //1.生成生产进度节点
                var processWrapperTask = oriTasks
                    .Where(o => o.ParentId == "0")
                    .Where(o => o.RelGroup == "Process")
                    .Where(o => o.TaskName == "生产进度")
                    .FirstOrDefault();
                if (processWrapperTask == null)
                {
                    processWrapperTask = new SchedulerTask()
                    {
                        ProjectId = projectId,
                        TaskName = "生产进度",
                        TaskType = "project",
                        PlanStartDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")),
                        PlanEndDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")).AddDays(1),
                        ParentId = "0",
                        RelGroup = "Process",
                        IsRelativeTask = true,
                        TenantId=project.TenantId,
                        TaskId = Guid.NewGuid().ToString()
                    };
                    await schedulerTaskManager.InsertAsync(processWrapperTask);
                }
                //2.生成零件节点
                //删除不存在的零件
                //oriTasks.Where(o => o.GetPropertyValue<int?>("PartId") != null && !parts.Select(p => p.Id).ToList().Contains(o.GetPropertyValue<int>("PartId")))
                //    .ToList()
                //    .ForEach(o => SchedulerTaskManager.Repository.HardDelete(o));
                foreach (var part in parts)
                {
                    var partTask = oriTasks.Where(o => o.ParentId == processWrapperTask.TaskId)
                        .Where(o => o.RelGroup == "Process" && o.RelType == "Part")
                        .Where(o => o.RelData == part.Id.ToString())
                        .FirstOrDefault();
                    if (partTask == null)
                    {
                        partTask = new SchedulerTask()
                        {
                            ProjectId = projectId,
                            ParentId = processWrapperTask.TaskId,
                            IsRelativeTask = true,
                            RelGroup = "Process",
                            RelType = "Part",
                            RelData = part.Id.ToString(),
                            TaskId = Guid.NewGuid().ToString(),
                            TenantId = project.TenantId,
                        };
                        await schedulerTaskManager.InsertAsync(partTask);
                    }
                    partTask.TaskName = part.PartName;
                    partTask.TaskType = "project";
                    if (part.ProcessTasks.Count > 0)
                    {
                        partTask.PlanStartDate = part.ProcessTasks.Min(o => o.SchedulePlanStartDate);
                        partTask.PlanEndDate = part.ProcessTasks.Max(o => o.SchedulePlanEndDate);
                    }


                    //3.生成任务节点
                    foreach (var processInfo in part.ProcessTasks)
                    {
                        var processTask = oriTasks.Where(o => o.ParentId == partTask.TaskId)
                            .Where(o => o.RelGroup == "Process" && o.RelType == "ProcessTask")
                            .Where(o => o.RelData == processInfo.Id.ToString())
                            .FirstOrDefault();
                        if (processTask == null)
                        {
                            processTask = new SchedulerTask()
                            {
                                ProjectId = projectId,
                                TaskType = "task",
                                ParentId = partTask.TaskId,
                                IsRelativeTask = true,
                                RelGroup = "Process",
                                RelType = "ProcessTask",
                                RelData = processInfo.Id.ToString(),
                                TaskId = Guid.NewGuid().ToString(),
                                TenantId = project.TenantId,
                            };
                            await schedulerTaskManager.InsertAsync(processTask);
                        }
                        processTask.TaskName = processInfo.ProcessType.ProcessTypeName;
                        processTask.PlanStartDate = processInfo.SchedulePlanStartDate;
                        processTask.PlanEndDate = processInfo.SchedulePlanEndDate;
                        processTask.StartDate = processInfo.StartDate;
                        processTask.EndDate = processInfo.EndDate;
                        processTask.Progress = processInfo.Progress;
                    }
                }
            }
            
        }
        #endregion

    }

    
}
