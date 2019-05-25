using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Master.Entity;
using Master.MultiTenancy;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations.Schema;

namespace Master.Authentication
{
    [Table("Permissions")]
    public abstract class PermissionSetting : CreationAuditedEntity<int>, IMayHaveTenant
    {
        public virtual int? TenantId { get; set; }
        public virtual Tenant Tenant { get; set; }
        /// <summary>
        /// 权限名称
        /// </summary>
        public virtual string Name { get; set; }
        /// <summary>
        /// 是否已分配
        /// </summary>
        public virtual bool IsGranted { get; set; }
        protected PermissionSetting()
        {
            IsGranted = true;
        }
    }

    public class PermissionSettingEntityMapConfiguration : EntityMappingConfiguration<PermissionSetting>
    {
        public override void Map(EntityTypeBuilder<PermissionSetting> b)
        {
            b.HasIndex(e => new { e.TenantId, e.Name });
        }
    }
}