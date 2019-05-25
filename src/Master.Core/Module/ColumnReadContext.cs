
using System;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Text;

namespace Master.Module
{
    /// <summary>
    /// 字段读取上下文，一般用于从数据库读取数据后的处理
    /// </summary>
    public class ColumnReadContext
    {
        public IDictionary<string,object> Entity { get; set; }
        public ColumnInfo ColumnInfo { get; set; }
    }
}
