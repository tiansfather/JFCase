using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Master.Controllers;
using Master.MultiTenancy;
using Master.Web.Models.Account;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Master.Web.Controllers
{
    public class AccountController : MasterControllerBase
    {
        private readonly TenantManager _tenantManager;
        public AccountController(
            TenantManager tenantManager
            )
        {
            _tenantManager = tenantManager;
        }
        public async Task<ActionResult> Login()
        {
            var referer = HttpContext.Request.UrlReferrer();
            //从管理端跳出重定向至管理端
            if (referer.IndexOf("Manager") >= 0)
            {
                HttpContext.Response.Redirect("/Account/gmLogin");
            }
            if (HttpContext.Request.Cookies["Side"] == "Manager")
            {
                
            }
            //生成一个标识存入Ｓｅｓｓｉｏｎ
            var guid = Guid.NewGuid();
            HttpContext.Session.Set("WeChatLoginId", guid);

            ViewBag.Guid = guid.ToString();
            return View();
        }
        public ActionResult GMLogin()
        {

            return View();
        }
        public ActionResult Logout()
        {
            Response.Cookies.Delete("token");
            return Redirect("/Home/Index");
        }
        public ActionResult GMLogout()
        {
            Response.Cookies.Delete("token");
            return RedirectToAction("GMLogin");
        }

        public IActionResult ChangePassword()
        {
            return View();
        }
    }
}