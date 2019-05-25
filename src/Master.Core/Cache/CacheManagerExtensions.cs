using Abp.Runtime.Caching;
using Master.Application.Features;
using Master.Authentication;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Cache
{
    public static class CacheManagerExtensions
    {
        public static ITypedCache<string, UserPermissionCacheItem> GetUserPermissionCache(this ICacheManager cacheManager)
        {
            return cacheManager.GetCache<string, UserPermissionCacheItem>(UserPermissionCacheItem.CacheStoreName);
        }

        public static ITypedCache<string, RolePermissionCacheItem> GetRolePermissionCache(this ICacheManager cacheManager)
        {
            return cacheManager.GetCache<string, RolePermissionCacheItem>(RolePermissionCacheItem.CacheStoreName);
        }

        public static ITypedCache<int, TenantFeatureCacheItem> GetTenantFeatureCache(this ICacheManager cacheManager)
        {
            return cacheManager.GetCache<int, TenantFeatureCacheItem>(TenantFeatureCacheItem.CacheStoreName);
        }

        public static ITypedCache<int, EditionFeatureCacheItem> GetEditionFeatureCache(this ICacheManager cacheManager)
        {
            return cacheManager.GetCache<int, EditionFeatureCacheItem>(EditionFeatureCacheItem.CacheStoreName);
        }
    }
}
