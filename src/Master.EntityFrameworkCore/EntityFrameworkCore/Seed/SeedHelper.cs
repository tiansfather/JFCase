using Abp.Authorization;
using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore.Uow;
using Abp.MultiTenancy;
using Master.Authentication;
using Master.EntityFrameworkCore.Seed.Tenants;
using Master.MultiTenancy;
using Master.Organizations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace Master.EntityFrameworkCore.Seed
{
    public static class SeedHelper
    {
        public static void SeedHostDb(IIocResolver iocResolver)
        {
            WithDbContext<MasterDbContext>(iocResolver, SeedHostDb);
        }

        public static void SeedHostDb(MasterDbContext context)
        {
            //context.Database.Migrate();
            context.SuppressAutoSetTenantId = true;
            //构建管理账号
            CreateHostRoleUser(context);
            //构建默认账套
            CreateDefaultTenant(context);
            //构建账套默认角色用户
            CreateTenantRoleUser(context, 1);
            //构建默认组织架架
            CreateTenantOrganization(context, 1);
            //构建默认模块数据
            new TenantDefaultModuleBuilder(context, 1).Create();

        }
        private static void CreateTenantOrganization(MasterDbContext context,int tenantId)
        {
            //根组织
            if (context.Organization.Count()==0)
            {
                context.Organization.Add(new Organization("系统管理") { Code="00001",TenantId=tenantId});
                context.SaveChanges();
            }
        }
        private static void CreateHostRoleUser(MasterDbContext context)
        {
            //管理员角色
            var adminRole = context.Role.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == null && r.Name == StaticRoleNames.Host.Admin);
            if (adminRole == null)
            {
                adminRole = context.Role.Add(new Role(null, StaticRoleNames.Host.Admin, StaticRoleNames.Host.Admin) { IsStatic = true }).Entity;
                context.SaveChanges();
            }
            // 使管理员有所有权限

            var grantedPermissions = context.Permission.IgnoreQueryFilters()
                .OfType<RolePermissionSetting>()
                .Where(p => p.TenantId == null && p.RoleId == adminRole.Id)
                .Select(p => p.Name)
                .ToList();

            using (var provider = IocManager.Instance.ResolveAsDisposable<IPermissionManager>())
            {
                var permissions = provider.Object.GetAllPermissions(MultiTenancySides.Host);
                permissions = permissions.Where(p => !grantedPermissions.Contains(p.Name)).ToList();


                if (permissions.Any())
                {
                    context.Permission.AddRange(
                        permissions.Select(permission => new RolePermissionSetting
                        {
                            TenantId = null,
                            Name = permission.Name,
                            IsGranted = true,
                            RoleId = adminRole.Id
                        })
                    );
                    context.SaveChanges();
                }
            }
            //管理用户
            var adminUser = context.User.IgnoreQueryFilters().FirstOrDefault(u => u.TenantId == null && u.UserName == User.AdminUserName);
            if (adminUser == null)
            {
                adminUser = User.CreateHostAdminUser();
                context.User.Add(adminUser);
                context.SaveChanges();
                // Assign Admin role to admin user
                context.UserRole.Add(new UserRole(null, adminUser.Id, adminRole.Id));
                context.SaveChanges();
            }
        }
        private static void CreateTenantRoleUser(MasterDbContext context, int tenantId)
        {
            //管理员角色
            var adminRole = context.Role.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == tenantId && r.Name == StaticRoleNames.Tenants.Admin);
            if (adminRole == null)
            {
                adminRole = context.Role.Add(new Role(tenantId, StaticRoleNames.Tenants.Admin, "系统管理员") { IsStatic = true }).Entity;                
            }
            //系统助理角色
            var assistantRole = context.Role.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == tenantId && r.Name == StaticRoleNames.Tenants.Assistant);
            if (assistantRole == null)
            {
                assistantRole = context.Role.Add(new Role(tenantId, StaticRoleNames.Tenants.Assistant, "系统助理") { IsStatic = true }).Entity;
            }
            //业务总管角色
            var managerRole = context.Role.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == tenantId && r.Name == StaticRoleNames.Tenants.Manager);
            if (managerRole == null)
            {
                managerRole = context.Role.Add(new Role(tenantId, StaticRoleNames.Tenants.Manager, "业务总管") { IsStatic = true }).Entity;
            }
            //业务主管角色
            var chargerRole = context.Role.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == tenantId && r.Name == StaticRoleNames.Tenants.Charger);
            if (chargerRole == null)
            {
                chargerRole = context.Role.Add(new Role(tenantId, StaticRoleNames.Tenants.Charger, "业务主管") { IsStatic = true }).Entity;
            }
            //矿工角色
            var minerRole = context.Role.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == tenantId && r.Name == StaticRoleNames.Tenants.Miner);
            if (minerRole == null)
            {
                minerRole = context.Role.Add(new Role(tenantId, StaticRoleNames.Tenants.Miner, "矿工") { IsStatic = true }).Entity;
            }
            context.SaveChanges();
            // 使管理员有所有权限

            var grantedPermissions = context.Permission.IgnoreQueryFilters()
                .OfType<RolePermissionSetting>()
                .Where(p => p.TenantId == tenantId && p.RoleId == adminRole.Id)
                .Select(p => p.Name)
                .ToList();

            using (var provider = IocManager.Instance.ResolveAsDisposable<IPermissionManager>())
            {
                var permissions = provider.Object.GetAllPermissions(MultiTenancySides.Tenant);
                permissions=permissions.Where(p=>!grantedPermissions.Contains(p.Name)).ToList();


                if (permissions.Any())
                {
                    context.Permission.AddRange(
                        permissions.Select(permission => new RolePermissionSetting
                        {
                            TenantId = tenantId,
                            Name = permission.Name,
                            IsGranted = true,
                            RoleId = adminRole.Id
                        })
                    );
                    context.SaveChanges();
                }
            }
            //管理用户
            var adminUser = context.User.IgnoreQueryFilters().FirstOrDefault(u => u.TenantId == tenantId && u.UserName == User.AdminUserName);
            if (adminUser == null)
            {
                adminUser = User.CreateTenantAdminUser(tenantId);
                context.User.Add(adminUser);
                context.SaveChanges();
                // Assign Admin role to admin user
                context.UserRole.Add(new UserRole(tenantId, adminUser.Id, adminRole.Id));
                context.SaveChanges();
            }
        }

        private static void CreateDefaultTenant(MasterDbContext context)
        {
            var defaultTenant = context.Set<Tenant>().IgnoreQueryFilters().FirstOrDefault(t => t.TenancyName == Tenant.DefaultTenantName);
            if (defaultTenant == null)
            {
                defaultTenant = new Tenant(Tenant.DefaultTenantName, Tenant.DefaultTenantName);
                defaultTenant.IsActive = true;
                //defaultTenant.ConnectionString = SimpleStringCipher.Instance.Encrypt("Server=localhost; Database=MasterDb_Tenant_"+AbpTenantBase.DefaultTenantName+"; User Id=skynetsoft;password=skynetsoft");

                //var defaultEdition = _context.Editions.IgnoreQueryFilters().FirstOrDefault(e => e.Name == EditionManager.DefaultEditionName);
                //if (defaultEdition != null)
                //{
                //    defaultTenant.EditionId = defaultEdition.Id;
                //}
                context.Set<Tenant>().Add(defaultTenant);
                context.SaveChanges();
            }
        }

        private static void WithDbContext<TDbContext>(IIocResolver iocResolver, Action<TDbContext> contextAction)
            where TDbContext : DbContext
        {
            using (var uowManager = iocResolver.ResolveAsDisposable<IUnitOfWorkManager>())
            {
                using (var uow = uowManager.Object.Begin(TransactionScopeOption.Suppress))
                {
                    var context = uowManager.Object.Current.GetDbContext<TDbContext>(MultiTenancySides.Host);

                    contextAction(context);

                    uow.Complete();
                }
            }
        }
    }
}
