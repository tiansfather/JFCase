using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.Events.Bus.Entities;
using Abp.Events.Bus.Handlers;
using Master.Entity;
using Master.MES.Jobs;
using Master.MultiTenancy;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Master.MES.Events
{
    /// <summary>
    /// 账套事件
    /// </summary>
    public class TenantEventHandler : IEventHandler<EntityCreatedEventData<Tenant>>,
        ITransientDependency
    {
        private readonly IBackgroundJobManager _backgroundJobManager;
        private readonly TacticManager _tacticManager;
        public TenantEventHandler(
            TacticManager tacticManager,
            IBackgroundJobManager backgroundJobManager)
        {
            _backgroundJobManager = backgroundJobManager;
            _tacticManager = tacticManager;
        }
        /// <summary>
        /// 新账套产生事件
        /// </summary>
        /// <param name="eventData"></param>
        [UnitOfWork]
        public virtual void HandleEvent(EntityCreatedEventData<Tenant> eventData)
        {
            
            try
            {
                var tactic = _tacticManager.GetAll().IgnoreQueryFilters().Where(o => o.TacticName == "注册提醒" && o.TenantId == null && o.IsActive).FirstOrDefault();
                if (tactic != null)
                {
                    //获取所有的被提醒人
                    var remindPersons = _tacticManager.GetTacticReminders(tactic.Id).Result;
                    foreach (var remindPerson in remindPersons)
                    {
                        var openid = remindPerson.GetPropertyValue<string>("OpenId");
                        var arg = new SendRegisteredMessageJobArgs()
                        {
                            OpenId = openid,
                            TenancyName = eventData.Entity.TenancyName,
                            Mobile = eventData.Entity.GetPropertyValue<string>("Mobile"),
                            PersonName = eventData.Entity.GetPropertyValue<string>("PersonName")

                        };

                        //发送注册提醒至管理人员
                        _backgroundJobManager.Enqueue<SendRegisteredMessageJob, SendRegisteredMessageJobArgs>(arg);
                    }
                }
            }
            catch
            {

            }
            
            
        }
    }
}
