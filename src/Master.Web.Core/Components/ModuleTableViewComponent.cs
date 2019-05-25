using Abp.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Abp.AutoMapper;
using System.Linq.Expressions;
using Master.Module;
using Abp.Domain.Entities;

namespace Master.Web.Components
{
    public class ModuleTableViewComponent : MasterViewComponent
    {
        private IModuleInfoManager _moduleManager;

        

        public ModuleTableViewComponent(IModuleInfoManager moduleManager)
        {
            _moduleManager = moduleManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(ModuleTableViewParam param)
        {            
            //获取模块信息
            var moduleInfo = await _moduleManager.GetModuleInfo(param.ModuleKey);

            if (moduleInfo == null)
            {
                return Content($"modulekey:{param.ModuleKey} not available");
            }
            
            var vm = new ModuleTableViewModel()
            {
                ModuleInfo=moduleInfo,
                ColumnFilter=param.ColumnFilter
            };
            param.MapTo(vm);

            if (string.IsNullOrEmpty(vm.Filter))
            {
                vm.Filter = vm.ID;
            }

            if (string.IsNullOrEmpty(vm.DataUrl))
            {
                //todo:统一进行数据处理
                if (vm.ModuleInfo.IsInterModule)
                {
                    var pluginName = vm.ModuleInfo.GetData<string>("PluginName");
                    if (string.IsNullOrEmpty(pluginName) || pluginName=="core") { pluginName = "app"; }
                    vm.DataUrl = $"/api/services/{pluginName}/{param.ModuleKey}/GetPageResult";
                }
                else
                {
                    vm.DataUrl = $"/api/services/app/ModuleData/GetPageResult?moduleKey="+param.ModuleKey;
                }
            }

            return View(vm);
        }
    }
    [AutoMap(typeof(ModuleTableViewModel))]
    public class ModuleTableViewParam
    {
        public string ID { get; set; }
        public string ModuleKey { get; set; }
        public string DataUrl { get; set; }
        public string Height { get; set; }
        public string Filter { get; set; }
        public string Where { get; set; }
        public string SearchKeys { get; set; }
        public string ClassName { get; set; }
        public bool ShowCheckbox { get; set; } = true;
        public Expression<Func<ColumnInfo,bool>> ColumnFilter { get; set; }
    }

    public class ModuleTableViewModel
    {
        public ModuleInfo ModuleInfo { get; set; }
        public string DataUrl { get; set; }
        public string ID { get; set; }
        public string Height { get; set; }
        public string Filter { get; set; }
        public string Where { get; set; }
        public string SearchKeys { get; set; }
        public string ClassName { get; set; }
        public bool ShowCheckbox { get; set; }
        public Expression<Func<ColumnInfo, bool>> ColumnFilter { get; set; }
    }
}
