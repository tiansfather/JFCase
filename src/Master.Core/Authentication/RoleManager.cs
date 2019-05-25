using Abp;
using Abp.Application.Features;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.MultiTenancy;
using Abp.Runtime.Caching;
using Abp.UI;
using Master.Cache;
using Master.Configuration;
using Master.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.Authentication
{
    public class RoleManager : DomainServiceBase<Role,int>, ITransientDependency
    {
        private readonly IPermissionManager _permissionManager;
        private readonly IRepository<RolePermissionSetting> _rolePermissionSettingRepository;

        public FeatureDependencyContext FeatureDependencyContext { get; set; }

        public IRoleManagementConfig RoleManagementConfig { get; set; }
        public RoleManager(
            IPermissionManager permissionManager,
            IRepository<RolePermissionSetting> rolePermissionSettingRepository
            )
        {
            _permissionManager = permissionManager;
            _rolePermissionSettingRepository = rolePermissionSettingRepository;
        }
        public virtual async Task CheckDuplicateRoleNameAsync(int? expectedRoleId, string name, string displayName)
        {
            var role = await FindByNameAsync(name);
            if (role != null && role.Id != expectedRoleId)
            {
                throw new UserFriendlyException("相同角色名称已存在");
            }

            role = await FindByDisplayNameAsync(displayName);
            if (role != null && role.Id != expectedRoleId)
            {
                throw new UserFriendlyException("相同角色名称已存在");
            }

        }
        private Task<Role> FindByDisplayNameAsync(string displayName)
        {
            return Repository.FirstOrDefaultAsync(o => o.DisplayName == displayName);
        }
        public virtual Task<Role> FindByNameAsync(string name)
        {
            return Repository.FirstOrDefaultAsync(o => o.Name == name);
        }
        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override async Task InsertAsync(Role entity)
        {
            //重名检测
            await CheckDuplicateRoleNameAsync(entity.Id, entity.Name, entity.DisplayName);

            var tenantId = GetCurrentTenantId();
            if (tenantId.HasValue && !entity.TenantId.HasValue)
            {
                entity.TenantId = tenantId.Value;
            }

             await base.InsertAsync(entity);
        }

        /// <summary>
        /// 修改角色
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override async Task UpdateAsync(Role entity)
        {
            await CheckDuplicateRoleNameAsync(entity.Id, entity.Name, entity.DisplayName);
            await base.UpdateAsync(entity);
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override Task DeleteAsync(Role entity)
        {
            if (entity.IsStatic)
            {
                throw new UserFriendlyException(string.Format(L("CanNotDeleteStaticRole"), entity.Name));
            }
            return base.DeleteAsync(entity);
        }


        #region 初始化账套角色
        public virtual async Task CreateStaticRoles(int tenantId)
        {
            var staticRoleDefinitions = RoleManagementConfig.StaticRoles.Where(sr => sr.Side == MultiTenancySides.Tenant);
            
            using (CurrentUnitOfWork.SetTenantId(tenantId))
            {
                foreach (var staticRoleDefinition in staticRoleDefinitions)
                {
                    var role = new Role
                    {
                        TenantId = tenantId,
                        Name = staticRoleDefinition.RoleName,
                        DisplayName = staticRoleDefinition.RoleName,
                        IsStatic = true
                    };

                    await InsertAsync(role);
                    
                }
            }

        }
        #endregion

        public virtual async Task<bool> IsGrantedAsync(int roleId, string permissionName)
        {
            return await IsGrantedAsync(roleId, _permissionManager.GetPermission(permissionName));
        }

        public virtual async Task<bool> IsGrantedAsync(int roleId, Permission permission)
        {
            //Get cached role permissions
            var cacheItem = await GetRolePermissionCacheItemAsync(roleId);

            //Check the permission
            return cacheItem.GrantedPermissions.Contains(permission.Name);
        }

        public virtual async Task<IReadOnlyList<Permission>> GetGrantedPermissionsAsync(int roleId)
        {
            return await GetGrantedPermissionsAsync(await GetByIdAsync(roleId));
        }

        public virtual async Task<IReadOnlyList<Permission>> GetGrantedPermissionsAsync(Role role)
        {
            var permissionList = new List<Permission>();

            foreach (var permission in _permissionManager.GetAllPermissions())
            {
                if (await IsGrantedAsync(role.Id, permission))
                {
                    permissionList.Add(permission);
                }
            }

            return permissionList;
        }
        /// <summary>
        /// 获取基于角色的权限设置信息
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public virtual async Task<IList<PermissionGrantInfo>> GetPermissionsAsync(int roleId)
        {
            return (await _rolePermissionSettingRepository.GetAllListAsync(p => p.RoleId == roleId))
                .Select(p => new PermissionGrantInfo(p.Name, p.IsGranted))
                .ToList();
        }
        private async Task<RolePermissionCacheItem> GetRolePermissionCacheItemAsync(int roleId)
        {
            var cacheKey = roleId + "@" + (GetCurrentTenantId() ?? 0);
            return await CacheManager.GetRolePermissionCache().GetAsync(cacheKey, async () =>
            {
                var newCacheItem = new RolePermissionCacheItem(roleId);

                var role = await GetByIdAsync(roleId);
                if (role == null)
                {
                    throw new AbpException("There is no role with given id: " + roleId);
                }

                //var staticRoleDefinition = RoleManagementConfig.StaticRoles.FirstOrDefault(r => r.RoleName == role.Name);
                //if (staticRoleDefinition != null)
                //{
                //    foreach (var permission in _permissionManager.GetAllPermissions())
                //    {
                //        if (staticRoleDefinition.IsGrantedByDefault(permission))
                //        {
                //            newCacheItem.GrantedPermissions.Add(permission.Name);
                //        }
                //    }
                //}

                foreach (var permissionInfo in await GetPermissionsAsync(roleId))
                {
                    if (permissionInfo.IsGranted)
                    {
                        newCacheItem.GrantedPermissions.AddIfNotContains(permissionInfo.Name);
                    }
                    else
                    {
                        newCacheItem.GrantedPermissions.Remove(permissionInfo.Name);
                    }
                }

                return newCacheItem;
            });
        }

        public virtual async Task SetGrantedPermissionsAsync(int roleId, IEnumerable<Permission> permissions)
        {
            await SetGrantedPermissionsAsync(await GetByIdAsync(roleId), permissions);
        }

        public virtual async Task SetGrantedPermissionsAsync(Role role, IEnumerable<Permission> permissions)
        {
            var oldPermissions = await GetGrantedPermissionsAsync(role);
            var newPermissions = permissions.ToArray();

            foreach (var permission in oldPermissions.Where(p => !newPermissions.Contains(p, PermissionEqualityComparer.Instance)))
            {
                await ProhibitPermissionAsync(role, permission);
            }

            foreach (var permission in newPermissions.Where(p => !oldPermissions.Contains(p, PermissionEqualityComparer.Instance)))
            {
                await GrantPermissionAsync(role, permission);
            }
        }

        public async Task GrantPermissionAsync(Role role, Permission permission)
        {
            if (await IsGrantedAsync(role.Id, permission))
            {
                return;
            }

            await RemovePermissionAsync(role, new PermissionGrantInfo(permission.Name, false));
            await AddPermissionAsync(role, new PermissionGrantInfo(permission.Name, true));
        }
        public async Task GrantAllPermissionsAsync(Role role)
        {
            FeatureDependencyContext.TenantId = role.TenantId;

            var permissions = _permissionManager.GetAllPermissions(role.GetMultiTenancySide())
                                                .Where(permission =>
                                                    permission.FeatureDependency == null ||
                                                    permission.FeatureDependency.IsSatisfied(FeatureDependencyContext)
                                                );

            await SetGrantedPermissionsAsync(role, permissions);
        }
        public async Task ProhibitAllPermissionsAsync(Role role)
        {
            foreach (var permission in _permissionManager.GetAllPermissions())
            {
                await ProhibitPermissionAsync(role, permission);
            }
        }
        public async Task ProhibitPermissionAsync(Role role, Permission permission)
        {
            if (!await IsGrantedAsync(role.Id, permission))
            {
                return;
            }

            await RemovePermissionAsync(role, new PermissionGrantInfo(permission.Name, true));
            await AddPermissionAsync(role, new PermissionGrantInfo(permission.Name, false));
        }

        public async Task ResetAllPermissionsAsync(Role role)
        {
            await RemoveAllPermissionSettingsAsync(role);
        }

        #region 角色权限设置
        private async Task<bool> HasPermissionAsync(int roleId, PermissionGrantInfo permissionGrant)
        {
            return await _rolePermissionSettingRepository.FirstOrDefaultAsync(
                p => p.RoleId == roleId &&
                     p.Name == permissionGrant.Name &&
                     p.IsGranted == permissionGrant.IsGranted
                ) != null;
        }
        /// <summary>
        /// 移除基于角色的所有权限设置
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        private async Task RemoveAllPermissionSettingsAsync(Role role)
        {
            await _rolePermissionSettingRepository.DeleteAsync(s => s.RoleId == role.Id);
        }
        private async Task AddPermissionAsync(Role role, PermissionGrantInfo permissionGrant)
        {
            if (await HasPermissionAsync(role.Id, permissionGrant))
            {
                return;
            }

            await _rolePermissionSettingRepository.InsertAsync(
                new RolePermissionSetting
                {
                    TenantId = role.TenantId,
                    RoleId = role.Id,
                    Name = permissionGrant.Name,
                    IsGranted = permissionGrant.IsGranted
                });
        }
        private async Task RemovePermissionAsync(Role role, PermissionGrantInfo permissionGrant)
        {
            await _rolePermissionSettingRepository.DeleteAsync(
                permissionSetting => permissionSetting.RoleId == role.Id &&
                                     permissionSetting.Name == permissionGrant.Name &&
                                     permissionSetting.IsGranted == permissionGrant.IsGranted
                );
        }
        #endregion

        private int? GetCurrentTenantId()
        {
            if (UnitOfWorkManager.Current != null)
            {
                return UnitOfWorkManager.Current.GetTenantId();
            }

            return AbpSession.TenantId;
        }

    }
}
