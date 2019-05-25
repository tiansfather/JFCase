using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Module.Attributes
{
    /// <summary>
    /// 标识一个内置模块
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    [AutoMap(typeof(ModuleInfo))]
    public class InterModuleAttribute : Attribute
    {
        public string ModuleName { get; set; }
        public string RequirePermission { get; set; }
        public string RequiredFeature { get; set; }
        public string Icon { get; set; }
        public string Url { get; set; }
        public string SortField { get; set; } = "Id";
        public SortType SortType { get; set; } = SortType.Desc;
        /// <summary>
        /// 基类型 ，如果设置，则生成模块时以基类做为绑定实体
        /// </summary>
        public Type BaseType { get; set; }
        /// <summary>
        /// 是否构建默认列
        /// </summary>
        public bool GenerateDefaultColumns { get; set; } = true;
        /// <summary>
        /// 是否构建默认按钮
        /// </summary>
        public bool GenerateDefaultButtons { get; set; } = true;
        public InterModuleAttribute(string moduleName)
        {
            ModuleName = moduleName;
        }
    }
}
