using System;
using Abp.AspNetCore;
using Abp.Castle.Logging.Log4Net;
using Abp.EntityFrameworkCore;
using Master.EntityFrameworkCore;
using Castle.Facilities.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Microsoft.Extensions.Configuration;
using Master.Configuration;
using Master.Authentication.JwtBearer;
using Abp.PlugIns;
using Senparc.CO2NET.RegisterServices;
using Senparc.Weixin.RegisterServices;
using Microsoft.Extensions.Options;
using Senparc.CO2NET;
using Senparc.Weixin.Entities;
using Senparc.Weixin;
using Senparc.Weixin.MP;
using Hangfire;
using Hangfire.MySql.Core;
using Abp.Hangfire;
using Master.Web.MiddleWares;
using Senparc.Weixin.MP.Containers;
using Master.Json;
using Master.Json.Converters;
using Microsoft.AspNetCore.Http.Features;

namespace Master.Web.Startup
{
    public class Startup
    {
        private readonly IConfigurationRoot _appConfiguration;
        private readonly IHostingEnvironment _env;

        public Startup(IHostingEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            //设置接收文件长度的最大值。
            services.Configure<FormOptions>(x =>
            {
                x.ValueLengthLimit = int.MaxValue;
                x.MultipartBodyLengthLimit = int.MaxValue;
                x.MultipartHeadersLengthLimit = int.MaxValue;
            });
            //Configure DbContext
            services.AddAbpDbContext<MasterDbContext>(options =>
            {
                DbContextOptionsConfigurer.Configure(options.DbContextOptions, options.ConnectionString);
            });
            services.AddSession(options=>options.IdleTimeout=TimeSpan.FromHours(2));//使用Session
            services.AddSignalR();//使用SignalR
            services.AddMvc(options =>
            {
                //options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            }).AddJsonOptions(options => {
                //忽略循环引用
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                //不使用驼峰样式的key
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                //设置时间格式
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd";
            });
            services.PostConfigure<MvcJsonOptions>(options =>
            {
                options.SerializerSettings.ContractResolver = new MyContractResolver();
                //options.SerializerSettings.Converters.Add(new BoolConvert());
            });
            //配置jwt
            AuthConfigurer.Configure(services, _appConfiguration);
            //配置hangfire
            services.AddHangfire(config =>
            {
                config.UseStorage(new MySqlStorage(_appConfiguration.GetConnectionString("HangFire")));//注意，这里使用的是mysql
            });
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Info { Title = "Master API", Version = "v1" });
                options.DocInclusionPredicate((docName, description) => true);
                SwaggerDocHelper.ConfigXmlCommentsPath(options);
                options.CustomSchemaIds(x => x.FullName);
                // Define the BearerAuth scheme that's in use
                //options.AddSecurityDefinition("bearerAuth", new ApiKeyScheme()
                //{
                //    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                //    Name = "Authorization",
                //    In = "header",
                //    Type = "apiKey"
                //});
                ////Assign scope requirements to operations based on AuthorizeAttribute
                //options.OperationFilter<SecurityRequirementsOperationFilter>();
            });

            /*
             * CO2NET 是从 Senparc.Weixin 分离的底层公共基础模块，经过了长达 6 年的迭代优化，稳定可靠。
             * 关于 CO2NET 在所有项目中的通用设置可参考 CO2NET 的 Sample：
             * https://github.com/Senparc/Senparc.CO2NET/blob/master/Sample/Senparc.CO2NET.Sample.netcore/Startup.cs
             */
            //services.Configure<SenparcWeixinSetting>(_appConfiguration.GetSection("SenparcWeixinSetting"));
            services.AddSenparcGlobalServices(_appConfiguration)//Senparc.CO2NET 全局注册
                    .AddSenparcWeixinServices(_appConfiguration);//Senparc.Weixin 注册

            //Configure Abp and Dependency Injection
            return services.AddAbp<MasterWebModule>(options =>
            {
                //Configure Log4Net logging
                options.IocManager.IocContainer.AddFacility<LoggingFacility>(
                    f => f.UseAbpLog4Net().WithConfig("log4net.config")
                );
                var pluginFolder = System.IO.Path.Combine(_env.ContentRootPath, "Plugins");
                if (System.IO.Directory.Exists(pluginFolder))
                {
                    options.PlugInSources.AddFolder(pluginFolder);
                }
                
            });

            
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IOptions<SenparcSetting> senparcSetting, IOptions<SenparcWeixinSetting> senparcWeixinSetting)
        {
            app.UseAbp(); //Initializes ABP framework.
            app.UseSession();
            
            //app.UseSignalR(routes =>
            //{
            //    routes.MapHub<ChatHub>("/signalr-ChatHub");
            //});
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            //自定义中间件
            app.UseThumbMiddleware();

            app.UseStaticFiles();            

            app.UseEmbeddedFiles(); //Allows to expose embedded files to the web!

            app.UseAuthentication();

            app.UseJwtTokenMiddleware();

            ApplicationConfigurer.Configure(app);

            app.UseHangfireServer();
            app.UseHangfireDashboard("/masterhangfire", new DashboardOptions
            {
                Authorization = new[] { new AbpHangfireAuthorizationFilter() }
            });

            app.UseSwagger();
            // Enable middleware to serve swagger-ui assets (HTML, JS, CSS etc.)
            app.UseSwaggerUI(options =>
            {                
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Master API V1");
            }); // URL: /swagger

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            // 启动 CO2NET 全局注册，必须！
            IRegisterService register = RegisterService.Start(env, senparcSetting.Value)
                                                        //关于 UseSenparcGlobal() 的更多用法见 CO2NET Demo：https://github.com/Senparc/Senparc.CO2NET/blob/master/Sample/Senparc.CO2NET.Sample.netcore/Startup.cs
                                                        .UseSenparcGlobal();

            #region 微信相关配置


            /* 微信配置开始
             * 
             * 建议按照以下顺序进行注册，尤其须将缓存放在第一位！
             */

            //注册开始



            //开始注册微信信息，必须！
            register.UseSenparcWeixin(senparcWeixinSetting.Value, senparcSetting.Value)
                //注意：上一行没有 ; 下面可接着写 .RegisterXX()

            #region 注册公众号或小程序（按需）

                //注册公众号（可注册多个）
                .RegisterMpAccount(senparcWeixinSetting.Value, "公众号")

                //除此以外，仍然可以在程序任意地方注册公众号或小程序：
                //AccessTokenContainer.Register(appId, appSecret, name);//命名空间：Senparc.Weixin.MP.Containers
            #endregion

            
            ;
            /* 微信配置结束 */

            #endregion
        }
    }
}
