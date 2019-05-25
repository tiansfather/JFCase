using Abp.AspNetCore;
using Abp.AspNetCore.Configuration;
using Abp.AutoMapper;
using Abp.Dependency;
using Abp.EntityFrameworkCore;
using Abp.EntityFrameworkCore.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Resources.Embedded;
using Master.Configuration;
using Master.EntityFrameworkCore;
using Master.EntityFrameworkCore.Seed;
using Master.MES;
using Master.MES.Dtos;
using Master.Projects;
using Master.WeiXin;
using System;
using System.Reflection;
using Abp.Configuration.Startup;
using Master.Entity;
using Abp.Threading.BackgroundWorkers;
using Master.MES.Jobs;
using Master.MES.Events;
using Master.Localization;

namespace Master
{
    [DependsOn(
        typeof(MasterCoreModule),
        typeof(MasterWeiXinModule),
        typeof(AbpEntityFrameworkCoreModule),
        typeof(AbpAutoMapperModule),
        typeof(AbpAspNetCoreModule))]
    public class MasterMESModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Modules.AbpEfCore().AddDbContext<MESDbContext>(options =>
            {
                if (options.ExistingConnection != null)
                {
                    DbContextOptionsConfigurer.Configure(options.DbContextOptions, options.ExistingConnection);
                }
                else
                {
                    DbContextOptionsConfigurer.Configure(options.DbContextOptions, options.ConnectionString);
                }
            });
            Configuration.Modules.AbpAspNetCore()
                 .CreateControllersForAppServices(
                     typeof(MasterMESModule).GetAssembly()
                 );

            Configuration.Settings.Providers.Add<MESSettingProvider>();
            Configuration.Features.Providers.Add<MESFeatureProvider>();

            Configuration.EmbeddedResources.Sources.Add(
                new EmbeddedResourceSet(
                    "/Views/",
                    Assembly.GetExecutingAssembly(),
                    "Master.Views"
                )
            );
            //替换默认的项目管理类
            Configuration.ReplaceService<IProjectManager, MESProjectManager>(DependencyLifeStyle.Transient);

            //dto映射配置
            Configuration.Modules.AbpAutoMapper().Configurators.Add(config =>
            {
                config.CreateMap<ProcessTask, ProcessTask>()
                      .ForMember(u => u.Id, options => options.Ignore());
                config.CreateMap<ProcessTask, ProcessTaskDto>()
                      .ForMember(u => u.ProjectSN, options => options.MapFrom(input=>input.Part.Project.ProjectSN))
                      .ForMember(u => u.PartName, options => options.MapFrom(input => input.Part.PartName))
                      .ForMember(u=>u.PartSN,options=>options.MapFrom(input=>input.Part.PartSN))
                      .ForMember(u => u.PartSpecification, options => options.MapFrom(input => input.Part.PartSpecification))
                      .ForMember(u => u.PartNum, options => options.MapFrom(input => input.Part.PartNum))
                      .ForMember(u => u.UnitName, options => options.MapFrom(input =>  input.Supplier!=null?input.Supplier.UnitName:"" ))
                      .ForMember(u => u.ProcessTypeName, options => options.MapFrom(input => input.ProcessType!=null?input.ProcessType.ProcessTypeName:""));
                config.CreateMap<ProcessTask, ProcessTaskViewDto>()
                      .ForMember(u => u.ProjectSN, options => options.MapFrom(input => input.Part.Project.ProjectSN))
                      .ForMember(u => u.PartName, options => options.MapFrom(input => input.Part.PartName))
                      .ForMember(u => u.PartSN, options => options.MapFrom(input => input.Part.PartSN))
                      .ForMember(u => u.PartSpecification, options => options.MapFrom(input => input.Part.PartSpecification))
                      .ForMember(u => u.PartNum, options => options.MapFrom(input => input.Part.PartNum))
                      .ForMember(u => u.UnitName, options => options.MapFrom(input => input.Supplier != null ? input.Supplier.UnitName : ""))
                      .ForMember(u => u.ProcessTypeName, options => options.MapFrom(input => input.ProcessType != null ? input.ProcessType.ProcessTypeName : ""));
            });

            IocManager.Register<MESConfiguration>();

            Configuration.Auditing.IsEnabled = false;
            MESLocalizationConfigurer.Configure(Configuration.Localization);
            //模块相关设置
            //系统名称
            Configuration.Modules.WebCore().SoftName = "模来模往";
            //加入通用模板视图
            Configuration.Modules.WebCore().CommonViews.Add("../MES/Common");
            //Configuration.Modules.WeiXin().OAuthBaseUrl = "http://mes.imould.me/get-weixin-code.html";
            //设置微信消息处理
            Configuration.Modules.WeiXin().WeiXinMessageHandlerType = typeof(WeiXinMessageHandler);
        }
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(MasterMESModule).GetAssembly());
        }

        public override void PostInitialize()
        {
            //加入定时任务
            var workManager = IocManager.Resolve<IBackgroundWorkerManager>();
            workManager.Add(IocManager.Resolve<CheckDelayJobs>());

            using (var migratorWrapper = IocManager.ResolveAsDisposable<MESDbMigrator>())
            {
                migratorWrapper.Object.CreateOrMigrateForHost(MESSeedHelper.SeedHostDb);
            }
        }

    }
}
