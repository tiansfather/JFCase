using Abp.Domain.Uow;
using Abp.MultiTenancy;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.MultiTenancy
{
    public class DbPerTenantConnectionStringResolveArgs : ConnectionStringResolveArgs
    {
        public int? TenantId { get; set; }

        public DbPerTenantConnectionStringResolveArgs(int? tenantId, MultiTenancySides? multiTenancySide = null)
            : base(multiTenancySide)
        {
            TenantId = tenantId;
        }

        public DbPerTenantConnectionStringResolveArgs(int? tenantId, ConnectionStringResolveArgs baseArgs)
        {
            TenantId = tenantId;
            MultiTenancySide = baseArgs.MultiTenancySide;

            foreach (var kvPair in baseArgs)
            {
                Add(kvPair.Key, kvPair.Value);
            }
        }
    }
}
