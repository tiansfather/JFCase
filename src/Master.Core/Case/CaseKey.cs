using Master.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Case
{
    public class CaseKey:BaseFullEntityWithTenant
    {
        public string KeyName { get; set; }
        public string KeyValue { get; set; }
        public int KeyNodeId { get; set; }
        public virtual BaseTree KeyNode { get; set; }
        public int? CaseInitialId { get; set; }
        public virtual CaseInitial CaseInitial { get; set; }
        public int? CaseFineId { get; set; }
        public virtual CaseFine CaseFine { get; set; }
    }
}
