using Abp.Application.Navigation;
using Abp.Authorization;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Events.Bus.Entities;
using Abp.Events.Bus.Handlers;
using Abp.Runtime.Caching;
using Master.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Master.Module
{
    /// <summary>
    /// 模块信息改变时移除缓存
    /// </summary>
    public class ModuleInfoChangedEventHandler : 
        IEventHandler<EntityDeletedEventData<ModuleInfo>>, 
        IEventHandler<EntityChangedEventData<ModuleInfo>>, 
        IEventHandler<ModuleInfoChangedEventData>, ITransientDependency
    {
        private readonly IPermissionManager _permissionManager;
        private readonly ICacheManager _cacheManager;
        private readonly IRepository<ModuleButton> _buttonRepository;
        private readonly IRepository<PermissionSetting> _permissionRepository;
        public ModuleInfoChangedEventHandler(ICacheManager cacheManager, IPermissionManager permissionManager, IRepository<PermissionSetting> permissionRepository, IRepository<ModuleButton> buttonRepository)
        {
            _cacheManager = cacheManager;
            _permissionManager = permissionManager;
            _permissionRepository = permissionRepository;
            _buttonRepository = buttonRepository;
        }
        public void HandleEvent(EntityDeletedEventData<ModuleInfo> eventData)
        {
            RemoveModuleInfoCache(eventData.Entity.ModuleKey,eventData.Entity.TenantId);
            RemoveModuleMenuCache(eventData.Entity.TenantId);
        }

        public void HandleEvent(EntityChangedEventData<ModuleInfo> eventData)
        {
            RemoveModuleInfoCache(eventData.Entity.ModuleKey, eventData.Entity.TenantId);
            RemoveModuleMenuCache(eventData.Entity.TenantId);
        }

        public void HandleEvent(ModuleInfoChangedEventData eventData)
        {
            RemoveModuleInfoCache(eventData.ModuleKey, eventData.TenantId);
            RemoveModuleMenuCache(eventData.TenantId);
        }
        [UnitOfWork]
        public virtual void RemoveModuleInfoCache(string moduleKey,int tenantId)
        {
            var key = moduleKey + "@" + tenantId;
            _cacheManager.GetCache<string, ModuleInfo>("ModuleInfo").Remove(key);
            //同时更新权限表，对于不存在的按钮，取消权限
            var buttonPermissions = _buttonRepository.GetAll().Where(o => o.ModuleInfo.ModuleKey == moduleKey).Select(o => $"Module.{moduleKey}.Button.{o.ButtonKey}").ToList();
            _permissionRepository.Delete(o => !buttonPermissions.Contains(o.Name) && o.Name.StartsWith($"Module.{moduleKey}.Button"));
            //取消权限缓存
            _cacheManager.GetCache<int, IList<Permission>>("ModuleInfoPermission").Remove(tenantId);
            _cacheManager.GetCache<int, IList<Permission>>("Permissions").Remove(tenantId);
        }

        public virtual void RemoveModuleMenuCache(int tenantId)
        {
            _cacheManager.GetCache<int, List<MenuItemDefinition>>("DefaultMenus").Remove(tenantId);
        }
    }
}
