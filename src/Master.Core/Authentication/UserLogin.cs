using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Master.Authentication
{
    public class UserLogin : Entity<int>, IMayHaveTenant
    {
        /// <summary>
        /// Maximum length of <see cref="LoginProvider"/> property.
        /// </summary>
        public const int MaxLoginProviderLength = 128;

        /// <summary>
        /// Maximum length of <see cref="ProviderKey"/> property.
        /// </summary>
        public const int MaxProviderKeyLength = 256;

        public virtual int? TenantId { get; set; }

        /// <summary>
        /// Id of the User.
        /// </summary>
        public virtual long UserId { get; set; }

        /// <summary>
        /// Login Provider.
        /// </summary>
        [Required]
        [MaxLength(MaxLoginProviderLength)]
        public virtual string LoginProvider { get; set; }

        /// <summary>
        /// Key in the <see cref="LoginProvider"/>.
        /// </summary>
        [Required]
        [MaxLength(MaxProviderKeyLength)]
        public virtual string ProviderKey { get; set; }

        public UserLogin()
        {

        }

        public UserLogin(int? tenantId, long userId, string loginProvider, string providerKey)
        {
            TenantId = tenantId;
            UserId = userId;
            LoginProvider = loginProvider;
            ProviderKey = providerKey;
        }
    }
}
