using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Master.Web.Components
{
    public class ModuleFormSubmitViewComponent: MasterViewComponent
    {
        public  Task<IViewComponentResult> InvokeAsync(ModuleFormSubmitViewParam param)
        {            

            return Task.FromResult(View(param) as IViewComponentResult);
        }
    }

    public class ModuleFormSubmitViewParam
    {
        public string PluginName { get; set; } = "app";
        public string ModuleKey { get; set; }
        public string ButtonKey { get; set; }
        public bool IsCustomModule { get; set; }
    }
}
