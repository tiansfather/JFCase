using Abp.Authorization;
using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.MultiTenancy;
using Abp.Runtime.Security;
using Abp.Runtime.Caching;
using Master.Cache;
using Master.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Master.Module;
using Master.MultiTenancy;
using Master.Entity;
using Abp.UI;
using Microsoft.AspNetCore.Http;
using Master.Case;

namespace Master.Authentication
{
    public class UserManager :ModuleServiceBase<User,long>, ITransientDependency
    {
        private readonly IPermissionManager _permissionManager;
        private readonly IRepository<UserRole> _userRoleRepository;
        private readonly IRepository<Role> _roleRepository;
        private readonly IRepository<UserPermissionSetting> _userPermissionSettingRepository;
        private readonly IRepository<UserLogin> _userLoginRepository;
        //private readonly RoleManager _roleManager;

        public IMultiTenancyConfig MultiTenancy { get; set; }
        //public IModuleInfoManager ModuleInfoManager { get; set; }
        //public TenantManager TenantManager { get; set; }
        public UserManager(
            IPermissionManager permissionManager,
            //RoleManager roleManager,
            IRepository<UserRole> userRoleRepository,
            IRepository<Role> roleRepository,
            IRepository<UserPermissionSetting> userPermissionSettingRepository,
            IRepository<UserLogin> userLoginRepository
            )
        {
            _permissionManager = permissionManager;
            _userRoleRepository = userRoleRepository;
            _roleRepository = roleRepository;
            _userPermissionSettingRepository = userPermissionSettingRepository;
            _userLoginRepository = userLoginRepository;
            //_roleManager = roleManager;
        }

        
        #region 数据验证
        public override async Task ValidateEntity(User entity)
        {
            if (entity.Id > 0 && await Repository.GetAll().CountAsync(o => o.UserName == entity.UserName && o.TenantId==entity.TenantId && o.Id != entity.Id) > 0)
            {
                throw new UserFriendlyException(L("用户名已被占用,请调整后再试"));
            }

            if (entity.Id == 0 && await Repository.GetAll().CountAsync(o => o.UserName == entity.UserName) > 0)
            {
                throw new UserFriendlyException(L("用户名已被占用，无法创建，请调整后再试"));
            }

            if (!string.IsNullOrEmpty(entity.Email))
            {
                if (entity.Id > 0 && await Repository.GetAll().CountAsync(o => o.Email == entity.Email && o.Id != entity.Id) > 0)
                {
                    throw new UserFriendlyException(L("邮箱已被占用，无法创建，请调整后再试"));
                }

                if (entity.Id == 0 && await Repository.GetAll().CountAsync(o => o.Email == entity.Email) > 0)
                {
                    throw new UserFriendlyException(L("邮箱已被占用，无法创建，请调整后再试"));
                }
            }

        }
        #endregion

        #region 角色
        public virtual async Task SetRoles(User user, int[] roleIds)
        {
            Repository.EnsureCollectionLoaded(user, o => o.Roles);
            var userRolesIds = user.Roles.Select(o => o.RoleId);
            foreach (var roleId in roleIds.Where(o => !userRolesIds.Contains(o)))
            {
                var userRole = new UserRole(user.TenantId, user.Id, roleId);
                await _userRoleRepository.InsertAsync(userRole);
            }
            foreach (var roleId in userRolesIds.Where(o => !roleIds.Contains(o)))
            {
                await _userRoleRepository.DeleteAsync(o => o.RoleId == roleId && o.TenantId == user.TenantId && o.UserId == user.Id);
            }

        }
        /// <summary>
        /// 去除用户的角色
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public virtual async Task RemoveFromRoleAsync(long userId, int roleId)
        {
            await _userRoleRepository.DeleteAsync(o => o.UserId == userId && o.RoleId == roleId);
        }
        /// <summary>
        /// 将用户添入角色
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public virtual async Task MoveIntoRoleAsync(long userId, int roleId)
        {
            if (await _userRoleRepository.CountAsync(o => o.UserId == userId && o.RoleId == roleId) == 0)
            {
                await _userRoleRepository.InsertAsync(new UserRole() { UserId = userId, RoleId = roleId, TenantId = AbpSession.TenantId });
            }
        }
        #endregion

        #region 密码
        public virtual async Task SetPassword(User user, string password)
        {
            user.Password = SimpleStringCipher.Instance.Encrypt(password);
            await Repository.UpdateAsync(user);
        }
        /// <summary>
        /// 检查用户帐户是否正确
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public virtual Task<bool> CheckPasswordAsync(User user, string password)
        {
            return Task.FromResult(SimpleStringCipher.Instance.Encrypt(password) == user.Password);
        }

        #endregion

        #region 人员获取
        /// <summary>
        /// 通过姓名获取用户
        /// </summary>
        /// <param name="names"></param>
        /// <returns></returns>
        public virtual async Task<List<User>> FindByRealNames(string[] names)
        {
            return await GetAll().Where(o => names.Contains(o.Name)).ToListAsync();
        }
        /// <summary>
        /// 通过帐套id，及用户名或手机号获取用户
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public virtual async Task<User> FindByNameOrPhone(int? tenantId, string usernameOrPhone)
        {
            return await GetAll().Where(o => o.TenantId == tenantId && (o.UserName == usernameOrPhone || o.PhoneNumber == usernameOrPhone)).FirstOrDefaultAsync();
        }
        /// <summary>
        /// 通过第三方登录寻找用户
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="login"></param>
        /// <returns></returns>
        public virtual Task<User> FindAsync(UserLoginInfo login)
        {
            var query = from userLogin in _userLoginRepository.GetAll()
                        join user in GetAll().IgnoreQueryFilters() on userLogin.UserId equals user.Id
                        where userLogin.LoginProvider == login.LoginProvider && userLogin.ProviderKey == login.ProviderKey && !user.IsDeleted
                        select user;

            return Task.FromResult(query.FirstOrDefault());
        }
        /// <summary>
        /// 获取某角色的所有用户
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public virtual async Task<List<User>> GetUsersInRoleAsync(int roleId)
        {
            var query = from user in GetAll()
                        join userRole in _userRoleRepository.GetAll() on user.Id equals userRole.UserId
                        where userRole.RoleId == roleId
                        select user;

            return await query.ToListAsync();
        }
        public virtual async Task<int> GetUsersCountInRoleAsync(int roleId)
        {
            var query = from user in GetAll()
                        join userRole in _userRoleRepository.GetAll() on user.Id equals userRole.UserId
                        where userRole.RoleId == roleId
                        select user;

            return await query.CountAsync();
        }
        /// <summary>
        /// 获取所有有指定权限的用户
        /// </summary>
        /// <param name="permissionName"></param>
        /// <returns></returns>
        public virtual async Task<List<User>> FindByPermission(string permissionName)
        {
            var users = await GetAllList();
            return users
                .Where(o=>o.IsActive)
                .Where(o => IsGrantedAsync(o.Id, permissionName).Result)
                .ToList();
        }
        #endregion

        #region 第三方登录
        /// <summary>
        /// 绑定第三方登录
        /// </summary>
        /// <param name="userLoginInfo"></param>
        /// <returns></returns>
        public virtual async Task BindExternalLogin(User user, UserLoginInfo userLoginInfo)
        {
            await _userLoginRepository.DeleteAsync(o => o.UserId == user.Id && o.LoginProvider == userLoginInfo.LoginProvider);
            var userlogin = new UserLogin()
            {
                UserId = user.Id,
                LoginProvider = userLoginInfo.LoginProvider,
                TenantId = user.TenantId,
                ProviderKey = userLoginInfo.ProviderKey
            };
            await _userLoginRepository.InsertAsync(userlogin);

        }
        #endregion

        #region 权限角色
        /// <summary>
        /// 获取用户拥有所有权限
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public virtual async Task<IReadOnlyList<Permission>> GetGrantedPermissionsAsync(User user)
        {
            var permissionList = new List<Permission>();

            foreach (var permission in _permissionManager.GetAllPermissions())
            {
                if (await IsGrantedAsync(user.Id, permission))
                {
                    permissionList.Add(permission);
                }
            }

            return permissionList;
        }
        /// <summary>
        /// 判断某用户是否有某权限
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="permissionName"></param>
        /// <returns></returns>
        public virtual async Task<bool> IsGrantedAsync(long userId, string permissionName)
        {
            //如果此权限是一个资源按钮,需要检查此按钮是否启用以及是否需要权限
            var button = await Resolve<IModuleInfoManager>().FindButtonByPermissionName(permissionName);
            if (button != null)
            {
                //未启用的资源的判断全部为false
                if (!button.IsEnabled)
                {
                    return false;
                }
                //已启用的资源若不需要权限，则判定为true
                else if (!button.RequirePermission)
                {
                    return true;
                }
            }
            return await IsGrantedAsync(
                userId,
                _permissionManager.GetPermission(permissionName)
                );
        }
        public virtual Task<bool> IsGrantedAsync(User user, Permission permission)
        {
            return IsGrantedAsync(user.Id, permission);
        }
        /// <summary>
        /// 判断用户是否有权限
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="permission"></param>
        /// <returns></returns>
        public virtual async Task<bool> IsGrantedAsync(long userId, Permission permission)
        {
            //当新权限加到系统中，为了防止影响旧有用户，默认均授权
            if (permission == null) { return true; }
            
            //判断权限归属是否符合Host,Tenant
            if (!permission.MultiTenancySides.HasFlag(GetCurrentMultiTenancySide()))
            {
                return false;
            }
            
            //Check for depended features
            //if (permission.FeatureDependency != null && GetCurrentMultiTenancySide() == MultiTenancySides.Tenant)
            //{
            //    FeatureDependencyContext.TenantId = GetCurrentTenantId();

            //    if (!await permission.FeatureDependency.IsSatisfiedAsync(FeatureDependencyContext))
            //    {
            //        return false;
            //    }
            //}

            //Get cached user permissions
            var cacheItem = await GetUserPermissionCacheItemAsync(userId);
            if (cacheItem == null)
            {
                return false;
            }
            //Check for user-specific value
            if (cacheItem.GrantedPermissions.Contains(permission.Name))
            {
                return true;
            }
            if (cacheItem.ProhibitedPermissions.Contains(permission.Name))
            {
                return false;
            }
            //Check for roles
            var roleManager = Resolve<RoleManager>();
            foreach (var roleId in cacheItem.RoleIds)
            {
                if (await roleManager.IsGrantedAsync(roleId, permission))
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 获取用户所有角色
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public virtual Task<List<Role>> GetRolesAsync(User user)
        {
            var query = from userrole in _userRoleRepository.GetAll()
                        join role in _roleRepository.GetAll() on userrole.RoleId equals role.Id
                        where userrole.UserId==user.Id
                        select role;

            return query.ToListAsync();
        }
        /// <summary>
        /// 获取针对用户的权限设置信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public virtual async Task<IList<PermissionGrantInfo>> GetPermissionsAsync(long userId)
        {
            return (await _userPermissionSettingRepository.GetAllListAsync(p => p.UserId == userId))
                .Select(p => new PermissionGrantInfo(p.Name, p.IsGranted))
                .ToList();
        }
        /// <summary>
        /// 获取用户权限缓存
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        private async Task<UserPermissionCacheItem> GetUserPermissionCacheItemAsync(long userId)
        {
            var cacheKey = userId + "@" + (GetCurrentTenantId() ?? 0);
            return await CacheManager.GetUserPermissionCache().GetAsync(cacheKey, async () =>
            {
                var user = await GetByIdAsync(userId);
                if (user == null)
                {
                    return null;
                }

                var newCacheItem = new UserPermissionCacheItem(userId);

                foreach (var role in await GetRolesAsync(user))
                {
                    newCacheItem.RoleIds.Add(role.Id);
                }

                foreach (var permissionInfo in await GetPermissionsAsync(userId))
                {
                    if (permissionInfo.IsGranted)
                    {
                        newCacheItem.GrantedPermissions.Add(permissionInfo.Name);
                    }
                    else
                    {
                        newCacheItem.ProhibitedPermissions.Add(permissionInfo.Name);
                    }
                }

                return newCacheItem;
            });
        }

        /// <summary>
        /// Sets all granted permissions of a user at once.
        /// Prohibits all other permissions.
        /// </summary>
        /// <param name="user">The user</param>
        /// <param name="permissions">Permissions</param>
        public virtual async Task SetGrantedPermissionsAsync(User user, IEnumerable<Permission> permissions)
        {
            var oldPermissions = await GetGrantedPermissionsAsync(user);
            var newPermissions = permissions.ToArray();

            foreach (var permission in oldPermissions.Where(p => !newPermissions.Contains(p)))
            {
                await ProhibitPermissionAsync(user, permission);
            }

            foreach (var permission in newPermissions.Where(p => !oldPermissions.Contains(p)))
            {
                await GrantPermissionAsync(user, permission);
            }
        }

        /// <summary>
        /// Prohibits all permissions for a user.
        /// </summary>
        /// <param name="user">User</param>
        public async Task ProhibitAllPermissionsAsync(User user)
        {
            foreach (var permission in _permissionManager.GetAllPermissions())
            {
                await ProhibitPermissionAsync(user, permission);
            }
        }

        /// <summary>
        /// Resets all permission settings for a user.
        /// It removes all permission settings for the user.
        /// User will have permissions according to his roles.
        /// This method does not prohibit all permissions.
        /// For that, use <see cref="ProhibitAllPermissionsAsync"/>.
        /// </summary>
        /// <param name="user">User</param>
        public async Task ResetAllPermissionsAsync(User user)
        {
            await RemoveAllPermissionSettingsAsync(user);
        }

        

        /// <summary>
        /// Grants a permission for a user if not already granted.
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="permission">Permission</param>
        public virtual async Task GrantPermissionAsync(User user, Permission permission)
        {
            await RemovePermissionAsync(user, new PermissionGrantInfo(permission.Name, false));

            if (await IsGrantedAsync(user.Id, permission))
            {
                return;
            }

            await AddPermissionAsync(user, new PermissionGrantInfo(permission.Name, true));
        }

        /// <summary>
        /// Prohibits a permission for a user if it's granted.
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="permission">Permission</param>
        public virtual async Task ProhibitPermissionAsync(User user, Permission permission)
        {
            await RemovePermissionAsync(user, new PermissionGrantInfo(permission.Name, true));

            if (!await IsGrantedAsync(user.Id, permission))
            {
                return;
            }

            await AddPermissionAsync(user, new PermissionGrantInfo(permission.Name, false));
        }
        private async Task RemoveAllPermissionSettingsAsync(User user)
        {
            await _userPermissionSettingRepository.DeleteAsync(o => o.UserId == user.Id);
        }
        private async Task AddPermissionAsync(User user, PermissionGrantInfo permissionGrant)
        {
            if (await HasPermissionAsync(user.Id, permissionGrant))
            {
                return;
            }

            await _userPermissionSettingRepository.InsertAsync(
                new UserPermissionSetting
                {
                    TenantId = user.TenantId,
                    UserId = user.Id,
                    Name = permissionGrant.Name,
                    IsGranted = permissionGrant.IsGranted
                });
        }
        private async Task RemovePermissionAsync(User user, PermissionGrantInfo permissionGrant)
        {
            await _userPermissionSettingRepository.DeleteAsync(
                permissionSetting => permissionSetting.UserId == user.Id &&
                                     permissionSetting.Name == permissionGrant.Name &&
                                     permissionSetting.IsGranted == permissionGrant.IsGranted
                );
        }
        private async Task<bool> HasPermissionAsync(long userId, PermissionGrantInfo permissionGrant)
        {
            return await _userPermissionSettingRepository.FirstOrDefaultAsync(
                p => p.UserId == userId &&
                     p.Name == permissionGrant.Name &&
                     p.IsGranted == permissionGrant.IsGranted
                ) != null;
        }
        /// <summary>
        /// 删除用户所有权限
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public virtual async Task DelUserPermissions(int userId)
        {
            await _userPermissionSettingRepository.DeleteAsync(o => o.UserId == userId);
        }
        #endregion

        #region 提交处理       

        public override async Task<object> DoAdd(ModuleInfo moduleInfo, IDictionary<string, string> Datas)
        {
            var user=await base.DoAdd(moduleInfo, Datas) as User;
            var roleManager = Resolve<RoleManager>();
            if (moduleInfo.ModuleKey == nameof(Assistant))
            {
                var role =await roleManager.FindByNameAsync(StaticRoleNames.Tenants.Assistant);
                await SetRoles(user, new int[] { role.Id });
                //生成密码并发送邮件
            }else if (moduleInfo.ModuleKey == nameof(Charger))
            {
                var role = await roleManager.FindByNameAsync(StaticRoleNames.Tenants.Charger);
                await SetRoles(user, new int[] { role.Id });
            }
            else if (moduleInfo.ModuleKey == nameof(Miner))
            {
                var role = await roleManager.FindByNameAsync(StaticRoleNames.Tenants.Miner);
                await SetRoles(user, new int[] { role.Id });
            }

            return user;
        }
        #endregion

        #region 模块数据
        public override IQueryable<User> GetFilteredQuery(string moduleKey = "")
        {
            var query = base.GetFilteredQuery(moduleKey);
            var roleManager = Resolve<RoleManager>();
            if (moduleKey == nameof(Assistant))
            {
                var assistantRole = roleManager.FindByNameAsync(StaticRoleNames.Tenants.Assistant).Result;
                query = query.Where(o => o.Roles.Count(r => r.RoleId == assistantRole.Id) > 0);
            }
            else if (moduleKey == nameof(Charger))
            {
                var chargerRole = roleManager.FindByNameAsync(StaticRoleNames.Tenants.Charger).Result;
                query = query.Where(o => o.Roles.Count(r => r.RoleId == chargerRole.Id) > 0);
            }
            else if (moduleKey == nameof(Miner))
            {
                var minerRole = roleManager.FindByNameAsync(StaticRoleNames.Tenants.Miner).Result;
                query = query.Where(o => o.Roles.Count(r => r.RoleId == minerRole.Id) > 0);
            }
            return query;
        }

        public override async Task FillEntityDataAfter(IDictionary<string, object> data, ModuleInfo moduleInfo, object entity)
        {
            var user = entity as User;
            var roles = await GetRolesAsync(user);
            await base.FillEntityDataAfter(data, moduleInfo, entity);
            data["RoleNames"] = string.Join(',', roles.Select(o => o.DisplayName));
            data["IsActive"] = user.IsActive;
            if (moduleInfo.ModuleKey == nameof(Assistant))
            {
                data["InputCaseNumber"] =await Resolve<CaseSourceManager>().GetAll().CountAsync(o => o.CreatorUserId == user.Id);
            }
        }
        #endregion

        #region 登录锁定
        public virtual async Task<bool> IsLockedOutAsync(User user)
        {
            try
            {
                var lockoutCount = await SettingManager.GetSettingValueAsync(Configuration.SettingNames.login_lockoutCount);
                return user.AccessFailedCount >= int.Parse(lockoutCount) && user.LockoutEndDate != null && user.LockoutEndDate.Value > DateTime.Now;
            }
            catch
            {
                return false;
            }
            
        }
        public virtual async Task AccessFailedAsync(User user)
        {
            try
            {
                if(user.LockoutEndDate!=null && user.LockoutEndDate.Value < DateTime.Now)
                {
                    user.AccessFailedCount = 0;
                    user.LockoutEndDate = null;
                }
                var lockoutCount = await SettingManager.GetSettingValueAsync(Configuration.SettingNames.login_lockoutCount);
                var lockoutDuration = await SettingManager.GetSettingValueAsync(Configuration.SettingNames.login_lockoutDuration);
                user.AccessFailedCount++;
                if (user.AccessFailedCount >= int.Parse(lockoutCount))
                {
                    user.LockoutEndDate = DateTime.Now.AddMinutes(int.Parse(lockoutDuration));
                }
            }
            catch
            {

            }
            

        }
        #endregion
        /// <summary>
        /// 获取当前是Host还是Tenant
        /// </summary>
        /// <returns></returns>
        private MultiTenancySides GetCurrentMultiTenancySide()
        {
            if (UnitOfWorkManager.Current != null)
            {
                return MultiTenancy.IsEnabled && !UnitOfWorkManager.Current.GetTenantId().HasValue
                    ? MultiTenancySides.Host
                    : MultiTenancySides.Tenant;
            }

            return AbpSession.MultiTenancySide;
        }
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
