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
using System.IO;
using Microsoft.AspNetCore.Http;
using System.Web;
using System.Text;
using System.IO.Compression;

namespace Master.Web.Controllers
{    
    

    public class HomeController : MasterControllerBase
    {
        private ISessionAppService _sessionAppService;
        private readonly UserManager _userManager;
        private readonly IRepository<Setting> _settingRepository;
        private readonly IConfigurationRoot _appConfiguration;
        private readonly IExternalAuthConfiguration _externalAuthConfiguration;


        public IRepository<BaseTree,int> BaseTreeRepository { get; set; }
        public IRepository<Tenant,int> TenantRepository { get; set; }
        public ITypeFinder TypeFinder { get; set; }
        public TokenAuthController TokenAuthController { get; set; }
        public IDynamicQuery DynamicQuery { get; set; }
        public IAbpStartupConfiguration AbpStartupConfiguration { get; set; }
        public HomeController(
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
        
        [AbpMvcAuthorize]
        public async Task<ActionResult> Main()
        {
            var user = AbpSession.ToUserIdentifier();
            Session.Dto.LoginInformationDto loginInfo;
            try
            {
                loginInfo = await _sessionAppService.GetCurrentLoginInformations();
            }
            catch
            {
                Response.Cookies.Delete("token");
                return Redirect("/Home/Index");
            }
            Logger.Info("登录用户:" + loginInfo.User.Id.ToString()+","+ string.Join(',',loginInfo.User.RoleNames));
            //仅矿工可进入首页
            if (!loginInfo.User.RoleNames.Contains(StaticRoleNames.Tenants.Miner))
            {
                Response.Cookies.Delete("token");
                return Redirect("/Home/Index?msg="+HttpUtility.UrlEncode( "仅矿工可以进入首页"));
            }
            //默认首页
            if (loginInfo.User.HomeUrl.IsNullOrEmpty())
            {
                loginInfo.User.HomeUrl = "Home/Default";
            }
            return View(loginInfo);
        }
        [AbpMvcAuthorize]
        public IActionResult Me()
        {
            return View();
        }
        /// <summary>
        /// 工作台
        /// </summary>
        /// <returns></returns>
        [AbpMvcAuthorize]
        public IActionResult Workbench()
        {
            return View();
        }
        /// <summary>
        /// 案源库
        /// </summary>
        /// <returns></returns>
        [AbpMvcAuthorize]
        public IActionResult Source()
        {
            return View();
        }
        /// <summary>
        /// 案例加工
        /// </summary>
        /// <returns></returns>
        [AbpMvcAuthorize]
        public IActionResult Process(string encrypedId)
        {
            var id = int.Parse(SimpleStringCipher.Instance.Decrypt(encrypedId));
            ViewBag.Id = id;
            return View();
        }
        /// <summary>
        /// 我的案例
        /// </summary>
        /// <returns></returns>
        [AbpMvcAuthorize]
        public IActionResult MyCase()
        {
            return View();
        }
        // <summary>
        /// 我的精品
        /// </summary>
        /// <returns></returns>
        [AbpMvcAuthorize]
        public IActionResult MyArt()
        {
            return View();
        }
        /// <summary>
        /// 看观点
        /// </summary>
        /// <returns></returns>
        [AbpMvcAuthorize]
        public IActionResult ViewPoint()
        {
            return View();
        }
        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            //生成一个标识存入Ｓｅｓｓｉｏｎ
            var guid = Guid.NewGuid();
            HttpContext.Session.Set("WeChatLoginId", guid);

            ViewBag.Guid = guid.ToString();
            return View();
        }
        /// <summary>
        /// 查案例
        /// </summary>
        /// <returns></returns>
        [AbpMvcAuthorize]
        public IActionResult ViewCase()
        {
            return View();
        }
        public IActionResult Default()
        {
            var viewName= AbpSession.MultiTenancySide == Abp.MultiTenancy.MultiTenancySides.Tenant ? "Default" : "HostDefault";
            return View(viewName);
        }
        
        public IActionResult Zip()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            ZipFile.ExtractToDirectory(@"G:\归档1.zip", @"G:\1", Encoding.GetEncoding("GB2312"));
            var names=Common.ZipHelper.GetFileNames(@"G:\归档1.zip");
            return Content(string.Join(",", names));
        }
        public async Task<IActionResult> Test()
        {
            await Common.EmailHelper.SendMailAsync(
                SettingManager.GetSettingValueAsync(SettingNames.mail_smtpServer).Result,
                Convert.ToBoolean(SettingManager.GetSettingValueAsync(SettingNames.mail_ssl).Result),
                SettingManager.GetSettingValueAsync(SettingNames.mail_username).Result,
                SettingManager.GetSettingValueAsync(SettingNames.mail_pwd).Result,
                SettingManager.GetSettingValueAsync(SettingNames.mail_nickname).Result,
                SettingManager.GetSettingValueAsync(SettingNames.mail_fromname).Result,
                "1018630493@qq.com", "Are you ready?", "Wating for you in the Conference");

            return Content("ok");
        }

        public IActionResult Session()
        {
            var user = HttpContext.Session.Get<User>("LoginInfo");
            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(user));
        }
    }
}