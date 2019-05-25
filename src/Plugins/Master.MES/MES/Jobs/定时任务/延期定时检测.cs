using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.Threading.BackgroundWorkers;
using Abp.Threading.Timers;
using Master.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Master.MES.Jobs
{
    public class CheckDelayJobs : PeriodicBackgroundWorkerBase, ISingletonDependency
    {
        private readonly ProcessTaskManager _processTaskManager;
        private readonly TacticManager _tacticManager;
        private readonly IBackgroundJobManager _backgroundJobManager;
        public RemindLogManager RemindLogManager { get; set; }
        public CheckDelayJobs(AbpTimer timer, ProcessTaskManager processTaskManager, TacticManager tacticManager,
           IBackgroundJobManager backgroundJobManager)
            : base(timer)
        {
            Timer.Period = 60*1000*60*2; //2小时执行一次
            _processTaskManager = processTaskManager;
            _tacticManager = tacticManager;
            _backgroundJobManager = backgroundJobManager;
        }

        [UnitOfWork]
        protected override void DoWork()
        {
            //只在工作时间执行
            var hour = DateTime.Now.Hour;
            if (hour < 8 || hour > 18)
            {
                return;
            }
            Logger.Info("开始延期任务检测");
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant,AbpDataFilters.MustHaveTenant))
            {
                var taskQuery = _processTaskManager.GetAll().Include(o => o.Part).ThenInclude(o => o.Project);
                //获取所有延期未上机，延期未下机，到料后未上机,工时超预计的任务
                var delayStartTasks = taskQuery.Where(o => o.StartDate == null && o.AppointDate != null && o.AppointDate.Value < DateTime.Now).ToList();
                SendDelayTaskMessage(delayStartTasks, DelayType.DelayStart);
                var delayEndTasks= taskQuery.Where(o => o.EndDate == null && o.RequireDate != null && o.RequireDate.Value < DateTime.Now).ToList();
                SendDelayTaskMessage(delayEndTasks, DelayType.DelayEnd);
                var receivedNotStartTasks= taskQuery.Where(o => o.StartDate == null && o.ReceiveDate != null && o.AppointDate.Value < DateTime.Now).ToList();
                SendDelayTaskMessage(receivedNotStartTasks, DelayType.ReceiveNotStart);
                var exceedHourTasks = taskQuery.Where(o => o.StartDate!=null && o.EstimateHours!=null && o.EndDate==null && (DateTime.Now- Convert.ToDateTime(o.StartDate)).TotalHours>Convert.ToDouble(o.EstimateHours.Value)).ToList();
                SendDelayTaskMessage(exceedHourTasks, DelayType.ExceedHour);
                CurrentUnitOfWork.SaveChanges();
            }
        }
        private void SendDelayTaskMessage(List<ProcessTask> processTasks,DelayType delayType)
        {
            foreach (var task in processTasks)
            {
                var remindInfos = _tacticManager.GetRemindPersonByDelayTask(task, delayType).GetAwaiter().GetResult();
                foreach (var remindInfo in remindInfos)
                {
                    var remindPerson = remindInfo.Person;
                    //如果提醒间隔小于1天，则不再提醒
                    var count = RemindLogManager.GetAll().Where(o => o.RemindType == "延期提醒" && o.Name == remindPerson.Name && o.TenantId == task.TenantId && o.Message == (task.ProcessSN + "-" + delayType.ToString()) && (o.CreationTime - DateTime.Now).TotalDays <1).Count();
                    if (count == 0)
                    {
                        var openid = remindPerson.GetPropertyValue<string>("OpenId");
                        //先产生一条提醒记录
                        var remindLog = new RemindLog()
                        {
                            RemindType = "延期提醒",
                            Name = remindPerson.Name,
                            TenantId = task.TenantId,
                            Message = task.ProcessSN + "-" + delayType.ToString()
                        };
                        var timespan = remindInfo.GetAvailableRemindTimeSpan();
                        if (timespan.TotalMinutes > 0)
                        {
                            remindLog.SetPropertyValue("errMsg", $"延迟{timespan.TotalMinutes.ToString("0")}分钟后运行");
                        }
                        var remindLogId = RemindLogManager.InsertAndGetIdAsync(remindLog).Result;
                        var arg = new SendWeiXinMessageJobArgs()
                        {
                            OpenId = openid,
                            DataId = task.Id,
                            RemindLogId = remindLogId,
                            ExtendInfo = delayType.ToString()
                        };

                        
                        _backgroundJobManager.Enqueue<SendDelayTaskWeiXinMessageJob, SendWeiXinMessageJobArgs>(arg,BackgroundJobPriority.Normal, timespan);

                    }
                    
                }
            }
        }
    }
}
