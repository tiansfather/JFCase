using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Proxying;
using Abp.Dependency;
using Abp.Extensions;
using Abp.Localization;
using Abp.Web.Api.ProxyScripting;
using Abp.Web.Authorization;
using Abp.Web.Features;
using Abp.Web.Localization;
using Abp.Web.MultiTenancy;
using Abp.Web.Navigation;
using Abp.Web.Security;
using Abp.Web.Sessions;
using Abp.Web.Settings;
using Abp.Web.Timing;

namespace Master.Web.Startup
{
    public class ScriptGenerator:ITransientDependency
    {
        public IApiProxyScriptManager ApiProxyScriptManager { get; set; }

        /// <summary>
        /// 生成js脚本
        /// </summary>
        public void GenerateScript()
        {
            GenerateProxyScripts();
        }


        private void GenerateProxyScripts()
        {
            var generationModel = new ApiProxyGenerationModel();
            generationModel.Normalize();
            var script = ApiProxyScriptManager.GetScript(generationModel.CreateOptions());
            string scriptPath = $"{Directory.GetCurrentDirectory()}\\wwwroot\\scripts\\";
            System.IO.Directory.CreateDirectory(scriptPath);
            System.IO.File.WriteAllText(System.IO.Path.Combine(scriptPath, "getall.js"), script);
        }

    }
}
