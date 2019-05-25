using Abp.AutoMapper;
using Master.Module;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Master.Web.Components
{
    public class ModuleFormViewComponent : MasterViewComponent
    {
        public IModuleInfoManager _moduleManager;

        public ModuleFormViewComponent(IModuleInfoManager moduleManager)
        {
            _moduleManager = moduleManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(ModuleFormViewParam param)
        {
            //获取模块信息
            var moduleInfo = await _moduleManager.GetModuleInfo(param.ModuleKey);

            if (moduleInfo == null)
            {
                return Content($"modulekey:{param.ModuleKey} not available");
            }

            var vm = new ModuleFormViewModel() { ModuleInfo= moduleInfo };
            param.MapTo(vm);

            //使用默认视图
            if (vm.FormType == FormType.Add || vm.FormType==FormType.Edit )
            {
                return View( vm);
            }
            //批量修改需要特殊处理
            if (vm.FormType == FormType.MultiEdit && !string.IsNullOrEmpty(vm.Data["Keys"].ToString()))
            {
                //如果有keys说明是第二步
                return View("MultiEdit_Form", vm);
            }
            return View(vm.FormType.ToString(),vm);
        }
    }
    [AutoMap(typeof(ModuleFormViewParam))]
    public class ModuleFormViewModel
    {
        public ModuleInfo ModuleInfo { get; set; }
        public FormType FormType { get; set; }
        public FormViewMode FormViewMode { get; set; }
        public IDictionary<string, object> Data { get; set; }
    }
    public class ModuleFormViewParam
    {
        public string ModuleKey { get; set; }
        /// <summary>
        /// 表单类型：添加、修改、查看
        /// </summary>
        public FormType FormType { get; set; }
        /// <summary>
        /// 表单展示风格
        /// </summary>
        public FormViewMode FormViewMode { get; set; } = FormViewMode.Default;
        public IDictionary<string,object> Data { get; set; }

    }

    
}
