using Abp.Domain.Entities.Auditing;
using Master.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Case
{
    public class TreeLabel : CreationAuditedEntity<int>
    {
        public int BaseTreeId { get; set; }
        public virtual BaseTree BaseTree { get; set; }
        public int LabelId { get; set; }
        public virtual Label Label { get; set; }
    }
}
