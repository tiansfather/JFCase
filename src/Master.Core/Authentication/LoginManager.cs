using Abp;
using Abp.Auditing;
using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Runtime.Security;
using Abp.Timing;
using Master.MultiTenancy;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Master.Authentication
{
    /// <summary>
    /// 登录
    /// </summary>
    public class LoginManager : ITransientDependency
    {
        public IClientInfoProvider ClientInfoProvider { get; set; }
        public IClaimsPrincipalCreator ClaimsPrincipalCreator { get; set; }
        private IUnitOfWorkManager UnitOfWorkManager { get; }
        private IMultiTenancyConfig MultiTenancyConfig { get; }
        private IRepository<Tenant> TenantRepository { get; }
        private IRepository<UserLoginAttempt> UserLoginAttemptRepository { get; }
        //private UserManager UserManager { get; }
        public LoginManager(
            IMultiTenancyConfig multiTenancyConfig,
            IRepository<Tenant> tenantRepository,
            IRepository<UserLoginAttempt> userLoginAttemptRepository,
            IUnitOfWorkManager unitOfWorkManager
            )
        {
            UnitOfWorkManager = unitOfWorkManager;
            //UserManager = userManager;
            TenantRepository = tenantRepository;
            UserLoginAttemptRepository = userLoginAttemptRepository;
            MultiTenancyConfig = multiTenancyConfig;

            ClientInfoProvider = NullClientInfoProvider.Instance;
        }

        private UserManager GetUserManager()
        {
            using (var userManagerWrapper=IocManager.Instance.ResolveAsDisposable<UserManager>())
            {
                return userManagerWrapper.Object;
            }
        }

        #region 第三方验证方式
        [UnitOfWork]
        public virtual async Task<LoginResult> LoginAsync(UserLoginInfo login)
        {
            var result = await LoginAsyncInternal(login);
            await SaveLoginAttempt(result, result.Tenant?.TenancyName, login.ProviderKey + "@" + login.LoginProvider);
            return result;
        }
        protected virtual async Task<LoginResult> LoginAsyncInternal(UserLoginInfo login)
        {
            if (login == null || login.LoginProvider.IsNullOrEmpty() || login.ProviderKey.IsNullOrEmpty())
            {
                throw new ArgumentException("login");
            }



            var user = await GetUserManager().FindAsync(login);
            if (user == null)
            {
                return new LoginResult(LoginResultType.UnknownExternalLogin);
            }
            using (UnitOfWorkManager.Current.SetTenantId(user.TenantId))
            {
                return await CreateLoginResultAsync(user, user.Tenant);
            }

        }
        #endregion

        [UnitOfWork]
        public virtual async Task<LoginResult> LoginAsync(string username, string password, string tenancyName = null)
        {
            var result = await LoginAsyncInternal(username, password, tenancyName);
            await SaveLoginAttempt(result, tenancyName, username);
            return result;
        }
        /// <summary>
        /// 账号密码方式登录
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="tenancyName"></param>
        /// <returns></returns>
        private async Task<LoginResult> LoginAsyncInternal(string username, string password, string tenancyName)
        {
            if (username.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(username));
            }

            if (password.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(password));
            }

            //Get and check tenant
            Tenant tenant = null;
            using (UnitOfWorkManager.Current.SetTenantId(null))
            {
                if (!MultiTenancyConfig.IsEnabled)
                {
                    tenant = await GetDefaultTenantAsync();
                }
                else if (!string.IsNullOrWhiteSpace(tenancyName))
                {
                    tenant = await TenantRepository.FirstOrDefaultAsync(t => t.TenancyName == tenancyName || t.Name==tenancyName);
                    if (tenant == null)
                    {
                        return new LoginResult(LoginResultType.InvalidTenancyName);
                    }

                    if (!tenant.IsActive)
                    {
                        return new LoginResult(LoginResultType.TenantIsNotActive, tenant);
                    }
                }
            }

            var tenantId = tenant == null ? (int?)null : tenant.Id;

            using (UnitOfWorkManager.Current.SetTenantId(tenantId))
            {
                var userManager = GetUserManager();

                var user = await userManager.FindByNameOrPhone(tenantId, username);
                if (user == null)
                {
                    return new LoginResult(LoginResultType.InvalidUserName, tenant);
                }
                if (await userManager.IsLockedOutAsync(user))
                {
                    return new LoginResult(LoginResultType.LockedOut, tenant, user);
                }
                if (!await userManager.CheckPasswordAsync(user, password))
                {
                    if (await TryLockOutAsync(tenantId, user.Id))
                    {
                        return new LoginResult(LoginResultType.LockedOut, tenant, user);
                    }
                    return new LoginResult(LoginResultType.InvalidPassword, tenant, user);
                }

                return await CreateLoginResultAsync(user, tenant);
            }

            
        }
        /// <summary>
        /// 生成登录结果
        /// </summary>
        /// <param name="user"></param>
        /// <param name="tenant"></param>
        /// <returns></returns>
        protected virtual async Task<LoginResult> CreateLoginResultAsync(User user, Tenant tenant = null)
        {
            if (tenant!=null && !tenant.IsActive)
            {
                return new LoginResult(LoginResultType.TenantIsNotActive, tenant);
            }
            if (!user.IsActive)
            {
                return new LoginResult(LoginResultType.UserIsNotActive, tenant);
            }
            //是否是第一次登录
            if (user.LastLoginTime == null)
            {
                user.IsFirstLogin = true;
            }
            else
            {
                user.IsFirstLogin = false;
            }
            user.LastLoginTime = Clock.Now;
            user.AccessFailedCount = 0;
            user.LockoutEndDate = null;

            await GetUserManager().UpdateAsync(user);

            await UnitOfWorkManager.Current.SaveChangesAsync();

            var principal = await ClaimsPrincipalCreator.CreateClaimsPrincipal(user);
            return new LoginResult(
                tenant,
                user,
                principal.Identity as ClaimsIdentity
            );
        }
        protected virtual async Task<bool> TryLockOutAsync(int? tenantId, long userId)
        {
            var userManager = GetUserManager();
            using (var uow = UnitOfWorkManager.Begin(TransactionScopeOption.Suppress))
            {
                using (UnitOfWorkManager.Current.SetTenantId(tenantId))
                {
                    var user = await userManager.GetByIdAsync(userId);

                    await userManager.AccessFailedAsync(user);

                    var isLockOut = await userManager.IsLockedOutAsync(user);

                    await UnitOfWorkManager.Current.SaveChangesAsync();

                    await uow.CompleteAsync();

                    return isLockOut;
                }
            }
        }
        public virtual  async Task<ClaimsPrincipal> CreateClaimsPrincipal(User user)
        {            

            var identity = new ClaimsIdentity(new Claim[] {
                new Claim(AbpClaimTypes.UserId,user.Id.ToString()),
                new Claim("Name",user.Name)
            },"Bearer");
            if (user.TenantId.HasValue)
            {
                identity.AddClaim(new Claim(AbpClaimTypes.TenantId, user.TenantId.ToString()));
            }
            //将用户是否独立加入到claim中
            //identity.AddClaim(new Claim("UserSeparate", user.IsSeparate ? "true" : ""));
            var roles = await GetUserManager().GetRolesAsync(user);
            identity.AddClaim(new Claim(AbpClaimTypes.Role, roles.Count>0?roles[0].Name:""));
            var principal = new ClaimsPrincipal(identity);
            return principal;
        }
        /// <summary>
        /// 获取默认账套
        /// </summary>
        /// <returns></returns>
        protected virtual async Task<Tenant> GetDefaultTenantAsync()
        {
            var tenant = await TenantRepository.FirstOrDefaultAsync(t => t.TenancyName == Tenant.DefaultTenantName);
            if (tenant == null)
            {
                throw new AbpException("There should be a 'Default' tenant if multi-tenancy is disabled!");
            }

            return tenant;
        }
        /// <summary>
        /// 记录入登录日志
        /// </summary>
        /// <param name="loginResult"></param>
        /// <param name="tenancyName"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        protected virtual async Task SaveLoginAttempt(LoginResult loginResult, string tenancyName, string userName)
        {
            using (var uow = UnitOfWorkManager.Begin(TransactionScopeOption.Suppress))
            {
                var tenantId = loginResult.Tenant != null ? loginResult.Tenant.Id : (int?)null;
                using (UnitOfWorkManager.Current.SetTenantId(tenantId))
                {
                    var loginAttempt = new UserLoginAttempt
                    {
                        TenantId = tenantId,
                        TenancyName = tenancyName,

                        UserId = loginResult.User != null ? loginResult.User.Id : (long?)null,
                        UserNameOrPhoneNumber = userName,

                        Result = loginResult.Result,

                        BrowserInfo = ClientInfoProvider.BrowserInfo,
                        ClientIpAddress = ClientInfoProvider.ClientIpAddress,
                        ClientName = ClientInfoProvider.ComputerName,
                    };

                    await UserLoginAttemptRepository.InsertAsync(loginAttempt);
                    await UnitOfWorkManager.Current.SaveChangesAsync();

                    await uow.CompleteAsync();
                }
            }
        }
    }
}
