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
            return RedirectToAction("Login");
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