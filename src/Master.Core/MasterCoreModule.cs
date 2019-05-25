using Abp.Authorization;
using Abp.Dependency;
using Abp.Domain.Services;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Timing;
using Master.Localization;
using Master.Timing;
using Abp.Configuration.Startup;
using System;
using Master.Configuration;
using Abp.MultiTenancy;
using Master.MultiTenancy;
using Abp.Reflection;
using System.Collections.Generic;
using Master.Module;
using Abp.Auditing;
using Master.Auditing;

namespace Master
{
    public class MasterCoreModule : AbpModule
    {
        public override void PreInitialize()
        {
            IocManager.Register<MasterConfiguration>();
            IocManager.Register<IRoleManagementConfig, RoleManagementConfig>();

            Configuration.Auditing.IsEnabledForAnonymousUsers = false;
            //启用多租户
            Configuration.MultiTenancy.IsEnabled = true;

            //使所有领域服务方法成为工作单元
            Configuration.UnitOfWork.ConventionalUowSelectors.Add(type => typeof(DomainService).IsAssignableFrom(type));

            Configuration.ReplaceService<ITenantStore, TenantStore>(DependencyLifeStyle.Singleton);
            //配置缓存2小时
            Configuration.Caching.ConfigureAll(cache =>
            {
                cache.DefaultSlidingExpireTime = TimeSpan.FromHours(2);
            });

            //Setting提供者
            Configuration.Settings.Providers.Add<MasterSettingProvider>();

            //替换默认的权限检查
            Configuration.ReplaceService<IPermissionChecker, Master.Authentication.PermissionChecker>(DependencyLifeStyle.Singleton);
            Configuration.ReplaceService<IPermissionManager, Master.Authentication.PermissionManager>(DependencyLifeStyle.Singleton);

            Configuration.ReplaceService<IAuditingStore, AuditLogManager>(DependencyLifeStyle.Singleton);

            MasterLocalizationConfigurer.Configure(Configuration.Localization);
            //配置静态角色
            AppRoleConfig.Configure(IocManager.Resolve<IRoleManagementConfig>());

            //初始化初始字典信息
            InitDictionary();
            //初始化关联数据
            InitRelativeDataProvider();
            //初始化实体标记
            InitStatusProvider();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(MasterCoreModule).GetAssembly());
        }

        public override void PostInitialize()
        {

            IocManager.Resolve<AppTimes>().StartupTime = Clock.Now;

        }

        /// <summary>
        /// 初始化关联数据提供者
        /// </summary>
        private void InitRelativeDataProvider()
        {
            using (var typeFinderWrapper = IocManager.ResolveAsDisposable<ITypeFinder>())
            {
                var types = typeFinderWrapper.Object.Find(o => typeof(IRelativeDataProvider).IsAssignableFrom(o) && !o.IsInterface);
                foreach (var type in types)
                {
                    var interfaceType = type.GetInterfaces()[0];
                    var entityType = interfaceType.GetGenericArguments()[0];

                    var providers = Configuration.Modules.Core().RelativeDataProviders;
                    if (!providers.ContainsKey(entityType))
                    {
                        providers.Add(entityType, new List<Type>());
                    }
                    providers[entityType].Add(type);
                }
            }
        }
        /// <summary>
        /// 初始化实体标记提供者
        /// </summary>
        private void InitStatusProvider()
        {
            using (var typeFinderWrapper = IocManager.ResolveAsDisposable<ITypeFinder>())
            {
                var types = typeFinderWrapper.Object.Find(o => typeof(IStatusProvider).IsAssignableFrom(o) && !o.IsInterface);
                foreach (var type in types)
                {
                    var interfaceType = type.GetInterfaces()[0];
                    var entityType = interfaceType.GetGenericArguments()[0];

                    var statusProviderInstance = Activator.CreateInstance(type) as IStatusProvider;

                    var entityStatusDefinitions = Configuration.Modules.Core().EntityStatusDefinitions;
                    if (!entityStatusDefinitions.ContainsKey(entityType))
                    {
                        entityStatusDefinitions.Add(entityType, new List<StatusDefinition>());
                    }
                    entityStatusDefinitions[entityType].AddRange(statusProviderInstance.GetStatusDefinitions());
                }
            }
        }
        private void InitDictionary()
        {
            //往来单位性质
            Configuration.Modules.Core().Dictionaries.Add(StaticDictionaryNames.UnitNature, new Dictionary<string, string>() { { "1", "客户" },{ "2","供应商"},{ "3", "客户及供应商" } });
            //往来单位供应类别
            Configuration.Modules.Core().Dictionaries.Add(StaticDictionaryNames.SupplierType, new Dictionary<string, string>() { { "采购", "采购" }, { "加工", "加工" }});
            //性别字典
            Configuration.Modules.Core().Dictionaries.Add(StaticDictionaryNames.Sex, new Dictionary<string, string>() { { "男", "男" }, { "女", "女" } });
            //学历字典
            Configuration.Modules.Core().Dictionaries.Add(StaticDictionaryNames.Degree, new Dictionary<string, string>() { { "小学", "小学" }, { "初中", "初中" }, { "高中", "高中" }, { "大学", "大学" }, { "硕士及以上", "硕士及以上" } });
            //婚姻状况
            Configuration.Modules.Core().Dictionaries.Add(StaticDictionaryNames.Marriage, new Dictionary<string, string>() { { "未婚", "未婚" }, { "已婚", "已婚" } });
            //项目类型
            Configuration.Modules.Core().Dictionaries.Add(StaticDictionaryNames.ProjectType, new Dictionary<string, string>() { { "普通", "普通" }});
        }
    }
}