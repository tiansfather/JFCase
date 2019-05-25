using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Master.Entity;
using Master.MultiTenancy;
using Master.Projects;

namespace Master.MES
{
    /// <summary>
    /// 提醒记录
    /// </summary>
    public class RemindLog: CreationAuditedEntity<int>, IHaveProperty, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        public virtual Tenant Tenant { get; set; }
        [Column(TypeName ="json")]
        public JsonObject<IDictionary<string, object>> Property { get; set; }
        /// <summary>
        /// 提醒类型
        /// </summary>
        public string RemindType { get; set; }
        /// <summary>
        /// 被提醒人名称
        /// </summary>
        public string Name { get; set; }
        public string Message { get; set; }
        public bool? Success { get; set; } 
    }
}
