using Abp.AspNetCore;
using Abp.AspNetCore.Configuration;
using Abp.Dependency;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Resources.Embedded;
using Master.Authentication.External;
using Master.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Senparc.Weixin;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Master.WeiXin
{
    [DependsOn(
        typeof(MasterCoreModule),
        typeof(AbpAspNetCoreModule))]
    public class MasterWeiXinModule : AbpModule
    {
        private readonly IConfigurationRoot _appConfiguration;

        public MasterWeiXinModule(IHostingEnvironment env)
        {
            _appConfiguration = AppConfigurations.Get(env.ContentRootPath, env.EnvironmentName);
        }

        public override void PreInitialize()
        {

            Configuration.Modules.AbpAspNetCore()
                 .CreateControllersForAppServices(
                     typeof(MasterWeiXinModule).GetAssembly()
                 );

            Configuration.EmbeddedResources.Sources.Add(
                new EmbeddedResourceSet(
                    "/Views/",
                    Assembly.GetExecutingAssembly(),
                    "Master.Views"
                )
            );

            IocManager.Register<WeiXinConfiguration>();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(MasterWeiXinModule).GetAssembly());

            //配置第三方登录
            ConfigurationExternalAuthProviders();

            //配置微信文件上传
            ConfigurationUploadProviders();
        }

        private void ConfigurationUploadProviders()
        {
            if (bool.TryParse(_appConfiguration["Upload:Wechat"], out var _))
            {

                Configuration.Modules.WebCore().UploadProviders.Add(new Web.Configuration.UploadProvider()
                {
                    ProviderName="微信上传",
                    ProviderUrl="/WeiXin/ScanUpload"
                });
            }
        }

        private void ConfigurationExternalAuthProviders()
        {
            var externalAuthConfiguration = IocManager.Resolve<IExternalAuthConfiguration>();
            if (bool.TryParse(_appConfiguration["Authentication:Wechat:IsEnabled"],out var _))
            {
                
                externalAuthConfiguration.Providers.Add(
                    new ExternalLoginProviderInfo(
                        WeChatAuthProviderApi.Name,
                        Config.SenparcWeixinSetting.WeixinAppId,
                        Config.SenparcWeixinSetting.WeixinAppSecret,
                        typeof(WeChatAuthProviderApi),
                        "/WeiXin/Login",
                        "/Weixin/BindLogin",
                        new { Icon= "layui-icon layui-icon-login-wechat", DisplayName = "微信" }
                    )
                );
            }
            if (bool.TryParse(_appConfiguration["Authentication:MiniProgram:IsEnabled"], out var _))
            {

                externalAuthConfiguration.Providers.Add(
                    new ExternalLoginProviderInfo(
                        MiniProgramAuthProviderApi.Name,
                        Config.SenparcWeixinSetting.WxOpenAppId,
                        Config.SenparcWeixinSetting.WxOpenAppSecret,
                        typeof(MiniProgramAuthProviderApi),
                        "",
                        "/MiniProgram/BindLogin",
                        new { Icon = "layui-icon layui-icon-template-1",DisplayName="小程序" }
                    )
                );
            }
        }

        public override void PostInitialize()
        {
            base.PostInitialize();
        }
    }
}
