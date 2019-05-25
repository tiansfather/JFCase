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
    public class RolePermissionCacheItemInvalidator :
        IEventHandler<EntityChangedEventData<RolePermissionSetting>>,
        IEventHandler<EntityDeletedEventData<Role>>,
        ITransientDependency
    {
        private readonly ICacheManager _cacheManager;

        public RolePermissionCacheItemInvalidator(ICacheManager cacheManager)
        {
            _cacheManager = cacheManager;
        }

        public void HandleEvent(EntityChangedEventData<RolePermissionSetting> eventData)
        {
            var cacheKey = eventData.Entity.RoleId + "@" + (eventData.Entity.TenantId ?? 0);
            _cacheManager.GetRolePermissionCache().Remove(cacheKey);
        }

        public void HandleEvent(EntityDeletedEventData<Role> eventData)
        {
            var cacheKey = eventData.Entity.Id + "@" + (eventData.Entity.TenantId ?? 0);
            _cacheManager.GetRolePermissionCache().Remove(cacheKey);
        }
    }
}
