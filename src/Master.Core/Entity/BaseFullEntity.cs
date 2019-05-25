using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Master.Authentication;
using Master.MultiTenancy;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Master.Entity
{
    /// <summary>
    /// 基类
    /// </summary>
    public abstract class BaseFullEntity : FullAuditedEntity, IExtendableObject,IHaveRemarks,IHaveProperty
    {
        /// <summary>
        /// Abp扩展信息存放
        /// </summary>
        public virtual string ExtensionData { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Remarks { get; set; }
        [ForeignKey("CreatorUserId")]
        public virtual User CreatorUser { get; set; }
        [ForeignKey("LastModifierUserId")]
        public virtual User LastModifierUser { get; set; }
        [ForeignKey("DeleterUserId")]
        public virtual User DeleterUser { get; set; }
        [Column(TypeName ="json")]
        public JsonObject<IDictionary<string, object>> Property { get; set; }
    }

    public abstract class BaseFullEntityWithTenant : BaseFullEntity, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public virtual Tenant Tenant { get; set; }
    }

}
