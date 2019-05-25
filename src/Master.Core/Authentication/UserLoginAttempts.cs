using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Abp.Timing;
using Master.Entity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Authentication
{
    
    public class UserLoginAttempt : Entity<int>, IHasCreationTime, IMayHaveTenant
    {       
        /// <summary>
        /// Tenant's Id, if <see cref="TenancyName"/> was a valid tenant name.
        /// </summary>
        public virtual int? TenantId { get; set; }

        /// <summary>
        /// Tenancy name.
        /// </summary>
        public virtual string TenancyName { get; set; }

        /// <summary>
        /// User's Id, if <see cref="UserNameOrEmailAddress"/> was a valid username or email address.
        /// </summary>
        public virtual long? UserId { get; set; }

        /// <summary>
        /// User name or email address
        /// </summary>
        public virtual string UserNameOrPhoneNumber { get; set; }

        /// <summary>
        /// IP address of the client.
        /// </summary>
        public virtual string ClientIpAddress { get; set; }

        /// <summary>
        /// Name (generally computer name) of the client.
        /// </summary>
        public virtual string ClientName { get; set; }

        /// <summary>
        /// Browser information if this method is called in a web request.
        /// </summary>
        public virtual string BrowserInfo { get; set; }

        /// <summary>
        /// Login attempt result.
        /// </summary>
        public virtual LoginResultType Result { get; set; }

        public virtual DateTime CreationTime { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserLoginAttempt"/> class.
        /// </summary>
        public UserLoginAttempt()
        {
            CreationTime = Clock.Now;
        }
    }

    public class UserLoginAttemptEntityMapConfiguration : EntityMappingConfiguration<UserLoginAttempt>
    {
        public override void Map(EntityTypeBuilder<UserLoginAttempt> b)
        {
            b.HasIndex(e => new { e.TenancyName, e.UserNameOrPhoneNumber, e.Result });
            b.HasIndex(ula => new { ula.UserId, ula.TenantId });
        }

    }
}
