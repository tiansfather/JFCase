using Abp.AutoMapper;
using Abp.Dependency;
using Abp.Reflection;
using Abp.UI;
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
    /// 表单项组件
    /// </summary>
    public class ModuleFormItemViewComponent : MasterViewComponent
    {
        private IDefaultValueParser _defaultValueParser;

        public ITypeFinder TypeFinder { get; set; }
        
        public IIocManager IocManager { get; set; }

        public ModuleFormItemViewComponent(IDefaultValueParser defaultValueParser)
        {
            _defaultValueParser = defaultValueParser;
        }
        public async Task<IViewComponentResult> InvokeAsync(ModuleFormItemViewParam param)
        {

            var vm = new ModuleFormItemViewModel();
            param.MapTo(vm);
            vm.ColumnInfo.Normalize();
            //获取值
            var columnValue =await GetValue(param);

            vm.Value = columnValue;

            
            //获取对应的视图
            var viewname = GetFormItemViewName(param);

            return View(viewname, vm) ;
        }
        /// <summary>
        /// 获取视图名称
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private string GetFormItemViewName(ModuleFormItemViewParam param)
        {
            
            var viewname = param.ColumnInfo.ColumnType.ToString();
            //非主引用使用空视图
            if (param.ColumnInfo.ColumnType == ColumnTypes.Reference && (param.ColumnInfo.ValuePath??"").StartsWith("@"))
            {
                viewname = "Default";
            }
            //自定义控件
            if (param.ColumnInfo.ColumnType == ColumnTypes.Customize)
            {
                var control = param.ColumnInfo.GetCustomizeControl();
                
                if (control == null)
                {
                    Logger.Error("未找到自定义控件：" + param.ColumnInfo.CustomizeControl);
                    throw new UserFriendlyException(L("未找到自定义控件") + param.ColumnInfo.CustomizeControl);
                }
                viewname ="Customize/"+ control.GetViewComponentName();
            }

            //如果是展示页，则视图名称为Text_View
            if (param.FormType == FormType.View)
            {
                viewname = viewname + "_View";
            }

            return viewname;
        }
        private async Task<object> GetValue(ModuleFormItemViewParam param)
        {
            object result=null;
            
            if (param.FormType == FormType.Add)
            {
                //添加表单时需要默认值计算
                result = await _defaultValueParser.Parse(new ColumnReadContext() { ColumnInfo = param.ColumnInfo });
            }
            else if (param.Data != null )
            {
                //如果实体存在，则直接取实体值
                param.Data.TryGetValue(param.ColumnInfo.ColumnKey, out result);
            }

            return result;
        }

    }

    [AutoMap(typeof(ModuleFormItemViewParam))]
    public class ModuleFormItemViewModel
    {
        public string PluginName { get; set; }
        public string ModuleKey { get; set; }
        public ColumnInfo ColumnInfo { get; set; }
        public IDictionary<string, object> Data { get; set; }
        /// <summary>
        /// 对应的值信息
        /// </summary>
        public object Value { get; set; }
    }

    public class ModuleFormItemViewParam
    {
        public string PluginName { get; set; }
        public string ModuleKey { get; set; }
        public ColumnInfo ColumnInfo { get; set; }
        public IDictionary<string,object> Data { get; set; }
        public FormType FormType { get; set; }
    }
}
