using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.EntityFrameworkCore.Repositories;
using Master.Authentication;
using Master.Cache;
using Master.EntityFrameworkCore;
using Master.EntityFrameworkCore.Seed.Tenants;
using Master.Module;
using Master.MultiTenancy;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.Tenants
{
    [AbpAuthorize]
    public class TenantAppService:MasterAppServiceBase<Tenant,int>
    {
        public ModuleInfoManager ModuleInfoManager { get; set; }
        public RoleManager RoleManager { get; set; }
        /// <summary>
        /// 设置账套的图标
        /// </summary>
        /// <param name="logo"></param>
        /// <returns></returns>
        public virtual async Task SetTenantLogo(string logo)
        {
            var tenant = await GetCurrentTenantAsync();
            tenant.Logo = logo;
        }

        /// <summary>
        /// 设置账套的版本
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="editionId"></param>
        /// <returns></returns>
        public virtual async Task SetEdition(int tenantId,int editionId)
        {
            var tenant = await Manager.GetByIdAsync(tenantId);
            tenant.EditionId = editionId;
        }

        /// <summary>
        /// 设置账套是否激活
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="isActive"></param>
        /// <returns></returns>
        [AbpAuthorize]
        public virtual async Task SetActive(int tenantId, bool isActive)
        {
            var tenant = await Repository.GetAsync(tenantId);
            tenant.IsActive = isActive;
            //如果账套还没有模块,则初始化模块
            if (!await ModuleInfoManager.GetAll().AnyAsync(o => o.TenantId == tenantId))
            {
                await InitModule(new int[] { tenant.Id });
            }
            await CurrentUnitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// 模块初始化
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="addOn">若为true,则保留原有数据更新，否则删除原模块数据</param>
        /// <returns></returns>
        public virtual async Task InitModule(IEnumerable<int> ids,bool addOn=true)
        {
            foreach (var tenantId in ids)
            {
                var modules = await ModuleInfoManager.GetAll().Where(o => o.TenantId == tenantId).ToListAsync();
                if (!addOn)
                {
                    //删除原模块数据
                    await ModuleInfoManager.Repository.HardDeleteAsync(o=>modules.Select(m=>m.Id).Contains(o.Id));
                }
                await CurrentUnitOfWork.SaveChangesAsync();
                var context = Repository.GetDbContext() as MasterDbContext;
                new TenantDefaultModuleBuilder(context, tenantId).Create();
            }

        }


        /// <summary>
        /// 初始化管理员权限
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [AbpAuthorize]
        public virtual async Task InitAdminRole(IEnumerable<int> ids)
        {
            var manager = Manager as TenantManager;
            var userManager = Resolve<UserManager>();
            var roleManager = Resolve<RoleManager>();
            var tenants = await manager.GetListByIdsAsync(ids);
            foreach (var tenant in tenants)
            {
                using (CurrentUnitOfWork.SetTenantId(tenant.Id))
                {
                    //通过手机获取管理用户
                    var adminUser = await manager.GetTenantAdminUser(tenant);
                    //获取管理员权限
                    var adminRole = await roleManager.GetAll().Where(o => o.TenantId == tenant.Id && o.Name == StaticRoleNames.Tenants.Admin).FirstOrDefaultAsync();
                    if (adminRole != null)
                    {
                        await roleManager.GrantAllPermissionsAsync(adminRole);

                        //清空角色权限缓存
                        var cacheKey = adminRole.Id + "@" + (adminRole.TenantId ?? 0);
                        await CacheManager.GetRolePermissionCache().RemoveAsync(cacheKey);
                    }

                    if (adminUser != null)
                    {
                        await userManager.SetRoles(adminUser, new int[] { adminRole.Id });
                    }

                }


            }
        }

        [AbpAuthorize]
        public virtual async Task UpdateField(int tenantId, string field, string value)
        {
            var userManager = Resolve<UserManager>();
            var tenant = await Repository.GetAsync(tenantId);
            switch (field)
            {
                case "tenancyName":
                    tenant.TenancyName = value;
                    break;
                case "name":
                    tenant.Name = value;
                    break;
                case "password":
                    //设置密码
                    //寻找账套中用户名和账套注册手机一样的用户，或者用户名是admin的用户
                    var user = await userManager.GetAll().IgnoreQueryFilters().Where(o => o.TenantId == tenantId && ( o.UserName == User.AdminUserName)).FirstOrDefaultAsync();
                    if (user != null)
                    {
                        await userManager.SetPassword(user, value);
                    }
                    return;
            }
            await Manager.UpdateAsync(tenant);
        }
    }
}
