using Abp.Authorization;
using Abp.Dependency;
using Abp.Events.Bus;
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
        private readonly IEventBus _eventBus;

        public UserPermissionCacheItemInvalidator(ICacheManager cacheManager, IEventBus eventBus)
        {
            _cacheManager = cacheManager;
            _eventBus = eventBus;
        }

        public void HandleEvent(EntityChangedEventData<UserPermissionSetting> eventData)
        {
            var cacheKey = eventData.Entity.UserId + "@" + (eventData.Entity.TenantId ?? 0);
            _cacheManager.GetUserPermissionCache().Remove(cacheKey);

            _eventBus.Trigger(new UserGrantedPermissionIndicator { UserId = eventData.Entity.UserId });

            //using (var userManagerWrapper = IocManager.Instance.ResolveAsDisposable<UserManager>())
            //{
            //    userManagerWrapper.Object.RemoveGrantedPermissionCache(eventData.Entity.UserId).GetAwaiter().GetResult();
            //}

        }

        public void HandleEvent(EntityChangedEventData<UserRole> eventData)
        {
            var cacheKey = eventData.Entity.UserId + "@" + (eventData.Entity.TenantId ?? 0);
            _cacheManager.GetUserPermissionCache().Remove(cacheKey);

            _eventBus.Trigger(new UserGrantedPermissionIndicator { UserId = eventData.Entity.UserId });
            //using (var userManagerWrapper = IocManager.Instance.ResolveAsDisposable<UserManager>())
            //{
            //    userManagerWrapper.Object.RemoveGrantedPermissionCache(eventData.Entity.UserId).GetAwaiter().GetResult();
            //}
        }

        public void HandleEvent(EntityDeletedEventData<User> eventData)
        {
            var cacheKey = eventData.Entity.Id + "@" + (eventData.Entity.TenantId ?? 0);
            _cacheManager.GetUserPermissionCache().Remove(cacheKey);
        }
    }
}
