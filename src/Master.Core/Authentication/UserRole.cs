using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Master.Entity;
using Master.MultiTenancy;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Authentication
{
    public class UserRole : CreationAuditedEntity<int>, IMayHaveTenant
    {
        public virtual int? TenantId { get; set; }
        public virtual Tenant Tenant { get; set; }

        /// <summary>
        /// User id.
        /// </summary>
        public virtual long UserId { get; set; }

        /// <summary>
        /// Role id.
        /// </summary>
        public virtual int RoleId { get; set; }

        /// <summary>
        /// Creates a new <see cref="UserRole"/> object.
        /// </summary>
        public UserRole()
        {

        }

        /// <summary>
        /// Creates a new <see cref="UserRole"/> object.
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="roleId">Role id</param>
        public UserRole(int? tenantId, long userId, int roleId)
        {
            TenantId = tenantId;
            UserId = userId;
            RoleId = roleId;
        }
    }

    public class UserRoleEntityMapConfiguration : EntityMappingConfiguration<UserRole>
    {
        public override void Map(EntityTypeBuilder<UserRole> b)
        {
            b.HasIndex(e => new { e.TenantId, e.UserId });
            b.HasIndex(e => new { e.TenantId, e.RoleId });
        }

    }
}
