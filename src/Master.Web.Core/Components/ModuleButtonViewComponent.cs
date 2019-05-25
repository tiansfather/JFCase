using Master.Module;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.Web.Components
{
    /// <summary>
    /// 模块按钮组
    /// </summary>
    public class ModuleButtonViewComponent: MasterViewComponent
    {
        public IModuleInfoManager _moduleManager;



        public ModuleButtonViewComponent(IModuleInfoManager moduleManager)
        {
            _moduleManager = moduleManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(string moduleKey)
        {
            //获取模块信息
            var moduleInfo = await _moduleManager.GetModuleInfo(moduleKey);

            if (moduleInfo == null)
            {
                return Content($"modulekey:{moduleKey} not available");
            }

            return View(moduleInfo);
        }
    }
}
