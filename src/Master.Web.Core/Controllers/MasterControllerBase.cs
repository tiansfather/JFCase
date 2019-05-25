using Abp.AspNetCore.Mvc.Controllers;
using Abp.Authorization;
using Abp.Web.Models;
using Abp.Web.Mvc.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.AutoMapper;
using Abp.Runtime.Caching;
using Master.Module;
using Master.Web.Components;
using Master.Dto;
using Abp.Auditing;

namespace Master.Controllers
{
    public abstract class MasterControllerBase : AbpController
    {
        public ICacheManager CacheManager { get; set; }
        protected MasterControllerBase()
        {
            LocalizationSourceName = MasterConsts.LocalizationSourceName;
        }


        [HttpGet]
        public virtual IActionResult Error(string msg,string detail="")
        {
            var vm = new ErrorViewModel()
            {
                ErrorInfo = new ErrorInfo(msg)
            };
            vm.ErrorInfo.Details = detail;
            return View("Error",vm);
        }

        [HttpGet]
        [DisableAuditing]
        public virtual IActionResult Success(string msg, string detail="")
        {
            var vm = new ErrorViewModel()
            {
                ErrorInfo = new ErrorInfo(msg)
            };
            vm.ErrorInfo.Details = detail;
            return View(vm);
        }
    }

    public abstract class MasterModuleControllerBase : MasterControllerBase
    {
        public IModuleInfoManager ModuleManager { get; set; }

        #region 基于模块的添加修改查看表单

        //[ResponseCache(VaryByQueryKeys =new string[] {"moduleKey" }]
        public virtual IActionResult Add(string modulekey)
        {
            //权限判定
            var permissionName = $"Module.{modulekey}.Button.Add";
            PermissionChecker.Authorize(permissionName);

            var param = new ModuleFormViewParam() { ModuleKey = modulekey };
            return View(param);
        }

        public async virtual Task<IActionResult> Edit(string modulekey, int data)
        {
            //权限判定
            var permissionName = $"Module.{modulekey}.Button.Edit";
            PermissionChecker.Authorize(permissionName);

            var moduleInfo = await ModuleManager.GetModuleInfo(modulekey);
            var formData = (await ModuleManager.GetModuleDataListAsync(moduleInfo, "Id=" + data)).First();

            var param = new ModuleFormViewParam() { ModuleKey = modulekey, Data = formData };
            return View(param);
        }

        public async virtual Task<IActionResult> View(string modulekey, int data, string viewName = "View")
        {
            //权限判定
            var permissionName = $"Module.{modulekey}.Button.View";
            PermissionChecker.Authorize(permissionName);

            var moduleInfo = await ModuleManager.GetModuleInfo(modulekey);
            var formData = (await ModuleManager.GetModuleDataListAsync(moduleInfo, "Id=" + data)).First();

            var param = new ModuleFormViewParam() { ModuleKey = modulekey, Data = formData };
            return View(viewName, param);
        }
        public async virtual Task<IActionResult> MultiEdit(string modulekey, string data, string keys = "")
        {
            //权限判定
            var permissionName = $"Module.{modulekey}.Button.MultiEdit";
            PermissionChecker.Authorize(permissionName);

            var moduleInfo = await ModuleManager.GetModuleInfo(modulekey);
            var formData = new Dictionary<string, object>();
            formData.Add("Ids", data);
            formData.Add("Keys", keys);
            var param = new ModuleFormViewParam() { ModuleKey = modulekey, Data = formData };
            return View(param);
        }
        public async virtual Task<IActionResult> Search(string modulekey)
        {
            var param = new ModuleFormViewParam() { ModuleKey = modulekey };
            var moduleInfo = await ModuleManager.GetModuleInfo(modulekey);
            //所有搜索列
            var searchColumns = moduleInfo.FilterdColumnInfos(FormType.Search).MapTo<List<SearchColumnInfoDto>>();
            ViewData["searchColumns"] = searchColumns;
            return View(param);
        }

        public async virtual Task<IActionResult> SearchItemShow(string modulekey, string columnKey, string value)
        {
            var moduleInfo = await ModuleManager.GetModuleInfo(modulekey);
            var formData = new Dictionary<string, object>();
            formData.Add("ColumnKey", columnKey);
            formData.Add(columnKey, value);
            var param = new ModuleFormViewParam() { ModuleKey = modulekey, Data = formData };
            return View(param);
        }
        #endregion
    }
}
