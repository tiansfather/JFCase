using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Configuration;
using Master.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Master.Web.Controllers
{
    [AbpMvcAuthorize]
    public class SettingController : MasterControllerBase
    {
        public ISettingDefinitionManager SettingDefinitionManager { get; set; }
        
        public async Task<IActionResult> Index()
        {
            var allSettingValues = await SettingManager.GetAllSettingValuesAsync();
            var settingValueDic=allSettingValues.ToDictionary(o => o.Name, o => (object)o.Value);
            var settings=SettingDefinitionManager.GetAllSettingDefinitions();
            ViewBag.SettingValues = settingValueDic;
            return View(settings);
        }
    }
}