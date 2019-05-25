using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Master.Entity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Master.Configuration
{
    public class Setting : AuditedEntity<int>, IMayHaveTenant
    {
        public virtual int? TenantId { get; set; }
        public virtual long? UserId { get; set; }
        [Required]
        public virtual string Name { get; set; }
        public virtual string Value { get; set; }

        public Setting()
        {

        }
        public Setting(int? tenantId, long? userId, string name, string value)
        {
            TenantId = tenantId;
            UserId = userId;
            Name = name;
            Value = value;
        }
    }

    public class SettingEntityMapConfiguration : EntityMappingConfiguration<Setting>
    {
        public override void Map(EntityTypeBuilder<Setting> b)
        {
            b.HasIndex(e => new { e.TenantId, e.Name });
        }

    }
}
