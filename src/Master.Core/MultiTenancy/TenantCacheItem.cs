using System;
using System.Collections.Generic;
using System.Text;

namespace Master.MultiTenancy
{
    [Serializable]
    public class TenantCacheItem
    {
        public const string CacheName = "TenantCache";

        public const string ByNameCacheName = "TenantByNameCache";

        public int Id { get; set; }

        public string Name { get; set; }

        public string TenancyName { get; set; }

        public string ConnectionString { get; set; }

        public int? EditionId { get; set; }

        public bool IsActive { get; set; }

        public object CustomData { get; set; }
    }
}
