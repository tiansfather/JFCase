using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Events.Bus;
using Abp.Events.Bus.Entities;
using Abp.Events.Bus.Handlers;
using Abp.Runtime.Caching;
using Master.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Master.Authentication
{
    public class RolePermissionCacheItemInvalidator :
        IEventHandler<EntityChangedEventData<RolePermissionSetting>>,
        IEventHandler<EntityDeletedEventData<Role>>,
        ITransientDependency
    {
        private readonly ICacheManager _cacheManager;
        private readonly IEventBus _eventBus;

        public RolePermissionCacheItemInvalidator(ICacheManager cacheManager, IEventBus eventBus)
        {
            _cacheManager = cacheManager;
            _eventBus = eventBus;
        }

        [UnitOfWork]
        public virtual void HandleEvent(EntityChangedEventData<RolePermissionSetting> eventData)
        {
            var cacheKey = eventData.Entity.RoleId + "@" + (eventData.Entity.TenantId ?? 0);
            _cacheManager.GetRolePermissionCache().Remove(cacheKey);

            //角色权限改变后清空角色对应用户的权限缓存
            _eventBus.Trigger(new UserGrantedPermissionIndicator { RoleId = eventData.Entity.RoleId });

            //var scope = IocManager.Instance.CreateScope();
            //var userRoleRepository = scope.Resolve<IRepository<UserRole, int>>();
            //var userManager = scope.Resolve<UserManager>();
            //var userIds = userRoleRepository.GetAll().Where(o => o.RoleId == eventData.Entity.RoleId).Select(o => o.UserId).ToList();
            //foreach (var userId in userIds)
            //{
            //    userManager.RemoveGrantedPermissionCache(userId).GetAwaiter().GetResult();
            //}
            //
        }

        public void HandleEvent(EntityDeletedEventData<Role> eventData)
        {
            var cacheKey = eventData.Entity.Id + "@" + (eventData.Entity.TenantId ?? 0);
            _cacheManager.GetRolePermissionCache().Remove(cacheKey);
        }
    }
}
