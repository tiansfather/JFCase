using Abp.Dependency;
using Abp.Events.Bus.Entities;
using Abp.Events.Bus.Handlers;
using Abp.Runtime.Caching;
using Master.Cache;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Authentication
{
    public class UserPermissionCacheItemInvalidator :
        IEventHandler<EntityChangedEventData<UserPermissionSetting>>,
        IEventHandler<EntityChangedEventData<UserRole>>,
        IEventHandler<EntityDeletedEventData<User>>,
        ITransientDependency
    {
        private readonly ICacheManager _cacheManager;

        public UserPermissionCacheItemInvalidator(ICacheManager cacheManager)
        {
            _cacheManager = cacheManager;
        }

        public void HandleEvent(EntityChangedEventData<UserPermissionSetting> eventData)
        {
            var cacheKey = eventData.Entity.UserId + "@" + (eventData.Entity.TenantId ?? 0);
            _cacheManager.GetUserPermissionCache().Remove(cacheKey);
        }

        public void HandleEvent(EntityChangedEventData<UserRole> eventData)
        {
            var cacheKey = eventData.Entity.UserId + "@" + (eventData.Entity.TenantId ?? 0);
            _cacheManager.GetUserPermissionCache().Remove(cacheKey);
        }

        public void HandleEvent(EntityDeletedEventData<User> eventData)
        {
            var cacheKey = eventData.Entity.Id + "@" + (eventData.Entity.TenantId ?? 0);
            _cacheManager.GetUserPermissionCache().Remove(cacheKey);
        }
    }
}
