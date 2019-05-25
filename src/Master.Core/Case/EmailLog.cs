using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Case
{
    public class EmailLog : CreationAuditedEntity<int>
    {
        public string ToEmail { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Message { get; set; }
        public bool? Success { get; set; }
    }
}
