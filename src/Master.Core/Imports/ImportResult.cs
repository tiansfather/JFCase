using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Imports
{
    /// <summary>
    /// 导入结果
    /// </summary>
    public class ImportResult
    {
        public bool Success { get; set; }
        public List<ImportResultDetail> ImportResultDetails { get; set; } = new List<ImportResultDetail>();
    }

    public class ImportResultDetail
    {
        public bool Success { get; set; }
        public int Row { get; set; }
        public string FieldName { get; set; }
        public string Message { get; set; }
    }
}
