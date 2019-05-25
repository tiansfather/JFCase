using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore;
using Abp.Extensions;
using Abp.MultiTenancy;
using Master.MultiTenancy;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;

namespace Master.EntityFrameworkCore
{
    public class DbMigrator<TDbContext> : IDbMigrator, ITransientDependency 
        where TDbContext:DbContext
    {
        public IUnitOfWorkManager UnitOfWorkManager { get; set; }
        public IDbPerTenantConnectionStringResolver ConnectionStringResolver { get; set; }
        public IDbContextResolver DbContextResolver { get; set; }
        public virtual void CreateOrMigrateForHost()
        {
            CreateOrMigrateForHost(null);
        }

        public virtual void CreateOrMigrateForHost(Action<TDbContext> seedAction)
        {
            CreateOrMigrate(null, seedAction);
        }

        public virtual void CreateOrMigrateForTenant(Tenant tenant)
        {
            CreateOrMigrateForTenant(tenant, null);
        }

        public virtual void CreateOrMigrateForTenant(Tenant tenant, Action<TDbContext> seedAction)
        {
            if (tenant.ConnectionString.IsNullOrEmpty())
            {
                return;
            }

            CreateOrMigrate(tenant, seedAction);
        }

        protected virtual void CreateOrMigrate(Tenant tenant, Action<TDbContext> seedAction)
        {
            var args = new DbPerTenantConnectionStringResolveArgs(
                tenant == null ? (int?)null : (int?)tenant.Id,
                tenant == null ? MultiTenancySides.Host : MultiTenancySides.Tenant
            );

            args["DbContextType"] = typeof(TDbContext);
            args["DbContextConcreteType"] = typeof(TDbContext);

            var nameOrConnectionString = ConnectionStringHelper.GetConnectionString(
                ConnectionStringResolver.GetNameOrConnectionString(args)
            );

            using (var uow = UnitOfWorkManager.Begin(TransactionScopeOption.Suppress))
            {
                using (var dbContext = DbContextResolver.Resolve<TDbContext>(nameOrConnectionString, null))
                {
                    dbContext.Database.Migrate();
                    seedAction?.Invoke(dbContext);
                    UnitOfWorkManager.Current.SaveChanges();
                    uow.Complete();
                }
            }
        }
    }
}
