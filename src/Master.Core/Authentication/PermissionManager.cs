using Abp;
using Abp.Application.Features;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Dependency;
using Abp.Domain.Services;
using Abp.MultiTenancy;
using Abp.Runtime.Caching;
using Abp.Runtime.Session;
using Master.Domain;
using Master.Menu;
using Master.Module;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace Master.Authentication
{
    /// <summary>
    /// 替换原有的权限管理类
    /// </summary>
    public class PermissionManager : DomainServiceBase, IPermissionManager
    {
        public IAbpSession AbpSession { get; set; }
        private readonly IIocManager _iocManager;
        private readonly ICacheManager _cacheManager;
        //private readonly IAuthorizationConfiguration _authorizationConfiguration;
        //private readonly IMenuManager _menuManager;
        //private readonly IModuleInfoManager _moduleInfoManager;

        public PermissionManager(
            IIocManager iocManager,
            ICacheManager cacheManager,
            //IAuthorizationConfiguration authorizationConfiguration,
            IModuleInfoManager moduleInfoManager
            )
        {
            _iocManager = iocManager;
            //_authorizationConfiguration = authorizationConfiguration;
            _cacheManager = cacheManager;

            AbpSession = NullAbpSession.Instance;
        }

        public IList<Permission> GetAll()
        {
            //var key = AbpSession.TenantId ?? 0;
            var key = CurrentUnitOfWork.GetTenantId() ?? 0;
            return _cacheManager.GetCache<int,IList<Permission>>("Permissions").Get(key, () => {
                var permissions = new List<Permission>();
                //加入导航权限
                permissions.AddRange(Resolve<IMenuManager>().GetAllMenuPermissions());
                //todo:加入菜单对应的资源权限
                //加入模块权限
                permissions.AddRange(Resolve<IModuleInfoManager>().GetAllModulePermissions());
                return permissions;
            });
            
        }

        public IReadOnlyList<Permission> GetAllPermissions(bool tenancyFilter = true)
        {
            using (var featureDependencyContext = _iocManager.ResolveAsDisposable<FeatureDependencyContext>())
            {
                var featureDependencyContextObject = featureDependencyContext.Object;
                return GetAll()
                    .WhereIf(tenancyFilter, p => p.MultiTenancySides.HasFlag(AbpSession.MultiTenancySide))
                    .Where(p =>
                        p.FeatureDependency == null ||
                        AbpSession.MultiTenancySide == MultiTenancySides.Host ||
                        p.FeatureDependency.IsSatisfied(featureDependencyContextObject)
                    ).ToImmutableList();
            }
        }

        public IReadOnlyList<Permission> GetAllPermissions(MultiTenancySides multiTenancySides)
        {
            using (var featureDependencyContext = _iocManager.ResolveAsDisposable<FeatureDependencyContext>())
            {
                var featureDependencyContextObject = featureDependencyContext.Object;
                return GetAll()
                    .Where(p => p.MultiTenancySides.HasFlag(multiTenancySides))
                    .Where(p =>
                        p.FeatureDependency == null ||
                        AbpSession.MultiTenancySide == MultiTenancySides.Host ||
                        (p.MultiTenancySides.HasFlag(MultiTenancySides.Host) &&
                         multiTenancySides.HasFlag(MultiTenancySides.Host)) ||
                        p.FeatureDependency.IsSatisfied(featureDependencyContextObject)
                    ).ToImmutableList();
            }
        }

        public Permission GetPermission(string name)
        {
            var permission = GetPermissionOrNull(name);
            //if (permission == null)
            //{
            //    throw new AbpException("There is no permission with name: " + name);
            //}

            return permission;
        }

        public Permission GetPermissionOrNull(string name)
        {
            var permission = GetAllPermissions().Where(o => o.Name == name).FirstOrDefault();
            return permission;
        }
    }
}
