using Abp.Dependency;
using Abp.Events.Bus.Entities;
using Abp.Events.Bus.Handlers;
using Abp.Runtime.Caching;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.MES.Events
{
    public class TacticEventHandler :
        IEventHandler<EntityChangedEventData<Tactic>>,
        ITransientDependency
    {
        private readonly ICacheManager _cacheManager;
        public TacticEventHandler(ICacheManager cacheManager)
        {
            _cacheManager = cacheManager;
        }
        public void HandleEvent(EntityChangedEventData<Tactic> eventData)
        {
            //删除对应账套策略缓存
            _cacheManager.GetCache<int, List<Tactic>>("Tactics").Remove(eventData.Entity.TenantId ?? 0);
        }
    }
}
