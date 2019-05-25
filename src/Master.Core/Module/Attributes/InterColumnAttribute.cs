using Abp.AutoMapper;
using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Master.Module.Attributes
{
    /// <summary>
    /// 标记一个内置列
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    [AutoMap(typeof(ColumnInfo))]
    public class InterColumnAttribute : Attribute
    {
        public string ColumnName { get; set; }
        public ColumnTypes ColumnType { get; set; } = ColumnTypes.Text;
        public string DisplayFormat { get; set; }
        /// <summary>
        /// radio,select,datetime,date
        /// </summary>
        public string ControlFormat { get; set; }
        public string DictionaryName { get; set; }
        public string VerifyRules { get; set; }
        public string DefaultValue { get; set; }
        public int MaxFileNumber { get; set; } = 1;
        public bool IsShownInList { get; set; } = true;
        public bool IsShownInAdd { get; set; } = true;
        public bool IsShownInEdit { get; set; } = true;
        public bool IsShownInView { get; set; } = true;
        public bool IsShownInMultiEdit { get; set; } = true;
        public bool IsEnableSort { get; set; } = true;
        
        /// <summary>
        /// 是否显示在高级查询页
        /// </summary>
        public bool IsShownInAdvanceSearch { get; set; } = true;
        /// <summary>
        /// 引用数据来源
        /// </summary>
        public RelativeDataType RelativeDataType { get; set; } = RelativeDataType.Default;
        /// <summary>
        /// 关联数据语句
        /// </summary>
        public string RelativeDataString { get; set; }
        public string ReferenceItemTpl { get; set; }
        public  string MaxReferenceNumber { get; set; }
        public  string ReferenceSearchColumns { get; set; }
        public string ReferenceSearchWhere { get; set; }
        public bool EnableAutoComplete { get; set; } = false;
        public bool EnableTotalRow { get; set; } = false;
        public string ExtensionData { get; set; }
        public string Renderer { get; set; }
        /// <summary>
        /// 模板数据
        /// </summary>
        public string Templet { get; set; }
        public string ValuePath { get; set; }
        public string DisplayPath { get; set; }

        public int Sort { get; set; }

        public InterColumnAttribute()
        {

        }
        /// <summary>
        /// 构建列信息
        /// </summary>
        /// <returns></returns>
        public ColumnInfo BuildColumnInfo(PropertyInfo propertyInfo=null)
        {
            var cInfo = this.MapTo<ColumnInfo>();
            cInfo.IsSystemColumn = false;
            cInfo.IsInterColumn = true;

            if (propertyInfo != null)
            {
                cInfo.ColumnKey = propertyInfo.Name;
                if (string.IsNullOrEmpty(cInfo.ValuePath))
                {
                    cInfo.ValuePath = propertyInfo.Name;
                }
            }
            return cInfo;
        }
    }
}
