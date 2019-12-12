using Abp.AspNetCore.Mvc.Authorization;
using Abp.Auditing;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Runtime.Security;
using Abp.Runtime.Session;
using Master.Authentication;
using Master.Authentication.External;
using Master.Configuration;
using Master.Controllers;
using Master.Session;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using Master.Entity;
using Master.MultiTenancy;
using Master.Menu;
using Abp.Reflection;
using Abp.Application.Features;
using Master.Models.TokenAuth;
using Master.EntityFrameworkCore;
using Master.Domain;
using Abp.Configuration.Startup;
using Microsoft.AspNetCore.Http;

namespace Master.Web.Controllers
{    
    
    //[AbpMvcAuthorize]
    public class ManagerController : MasterControllerBase
    {
        private ISessionAppService _sessionAppService;
        private readonly UserManager _userManager;
        private readonly IRepository<Setting> _settingRepository;
        private readonly IConfigurationRoot _appConfiguration;
        private readonly IExternalAuthConfiguration _externalAuthConfiguration;


        public IHttpContextAccessor HttpContextAccessor { get; set; }
        public IRepository<BaseTree,int> BaseTreeRepository { get; set; }
        public IRepository<Tenant,int> TenantRepository { get; set; }
        public ITypeFinder TypeFinder { get; set; }
        public TokenAuthController TokenAuthController { get; set; }
        public IDynamicQuery DynamicQuery { get; set; }
        public IAbpStartupConfiguration AbpStartupConfiguration { get; set; }
        public ManagerController(
            ISessionAppService sessionAppService,
            IHostingEnvironment env, 
            UserManager userManager,
            IRepository<Setting> settingRepository, 
            IExternalAuthConfiguration externalAuthConfiguration)
        {
            _userManager = userManager;
            _settingRepository = settingRepository;
            _sessionAppService = sessionAppService;
            _appConfiguration = env.GetAppConfiguration();
            _externalAuthConfiguration = externalAuthConfiguration;
        }
        
        //[AbpMvcAuthorize]
        public async Task<ActionResult> Index()
        {

            if (HttpContext.Session.Get<int?>("LoginInfo") == null)
            {
                Response.Cookies.Delete("token");
                return Redirect("/Account/gmLogin");
            }
            Response.Cookies.Append("Side", "Manager");
            var user = AbpSession.ToUserIdentifier();
            Session.Dto.LoginInformationDto loginInfo;
            try
            {
                loginInfo = await _sessionAppService.GetCurrentLoginInformations();
            }
            catch
            {
                Response.Cookies.Delete("token");
                return Redirect("/Account/gmLogin");
            }
            //默认首页
            if (loginInfo.User.HomeUrl.IsNullOrEmpty())
            {
                loginInfo.User.HomeUrl = "Manager/Default";
            }
            
            return View(loginInfo);
        }
        public IActionResult Default()
        {
            if (AbpSession.UserId == null)
            {
                return Redirect("/Account/gmLogin");
            }
            var viewName= AbpSession.MultiTenancySide == Abp.MultiTenancy.MultiTenancySides.Tenant ? "Default" : "HostDefault";
            return View(viewName);
        }
        /// <summary>
        /// 展示视图
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IActionResult Show(string name)
        {
            if (AbpSession.UserId == null)
            {
                return Redirect("/Account/gmLogin");
            }
            return View(name);
        }

        
        /// <summary>
        /// 用户信息
        /// </summary>
        /// <returns></returns>
        [AbpMvcAuthorize]
        public IActionResult UserInfo()
        {
            var providers = _externalAuthConfiguration.Providers;
            //bool.TryParse(_appConfiguration["Authentication:Wechat:IsEnabled"], out var enableWeChatAuthentication);
            //bool.TryParse(_appConfiguration["Authentication:MiniProgram:IsEnabled"], out var enableMiniProgramAuthentication);
            //ViewBag.EnableWeChatAuthentication = enableWeChatAuthentication;
            //ViewBag.EnableMiniProgramAuthentication = enableMiniProgramAuthentication;
            ViewBag.ExternalAuthProviders = providers;
            return View();
        }
        

        
    }
}