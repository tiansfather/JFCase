using Abp.Dependency;
using Abp.Events.Bus.Entities;
using Abp.Events.Bus.Handlers;
using Abp.Runtime.Caching;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Configuration.Dictionaries
{
    public class DictionaryChangedEventHandler : IEventHandler<EntityDeletedEventData<Dictionary>>, IEventHandler<EntityCreatedEventData<Dictionary>>, IEventHandler<EntityChangedEventData<Dictionary>>, ITransientDependency
    {
        private readonly ICacheManager _cacheManager;
        public DictionaryChangedEventHandler(ICacheManager cacheManager)
        {
            _cacheManager = cacheManager;
        }
        public void HandleEvent(EntityDeletedEventData<Dictionary> eventData)
        {
            RemoveDictionaryCache(eventData.Entity.TenantId);
        }

        public void HandleEvent(EntityCreatedEventData<Dictionary> eventData)
        {
            RemoveDictionaryCache(eventData.Entity.TenantId);
        }

        public void HandleEvent(EntityChangedEventData<Dictionary> eventData)
        {
            RemoveDictionaryCache(eventData.Entity.TenantId);
        }

        private void RemoveDictionaryCache(int tenantId)
        {
            _cacheManager.GetCache<int, Dictionary<string, Dictionary<string, string>>>("UserDictionary").Remove(tenantId);
        }
    }
}
