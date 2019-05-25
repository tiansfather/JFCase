using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Master.Entity;
using Master.MultiTenancy;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Master.Authentication
{
    public class Role:FullAuditedEntity<int>, IMayHaveTenant,IHaveRemarks
    {
        public virtual int? TenantId { get; set; }
        public virtual Tenant Tenant { get; set; }
        public virtual string Name { get; set; }
        public virtual string DisplayName { get; set; }
        /// <summary>
        /// 静态角色不允许修改
        /// </summary>
        public virtual bool IsStatic { get; set; }
        /// <summary>
        /// 用于给新的用户自动添加角色
        /// </summary>
        [ForeignKey("RoleId")]
        public virtual ICollection<RolePermissionSetting> Permissions { get; set; }
        public virtual bool IsDefault { get; set; }
        public virtual User CreatorUser { get; set; }
        public virtual User LastModifierUser { get; set; }
        public virtual User DeleterUser { get; set; }
        public string Remarks { get; set; }

        public Role()
        {
            Name = Guid.NewGuid().ToString("N");
        }

        public Role(int? tenantId, string displayName)
            : this()
        {
            TenantId = tenantId;
            DisplayName = displayName;
        }

        public Role(int? tenantId, string name, string displayName)
            : this(tenantId, displayName)
        {
            Name = name;
        }
    }
}
