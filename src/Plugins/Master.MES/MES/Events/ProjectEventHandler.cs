using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.Events.Bus.Entities;
using Abp.Events.Bus.Handlers;
using Master.Projects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.MES.Events
{
    /// <summary>
    /// 项目事件
    /// </summary>
    public class ProjectEventHandler : IEventHandler<EntityDeletedEventData<Project>>,
        ITransientDependency
    {
        public PartManager PartManager { get; set; }
        /// <summary>
        /// 项目删除后删除对应零件
        /// </summary>
        /// <param name="eventData"></param>
        [UnitOfWork]
        public virtual void HandleEvent(EntityDeletedEventData<Project> eventData)
        {
            PartManager.Repository.Delete(o => o.ProjectId == eventData.Entity.Id);
        }
    }
}
