using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Events.Bus.Entities;
using Abp.Events.Bus.Handlers;
using Abp.Runtime.Caching;
using Castle.Core.Logging;
using Master.MES.Jobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Master.MES.Events
{
    /// <summary>
    /// 加工任务相关事件
    /// </summary>
    public class ProcessTaskEventHandler :
        IEventHandler<EntityDeletedEventData<ProcessTask>>,
        IEventHandler<EntityChangedEventData<ProcessTask>>,
        ITransientDependency
    {
        public ProcessTaskReportManager ProcessTaskReportManager { get; set; }
        public MESProjectManager MESProjectManager { get; set; }
        public ProcessTaskManager ProcessTaskManager { get; set; }
        public PartManager PartManager { get; set; }
        public ICacheManager CacheManager { get; set; }
        public ILogger Logger { get; set; }
        public IBackgroundJobManager BackgroundJobManager { get; set; }
        /// <summary>
        /// 当任务被删除后
        /// </summary>
        /// <param name="eventData"></param>
        [UnitOfWork]
        public virtual void HandleEvent(EntityDeletedEventData<ProcessTask> eventData)
        {
            //同时删除任务的报工记录
            ProcessTaskReportManager.Repository.Delete(o => o.ProcessTaskId == eventData.Entity.Id);
        }
        /// <summary>
        /// 当加工任务变化时，清除对应的项目生产汇总数据缓存
        /// </summary>
        /// <param name="eventData"></param>
        [UnitOfWork]
        public void HandleEvent(EntityChangedEventData<ProcessTask> eventData)
        {
            try
            {
                var partId = eventData.Entity.PartId;
                var projectId = PartManager.GetByIdFromCacheAsync(partId).Result.ProjectId;
                var cacheKey = $"{projectId}@{eventData.Entity.TenantId}";
                CacheManager.GetCache<string, Dictionary<string, object>>("ProjectProcessSummary").Remove(cacheKey);

                //判断后台任务中是否有在进行此项目的进度同步
                if (!SyncProjectScheduleJobs.QueuedProject.Contains(projectId))
                {
                    var args = new SyncProjectScheduleJobsArg()
                    {
                        ProjectId = projectId,
                        ReNew = false
                    };
                    BackgroundJobManager.Enqueue<SyncProjectScheduleJobs, SyncProjectScheduleJobsArg>(args);
                    SyncProjectScheduleJobs.QueuedProject.Add(projectId);
                }
                
            }
            catch(Exception ex)
            {
                
            }
           
        }
    }
}
