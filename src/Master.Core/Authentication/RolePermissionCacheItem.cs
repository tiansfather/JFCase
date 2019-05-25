using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Authentication
{
    [Serializable]
    public class RolePermissionCacheItem
    {
        public const string CacheStoreName = "RolePermissions";

        public long RoleId { get; set; }

        public HashSet<string> GrantedPermissions { get; set; }

        public RolePermissionCacheItem()
        {
            GrantedPermissions = new HashSet<string>();
        }

        public RolePermissionCacheItem(int roleId)
            : this()
        {
            RoleId = roleId;
        }
    }
}
