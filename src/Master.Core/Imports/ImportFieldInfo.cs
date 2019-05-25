using Master.Module;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Imports
{
    /// <summary>
    /// 导入列信息
    /// </summary>
    public class ImportFieldInfo
    {
        /// <summary>
        /// 对应导入数据的列号
        /// </summary>
        public int? FieldIndex { get; set; }
        /// <summary>
        /// 字段名
        /// </summary>
        public string FieldName { get; set; }
        /// <summary>
        /// 显示名称
        /// </summary>
        public string DisplayName { get; set; }
        public Dictionary<string,string> AvailableValues { get; set; }
        /// <summary>
        /// 是否必填
        /// </summary>
        public bool Required { get; set; }
        /// <summary>
        /// 字段类型
        /// </summary>
        public ColumnTypes ColumnTypes { get; set; }
    }
}
