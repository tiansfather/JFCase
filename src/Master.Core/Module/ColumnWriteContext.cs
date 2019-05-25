
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Module
{
    /// <summary>
    /// 字段写入上下文，一般用于向数据库更新数据
    /// </summary>
    public class ColumnWriteContext
    {
        public IDictionary<string, string> Datas { get; set; }
        public ColumnInfo ColumnInfo { get; set; }
        public object Entity { get; set; }
    }
}
