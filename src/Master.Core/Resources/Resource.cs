using Abp.Domain.Entities;
using Master.Entity;
using Master.MultiTenancy;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Resources
{
    public class Resource : BaseFullEntity, IMayHaveTenant, IPassivable, IHaveStatus,IHaveSort
    {

        public int? TenantId { get; set; }
        public virtual Tenant Tenant { get; set; }
        public string ResourceName { get; set; }
        public string ResourceType { get; set; }
        public string ResourceContent { get; set; }
        public bool IsActive { get; set; } = true;
        public string Status { get; set; }
        public int Sort { get; set; }
    }
}
