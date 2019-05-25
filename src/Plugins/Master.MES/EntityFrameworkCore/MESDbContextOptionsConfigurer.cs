using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace Master.EntityFrameworkCore
{
    public static class MESDbContextOptionsConfigurer
    {
        public static readonly LoggerFactory MyLoggerFactory
    = new LoggerFactory(new[] { new DebugLoggerProvider((categoryName, logLevel) => (logLevel == LogLevel.Information) && (categoryName == DbLoggerCategory.Database.Command.Name)) });
        public static void Configure(
            DbContextOptionsBuilder<MESDbContext> dbContextOptions,
            string connectionString
            )
        {
            dbContextOptions.UseLazyLoadingProxies().UseMySql(connectionString).ConfigureWarnings(warnnngs => { warnnngs.Log(CoreEventId.LazyLoadOnDisposedContextWarning); warnnngs.Log(CoreEventId.DetachedLazyLoadingWarning); })
                .UseLoggerFactory(MyLoggerFactory)
                ;
            //builder.UseSqlServer(connectionString,b=>b.UseRowNumberForPaging());
        }
        public static void Configure(DbContextOptionsBuilder<MESDbContext> dbContextOptions, DbConnection connection)
        {
            dbContextOptions.UseLazyLoadingProxies().UseMySql(connection).ConfigureWarnings(warnnngs => { warnnngs.Log(CoreEventId.LazyLoadOnDisposedContextWarning); warnnngs.Log(CoreEventId.DetachedLazyLoadingWarning); })
                .UseLoggerFactory(MyLoggerFactory)
                ;
            //builder.UseSqlServer(connection, b => b.UseRowNumberForPaging());
        }
    }
}
