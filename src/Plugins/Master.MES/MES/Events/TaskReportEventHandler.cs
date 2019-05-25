using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.Events.Bus.Entities;
using Abp.Events.Bus.Handlers;
using Castle.Core.Logging;
using Master.Entity;
using Master.MES.Jobs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.MES.Events
{
    /// <summary>
    /// 有新的报工产生时的事件处理
    /// </summary>
    public class TaskReportEventHandler :
        IEventHandler<EntityCreatedEventData<ProcessTaskReport>>,
        ITransientDependency
    {
        private readonly TacticManager _tacticManager;
        private readonly ProcessTaskReportManager _processTaskReportManager;
        private readonly IBackgroundJobManager _backgroundJobManager;
        public ILogger Logger { get; set; }

        public RemindLogManager RemindLogManager { get; set; }
        public TaskReportEventHandler(
            TacticManager tacticManager,
            ProcessTaskReportManager processTaskReportManager,
            IBackgroundJobManager backgroundJobManager)
        {
            _tacticManager = tacticManager;
            _backgroundJobManager = backgroundJobManager;
            _processTaskReportManager = processTaskReportManager;
        }
        [UnitOfWork]
        public virtual void HandleEvent(EntityCreatedEventData<ProcessTaskReport> eventData)
        {
            //获取所有的被提醒人
            var remindInfos = _tacticManager.GetRemindPersonsByReport(eventData.Entity.Id).Result;
            foreach (var remindInfo in remindInfos)
            {
                var timespan = remindInfo.GetAvailableRemindTimeSpan();
                var remindPerson = remindInfo.Person;
                var openid = remindPerson.GetPropertyValue<string>("OpenId");
                //先产生一条提醒记录
                var remindLog = new RemindLog()
                {
                    RemindType = "报工提醒",
                    Name = remindPerson.Name,
                    TenantId = eventData.Entity.TenantId,
                    Message = eventData.Entity.ReportType.ToString(),                    
                };
                if (timespan.TotalMinutes > 0)
                {
                    remindLog.SetPropertyValue("errMsg", $"延迟{timespan.TotalMinutes.ToString("0")}分钟后运行");
                }
                var remindLogId = RemindLogManager.InsertAndGetIdAsync(remindLog).Result;
                
                var arg = new SendWeiXinMessageJobArgs()
                {
                    OpenId = openid,
                    DataId = eventData.Entity.Id,
                    RemindLogId = remindLogId,

                };

                
                _backgroundJobManager.Enqueue<SendReportWeiXinMessageJob, SendWeiXinMessageJobArgs>(arg, BackgroundJobPriority.Normal, timespan);
                //_backgroundJobManager.Enqueue<SendReportWeiXinMessageJob, SendWeiXinMessageJobArgs>(arg);
                //WeiXin.WeiXinHelper.SendTemplateMessage(openid, templateId, url, message);
            }
            //_backgroundJobManager.Enqueue<SendReportWeiXinMessageJob, int>(eventData.Entity.Id);

        }
    }
}
