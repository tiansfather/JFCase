using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.Domain.Uow;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.MES.Jobs
{
    /// <summary>
    /// 项目进度同步任务
    /// </summary>
    public class SyncProjectScheduleJobs : BackgroundJob<SyncProjectScheduleJobsArg>, ITransientDependency
    {
        //用于记录当前正在同步的项目Id
        public static List<int> QueuedProject = new List<int>();
        public MESProjectManager MESProjectManager { get; set; }
        [UnitOfWork]
        public override void Execute(SyncProjectScheduleJobsArg args)
        {
            MESProjectManager.SyncProcessSchedule(args.ProjectId, args.ReNew).GetAwaiter().GetResult();
            //移除项目Id
            if (QueuedProject.Contains(args.ProjectId))
            {
                QueuedProject.Remove(args.ProjectId);
            }
        }
    }

    public class SyncProjectScheduleJobsArg
    {
        public int ProjectId { get; set; }
        public bool ReNew { get; set; }
    }
}
