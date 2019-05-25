using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Master.Application.Editions;
using Master.Authentication;
using Master.Entity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Master.MultiTenancy

{
    public class Tenant : FullAuditedEntity<int>, IPassivable,IAutoEntity,IHaveProperty
    {
        public virtual Edition Edition { get; set; }
        public virtual int? EditionId { get; set; }
        public Tenant()
        {

        }
        public Tenant(string tenancyName,string name)
        {
            TenancyName = tenancyName;
            Name = name;
        }
        public const string DefaultTenantName = "Default";
        /// <summary>
        /// 账套显示名称
        /// </summary>
        public virtual string TenancyName { get; set; }
        /// <summary>
        /// 账套名称
        /// </summary>
        public virtual string Name { get; set; }
        public virtual string ConnectionString { get; set; }
        public virtual bool IsActive { get; set; } = false;
        public virtual User CreatorUser { get; set; }
        public virtual User LastModifierUser { get; set; }
        public virtual User DeleterUser { get; set; }
        [Column(TypeName ="json")]
        public JsonObject<IDictionary<string, object>> Property { get; set; }
        [NotMapped]
        public string Logo
        {
            get
            {
                return this.GetPropertyValue<string>("Logo");
            }
            set
            {
                this.SetPropertyValue("Logo", value);
            }
        }
    }
    public class TenantEntityMapConfiguration : EntityMappingConfiguration<Tenant>
    {
        public override void Map(EntityTypeBuilder<Tenant> b)
        {
            b.HasOne(p => p.DeleterUser)
                .WithMany()
                .HasForeignKey(p => p.DeleterUserId);

            b.HasOne(p => p.CreatorUser)
                .WithMany()
                .HasForeignKey(p => p.CreatorUserId);

            b.HasOne(p => p.LastModifierUser)
                .WithMany()
                .HasForeignKey(p => p.LastModifierUserId);
        }
    }
}
