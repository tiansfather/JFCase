using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Master.Controllers;
using Master.Dto;
using Master.Module;
using Microsoft.AspNetCore.Http;

namespace Master.Web.Mvc.Controllers
{
    [AbpMvcAuthorize]
    public class ModuleInfoController : MasterControllerBase
    {
        public IModuleInfoManager ModuleManager { get; set; }
        #region 模块管理
        public IActionResult Index(int? tenantId)
        {
            if(AbpSession.TenantId==null && tenantId != null)
            {
                HttpContext.Session.Set("TenantId", tenantId);
            }
            return View();
        }
        public async Task<IActionResult> Edit(string data)
        {
            var moduleInfo = await ModuleManager.GetModuleInfo(data);
            return View(moduleInfo);
        }
        public async Task<IActionResult> Column(string data)
        {
            var moduleInfo = await ModuleManager.GetModuleInfo(data);
            var allModuleInfos = await ModuleManager.Repository.GetAllListAsync();
            ViewData["AllModules"] = allModuleInfos;
            return View(moduleInfo);
        }
        public async Task<IActionResult> Button(string data)
        {
            var moduleInfo = await ModuleManager.GetModuleInfo(data);
            return View(moduleInfo);
        }
        #endregion
    }
}