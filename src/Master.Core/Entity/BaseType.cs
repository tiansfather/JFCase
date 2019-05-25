using Abp.Domain.Entities;
using Master.MultiTenancy;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Master.Entity
{
    /// <summary>
    /// 分类通用基类
    /// </summary>
    public class BaseType : BaseFullEntity, IMayHaveTenant, IHaveSort,IPassivable
    {
        public int? TenantId { get; set; }
        public virtual Tenant Tenant { get; set; }
        public int Sort { get; set; }
        public string Discriminator { get; set; }
        public string Code { get; set; }
        [Required]
        public string Name { get; set; }
        public string BriefName { get; set; }
        public bool IsActive { get;set; }
    }
}
