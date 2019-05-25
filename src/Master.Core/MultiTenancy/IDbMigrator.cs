using System;
using System.Collections.Generic;
using System.Text;

namespace Master.MultiTenancy
{
    public interface IDbMigrator
    {
        void CreateOrMigrateForHost();

        void CreateOrMigrateForTenant(Tenant tenant);
    }
}
