using Abp.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;
using System.Data.Common;

namespace Master.EntityFrameworkCore
{
    public static class DbContextOptionsConfigurer
    {
        
        public static readonly LoggerFactory MyLoggerFactory
    = new LoggerFactory(new[] { new DebugLoggerProvider((categoryName, logLevel) => (logLevel == LogLevel.Information) && (categoryName == DbLoggerCategory.Database.Command.Name)) });
        public static void Configure<TDbContext>(
            DbContextOptionsBuilder<TDbContext> dbContextOptions, 
            string connectionString
            )
            where TDbContext : AbpDbContext
        {
            dbContextOptions.UseLazyLoadingProxies().UseMySql(connectionString).ConfigureWarnings(warnnngs => { warnnngs.Log(CoreEventId.LazyLoadOnDisposedContextWarning); warnnngs.Log(CoreEventId.DetachedLazyLoadingWarning); })
                //.UseLoggerFactory(MyLoggerFactory)
                ;
            //builder.UseSqlServer(connectionString,b=>b.UseRowNumberForPaging());
        }
        public static void Configure<TDbContext>(DbContextOptionsBuilder<TDbContext> dbContextOptions, DbConnection connection)
            where TDbContext : AbpDbContext
        {
            dbContextOptions.UseLazyLoadingProxies().UseMySql(connection).ConfigureWarnings(warnnngs => { warnnngs.Log(CoreEventId.LazyLoadOnDisposedContextWarning); warnnngs.Log(CoreEventId.DetachedLazyLoadingWarning); })
                //.UseLoggerFactory(MyLoggerFactory)
                ;
            //builder.UseSqlServer(connection, b => b.UseRowNumberForPaging());
        }
    }
}
