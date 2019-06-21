using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Case
{
    public class CaseReadHistory: CreationAuditedEntity<int>
    {
        public int CaseInitialId { get; set; }
        public virtual CaseInitial CaseInitial { get; set; }
    }
}
