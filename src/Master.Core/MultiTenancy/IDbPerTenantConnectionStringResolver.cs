using Abp.Domain.Uow;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.MultiTenancy
{
    public interface IDbPerTenantConnectionStringResolver : IConnectionStringResolver
    {
        /// <summary>
        /// Gets the connection string for given args.
        /// </summary>
        string GetNameOrConnectionString(DbPerTenantConnectionStringResolveArgs args);
    }
}
