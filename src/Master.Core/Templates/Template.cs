using Abp.Domain.Entities;
using Master.Entity;
using Master.MultiTenancy;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Templates
{
    /// <summary>
    /// 模板数据
    /// </summary>
    public class Template : BaseFullEntity, IMayHaveTenant,IPassivable,IHaveStatus
    {
        public const string Status_Default = "Default";

        public int? TenantId { get; set; }
        public virtual Tenant Tenant { get; set; }
        public string TemplateName { get; set; }
        public string TemplateType { get; set; }
        public string TemplateContent { get; set; }
        public bool IsActive { get; set; } = true;
        public string Status { get; set; }
    }
}
