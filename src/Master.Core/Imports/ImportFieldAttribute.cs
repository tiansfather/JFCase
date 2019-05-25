using Abp.AutoMapper;
using Master.Module;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Imports
{
    [AttributeUsage(AttributeTargets.Property)]
    [AutoMap(typeof(ImportFieldInfo))]
    public class ImportFieldAttribute : Attribute
    {
        /// <summary>
        /// 字典名称
        /// </summary>
        public string DictionaryName { get; set; }
        /// <summary>
        /// 是否必填
        /// </summary>
        public bool Required { get; set; }
        /// <summary>
        /// 字段类型
        /// </summary>
        public ColumnTypes ColumnTypes { get; set; } = ColumnTypes.Text;
    }
}
