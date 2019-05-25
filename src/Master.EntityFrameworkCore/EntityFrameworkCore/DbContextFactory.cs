using Abp.EntityFrameworkCore;
using Master.Configuration;
using Master.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.EntityFrameworkCore
{
    public abstract class DbContextFactory<TDbContext> : IDesignTimeDbContextFactory<TDbContext>
        where TDbContext : AbpDbContext
    {
        public abstract TDbContext CreateDbContext(DbContextOptions<TDbContext> options);

        public virtual string GetConnectionStringName()
        {
            return MasterConsts.ConnectionStringName;
        }

        public TDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<TDbContext>();
            var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder());

            DbContextOptionsConfigurer.Configure(
                builder,
                configuration.GetConnectionString(GetConnectionStringName())
            );

            return CreateDbContext(builder.Options);
            //return new MasterDbContext(builder.Options);
        }
    }
}
