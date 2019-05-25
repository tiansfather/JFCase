using Abp.BackgroundJobs;
using Abp.Domain.Repositories;
using Abp.EntityFrameworkCore.Repositories;
using Master.Authentication;
using Master.Cache;
using Master.Domain;
using Master.Entity;
using Master.EntityFrameworkCore;
using Master.EntityFrameworkCore.Seed.Tenants;
using Master.MES.Jobs;
using Master.Module;
using Master.MultiTenancy;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.MES
{
    public class MESTenantManager:TenantManager
    {
        //public ModuleInfoManager ModuleInfoManager { get; set; }
        //public RoleManager RoleManager { get; set; }
        /// <summary>
        /// 获取账套管理员账号
        /// </summary>
        /// <param name="tenant"></param>
        /// <returns></returns> 
        public virtual async Task<User> GetTenantAdminUser(Tenant tenant)
        {
            //通过手机获取管理用户
            var mobile = tenant.GetPropertyValue<string>("Mobile");
            var adminUser = await Resolve<UserManager>().GetAll().IgnoreQueryFilters().Where(o => o.TenantId == tenant.Id && (o.UserName == mobile || o.PhoneNumber == mobile)).FirstOrDefaultAsync();
            return adminUser;
        }

        public virtual async Task SetActive(int tenantId, bool isActive)
        {
            var tenant = await Repository.GetAsync(tenantId);
            tenant.IsActive = isActive;

            if (isActive)
            {
                //将初始化工作放入后台任务
                await Resolve<IBackgroundJobManager>().EnqueueAsync<InitModuleJobs, InitModuleJobsArgs>(new InitModuleJobsArgs() { TenantId = tenantId });
            }
            

            //如果账套还没有模块,则初始化模块
            //if (!await Resolve<IModuleInfoManager>().GetAll().AnyAsync(o => o.TenantId == tenantId))
            //{
            //    await InitModule(new int[] { tenant.Id });
            //}
            //await InitAdminRole(new int[] { tenantId });

            await CurrentUnitOfWork.SaveChangesAsync();
        }
        /// <summary>
        /// 模块初始化
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public virtual async Task InitModule(IEnumerable<int> ids)
        {
            var moduleInfoManager = Resolve<IModuleInfoManager>();
            foreach (var tenantId in ids)
            {
                var modules = await moduleInfoManager.GetAll().Where(o => o.TenantId == tenantId).ToListAsync();
                foreach (var module in modules)
                {
                    await moduleInfoManager.Repository.HardDeleteAsync(module);
                }
                await CurrentUnitOfWork.SaveChangesAsync();
                var context = Repository.GetDbContext() as MasterDbContext;
                new TenantDefaultModuleBuilder(context, tenantId).Create();
            }

        }

        public virtual async Task InitAdminRole(IEnumerable<int> ids)
        {
            var roleManager = Resolve<RoleManager>();
            var userManager = Resolve<UserManager>();
            var tenants = await GetListByIdsAsync(ids);
            foreach (var tenant in tenants)
            {

                using (CurrentUnitOfWork.SetTenantId(tenant.Id))
                {
                    //通过手机获取管理用户
                    var adminUser = await GetTenantAdminUser(tenant);
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
    }
}
