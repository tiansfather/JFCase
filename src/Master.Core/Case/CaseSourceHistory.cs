using Master.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Case
{
    /// <summary>
    /// 判例放回记录
    /// </summary>
    public class CaseSourceHistory:BaseFullEntityWithTenant
    {
        public int CaseSourceId { get; set; }
        public virtual CaseSource CaseSource { get; set; }
        public string Reason { get; set; }
    }
}
