using Abp.AutoMapper;
using Master.Module;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Configuration
{
    [AutoMap(typeof(ColumnInfo))]
    public class SettingUIInfo
    {
        public ColumnTypes ColumnType { get; set; }
        public string Renderer { get; set; }
        public string ControlFormat { get; set; }
        public string DictionaryName { get; set; }
        public string Tips { get; set; }
    }

}
