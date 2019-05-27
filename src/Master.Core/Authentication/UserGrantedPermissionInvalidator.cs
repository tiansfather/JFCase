using Abp.Authorization;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Events.Bus;
using Abp.Events.Bus.Handlers;
using Abp.Runtime.Caching;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Master.Authentication
{
    /// <summary>
    /// 当用户权限相关发生变化时(如版本、特性、角色)，清空用户权限缓存
    /// </summary>
    public class UserGrantedPermissionInvalidator : IEventHandler<UserGrantedPermissionIndicator>, ITransientDependency
    {
        public ICacheManager CacheManager { get; set; }
        public void HandleEvent(UserGrantedPermissionIndicator eventData)
        {
            var users = GetUsersByIndicator(eventData);
            foreach (var user in users)
            {
                var cacheKey = user.Id + "@" + (user.TenantId ?? 0);
                CacheManager.GetCache<string, IReadOnlyList<Permission>>("UserGrantedPermissions").Remove(cacheKey);
            }
        }

        private IEnumerable<User> GetUsersByIndicator(UserGrantedPermissionIndicator userGrantedPermissionIndicator)
        {
            var users = new List<User>();
            var scope = IocManager.Instance.CreateScope();
            var userManager = scope.Resolve<UserManager>();
            var userRoleRepository = scope.Resolve<IRepository<UserRole, int>>();
            if (userGrantedPermissionIndicator.UserId.HasValue)
            {
                users.Add(userManager.GetByIdAsync(userGrantedPermissionIndicator.UserId.Value).Result);
            }
            else if (userGrantedPermissionIndicator.RoleId.HasValue)
            {
                var userIds = userRoleRepository.GetAll().Where(o => o.RoleId == userGrantedPermissionIndicator.RoleId.Value).Select(o => o.UserId).ToList();
                users = userManager.GetListByIdsAsync(userIds).Result.ToList();
            }
            else if (userGrantedPermissionIndicator.TenantId.HasValue)
            {
                users = userManager.GetAll().IgnoreQueryFilters().Where(o => !o.IsDeleted && o.TenantId == userGrantedPermissionIndicator.TenantId.Value).ToList();
            }
            else if (userGrantedPermissionIndicator.EditionId.HasValue)
            {
                users = userManager.GetAll().IgnoreQueryFilters().Where(o => !o.IsDeleted && o.Tenant.EditionId == userGrantedPermissionIndicator.EditionId.Value).ToList();
            }

            return users;
        }
    }

    public class UserGrantedPermissionIndicator : EventData
    {
        public long? UserId { get; set; }
        public int? RoleId { get; set; }
        public int? TenantId { get; set; }
        public int? EditionId { get; set; }
    }
}
