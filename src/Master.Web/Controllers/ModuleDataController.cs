using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Master.Controllers;
using Master.Module;
using Microsoft.AspNetCore.Mvc;

namespace Master.Web.Mvc.Controllers
{
    [AbpMvcAuthorize]
    public class ModuleDataController : MasterModuleControllerBase
    {
        private readonly IModuleInfoManager _moduleInfoManager;
        public ModuleDataController(IModuleInfoManager moduleInfoManager)
        {
            _moduleInfoManager = moduleInfoManager;
        }
        public async Task<IActionResult> Index(string moduleKey)
        {
            var moduleInfo = await _moduleInfoManager.GetModuleInfo(moduleKey);
            if (moduleInfo == null)
            {
                throw new Exception($"Module {moduleKey} not found");
            }
            if (moduleInfo.IsInterModule)
            {
                throw new Exception($"Module {moduleKey} is inter Module");
            }

            return View(moduleInfo);
        }

        /// <summary>
        /// 选择关联模块数据
        /// </summary>
        /// <param name="moduleKey"></param>
        /// <returns></returns>
        public async Task<IActionResult> RelativeSelect(string moduleKey, string columnKey)
        {
            var moduleInfo = await _moduleInfoManager.GetModuleInfo(moduleKey);
            if (moduleInfo == null)
            {
                throw new Exception($"Module {moduleKey} not found");
            }

            return View(moduleInfo);
        }
    }
}