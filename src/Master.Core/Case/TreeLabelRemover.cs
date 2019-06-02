using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Events.Bus.Entities;
using Abp.Events.Bus.Handlers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Case
{
    public class TreeLabelRemover :
        IEventHandler<EntityDeletedEventData<Label>>,
        ITransientDependency
    {
        public IRepository<TreeLabel,int> Repository { get; set; }
        public void HandleEvent(EntityDeletedEventData<Label> eventData)
        {
            //删除标签后同时清除对应的标签和树绑定信息
            Repository.Delete(o => o.LabelId == eventData.Entity.Id);
        }
    }
}
