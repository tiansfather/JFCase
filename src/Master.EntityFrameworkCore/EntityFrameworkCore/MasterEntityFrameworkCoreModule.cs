using Abp.AutoMapper;
using Abp.EntityFramework;
using Abp.EntityFrameworkCore;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Master.EntityFrameworkCore.EntityFinder;
using Abp.Configuration.Startup;
using Abp.Domain.Uow;
using Castle.MicroKernel.Registration;
using Master.MultiTenancy;
using Master.EntityFrameworkCore.Seed;
using Abp.EntityFrameworkCore.Configuration;
using Abp.Dependency;
using Abp.Reflection;
using System.Reflection;
using Abp.Collections.Extensions;
using Master.EntityFrameworkCore.CacheRegistrar;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using Master.Configuration;

namespace Master.EntityFrameworkCore
{
    [DependsOn(
        typeof(MasterCoreModule), 
        typeof(AbpEntityFrameworkCoreModule),
        typeof(AbpAutoMapperModule))]
    public class MasterEntityFrameworkCoreModule : AbpModule
    {
        private readonly ITypeFinder _typeFinder;
        private readonly IConfigurationRoot _appConfiguration;
        public bool SkipDbSeed { get; set; }

        public MasterEntityFrameworkCoreModule(ITypeFinder typeFinder, IHostingEnvironment env)
        {
            _typeFinder = typeFinder;
            _appConfiguration = AppConfigurations.Get(env.ContentRootPath, env.EnvironmentName);
        }
        public override void PreInitialize()
        {
            Configuration.ReplaceService(typeof(IConnectionStringResolver), () =>
            {
                IocManager.IocContainer.Register(
                    Component.For<IConnectionStringResolver, IDbPerTenantConnectionStringResolver>()
                        .ImplementedBy<DbPerTenantConnectionStringResolver>()
                        .LifestyleTransient()
                    );
            });
            //使用自定义的实体查找类代替默认的
            Configuration.ReplaceService<IDbContextEntityFinder, MasterDbContextEntityFinder>();

            Configuration.Modules.AbpEfCore().AddDbContext<MasterDbContext>(options =>
            {
                //此行会减缓执行速度，仅供调试使用
                //options.DbContextOptions.UseLoggerFactory(Mlogger);
                if (options.ExistingConnection != null)
                {
                    DbContextOptionsConfigurer.Configure(options.DbContextOptions, options.ExistingConnection);
                }
                else
                {
                    DbContextOptionsConfigurer.Configure(options.DbContextOptions, options.ConnectionString);
                }
            });
        }
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(MasterEntityFrameworkCoreModule).GetAssembly());
            //注册缓存
            RegisterEntityCoreCache();
        }

        public override void PostInitialize()
        {
            using (var migratorWrapper = IocManager.ResolveAsDisposable<MasterDbMigrator>())
            {
                migratorWrapper.Object.CreateOrMigrateForHost(SeedHelper.SeedHostDb);
                //if (bool.TryParse(_appConfiguration["Seed"], out var _))
                //{
                //    migratorWrapper.Object.CreateOrMigrateForHost(SeedHelper.SeedHostDb);
                //}
                //else
                //{
                //    migratorWrapper.Object.CreateOrMigrateForHost(null);
                //}
               
            }
            if (!SkipDbSeed)
            {
                //SeedHelper.SeedHostDb(IocManager);
            }
        }

        private void RegisterEntityCoreCache()
        {
            var dbContextTypes =
                _typeFinder.Find(type =>
                {
                    var typeInfo = type.GetTypeInfo();
                    return typeInfo.IsPublic &&
                           !typeInfo.IsAbstract &&
                           typeInfo.IsClass &&
                           typeof(AbpDbContext).IsAssignableFrom(type);
                });

            if (dbContextTypes.IsNullOrEmpty())
            {
                Logger.Warn("No class found derived from AbpDbContext.");
                return;
            }

            using (IScopedIocResolver scope = IocManager.CreateScope())
            {
                foreach (var dbContextType in dbContextTypes)
                {

                    scope.Resolve<IEntityCoreCacheRegistrar>().RegisterForDbContext(dbContextType, IocManager);

                }

            }
        }
    }
}