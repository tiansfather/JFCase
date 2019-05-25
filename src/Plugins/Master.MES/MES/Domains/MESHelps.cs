using Abp.Domain.Entities;
using Master.Entity;
using Master.MultiTenancy;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.MES
{
    public class MESHelps : BaseFullEntity, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        public virtual Tenant Tenant { get; set; }
        public string HelpTitle { get; set; }
        public string HelpContent { get; set; }
        public string MenuName { get; set; }
        public string MenuDisplayName { get; set; }
    }
}
