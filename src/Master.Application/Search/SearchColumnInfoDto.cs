using Abp.AutoMapper;
using Abp.Runtime.Validation;
using Master.Module;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Dto
{
    [AutoMap(typeof(ColumnInfo))]
    public class SearchColumnInfoDto
    {
        public string ValuePath { get; set; }
        public string ColumnKey { get; set; }
        public string ColumnName { get; set; }
        public ColumnTypes ColumnType { get; set; }

    }
}
