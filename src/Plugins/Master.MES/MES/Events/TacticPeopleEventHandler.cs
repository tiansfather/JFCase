using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.Events.Bus.Entities;
using Abp.Events.Bus.Handlers;
using Master.MES.Jobs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Master.MES.Events
{
    /// <summary>
    /// 绑定提醒人或解绑提醒人时的事件处理
    /// </summary>
    public class TacticPeopleEventHandler
        : IEventHandler<EntityCreatedEventData<TacticPerson>>,
        IEventHandler<EntityDeletedEventData<TacticPerson>>,
        ITransientDependency
    {
        private readonly TacticManager _tacticManager;
        private readonly PersonManager _personManager;
        private readonly IBackgroundJobManager _backgroundJobManager;

        public RemindLogManager RemindLogManager { get; set; }
        public TacticPeopleEventHandler(
            TacticManager tacticManager,
            PersonManager personManager,
            IBackgroundJobManager backgroundJobManager)
        {
            _tacticManager = tacticManager;
            _personManager = personManager;
            _backgroundJobManager = backgroundJobManager;
        }
        /// <summary>
        /// 绑定策略后事件
        /// </summary>
        /// <param name="eventData"></param>
        [UnitOfWork]
        public virtual void HandleEvent(EntityCreatedEventData<TacticPerson> eventData)
        {
            var person = _personManager.Repository.GetAll().IgnoreQueryFilters().Where(o=>o.Id==eventData.Entity.PersonId).Single();
            var tactic = _tacticManager.Repository.GetAll().IgnoreQueryFilters().Where(o=>o.Id==eventData.Entity.TacticId).Single();

            //先产生一条提醒记录
            var remindLog = new RemindLog()
            {
                RemindType = "策略绑定提醒",
                Name = person.Name,
                TenantId = tactic.TenantId,
                Message = ""
            };
            var remindLogId = RemindLogManager.InsertAndGetIdAsync(remindLog).Result;
            var arg = new SendTacticBindWeiXinMessageJobArgs()
            {
                BindType=1,
                TacticId=tactic.Id,
                PersonId=person.Id,
                RemindLogId = remindLogId
            };


            _backgroundJobManager.Enqueue<SendTacticBindWeiXinMessageJob, SendTacticBindWeiXinMessageJobArgs>(arg);
        }

        /// <summary>
        /// 解除绑定后事件
        /// </summary>
        /// <param name="eventData"></param>
        [UnitOfWork]
        public virtual void HandleEvent(EntityDeletedEventData<TacticPerson> eventData)
        {
            var person = _personManager.Repository.GetAll().IgnoreQueryFilters().Where(o => o.Id == eventData.Entity.PersonId).Single();
            var tactic = _tacticManager.Repository.GetAll().IgnoreQueryFilters().Where(o => o.Id == eventData.Entity.TacticId).Single();
            //先产生一条提醒记录
            var remindLog = new RemindLog()
            {
                RemindType = "策略解除绑定提醒",
                Name = person.Name,
                TenantId = tactic.TenantId,
                Message = ""
            };
            var remindLogId = RemindLogManager.InsertAndGetIdAsync(remindLog).Result;
            var arg = new SendTacticBindWeiXinMessageJobArgs()
            {
                BindType = 0,
                TacticId = tactic.Id,
                PersonId = person.Id,
                RemindLogId = remindLogId
            };


            _backgroundJobManager.Enqueue<SendTacticBindWeiXinMessageJob, SendTacticBindWeiXinMessageJobArgs>(arg);
        }
    }
}
