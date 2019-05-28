using Abp.AspNetCore;
using Abp.AspNetCore.Configuration;
using Abp.AspNetCore.Mvc.Controllers;
using Abp.AspNetCore.Mvc.Proxying;
using Abp.Dependency;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Threading.BackgroundWorkers;
using Abp.Web;
using Abp.Web.Api.ProxyScripting;
using Master.Configuration;
using Master.Core.Jobs;
using Master.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.IO;
using Abp.Configuration.Startup;
using Abp.Web.Authorization;

namespace Master.Web.Startup
{
    [DependsOn(typeof(MasterWebCoreModule), typeof(AbpWebCommonModule))]
    public class MasterWebModule : AbpModule
    {
        private readonly IConfigurationRoot _appConfiguration;

        public MasterWebModule(IHostingEnvironment env)
        {
            _appConfiguration = AppConfigurations.Get(env.ContentRootPath, env.EnvironmentName);
        }

        public override void PreInitialize()
        {
            

            //Configuration.Navigation.Providers.Add<MasterNavigationProvider>();
            Configuration.Modules.AbpAspNetCore()
                .CreateControllersForAppServices(
                    typeof(MasterApplicationModule).GetAssembly()
                );

            //Setting提供者
            Configuration.Settings.Providers.Add<WebSettingProvider>();
            Configuration.ReplaceService<IAuthorizationScriptManager, MyAuthorizationScriptManager>(DependencyLifeStyle.Singleton);
            //加入通用模板视图
            Configuration.Modules.WebCore().CommonViews.Add("../Home/VueFormComponent");
            Configuration.Modules.WebCore().CommonViews.Add("../Home/VueSheetComponent");
            Configuration.Modules.WebCore().CommonViews.Add("../Home/VueFormComponentExtend");
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(MasterWebModule).GetAssembly());
        }

        public override void PostInitialize()
        {
            //加入定时任务
            var workManager = IocManager.Resolve<IBackgroundWorkerManager>();
            workManager.Add(IocManager.Resolve<DeleteOldFileJobs>());
            using (var scriptGeneratorWrapper = IocManager.ResolveAsDisposable<ScriptGenerator>())
            {
                scriptGeneratorWrapper.Object.GenerateScript();
            }


                base.PostInitialize();
        }
    }
}