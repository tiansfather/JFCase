using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Authentication
{
    [Serializable]
    public class UserPermissionCacheItem
    {
        public const string CacheStoreName = "UserPermissions";

        public long UserId { get; set; }

        public List<int> RoleIds { get; set; }

        public HashSet<string> GrantedPermissions { get; set; }

        public HashSet<string> ProhibitedPermissions { get; set; }

        public UserPermissionCacheItem()
        {
            RoleIds = new List<int>();
            GrantedPermissions = new HashSet<string>();
            ProhibitedPermissions = new HashSet<string>();
        }

        public UserPermissionCacheItem(long userId)
            : this()
        {
            UserId = userId;
        }
    }
}
