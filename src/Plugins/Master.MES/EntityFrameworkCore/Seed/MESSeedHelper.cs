using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore.Uow;
using Abp.MultiTenancy;
using Master.MES;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;
using System.Linq;

namespace Master.EntityFrameworkCore.Seed
{
    public class MESSeedHelper
    {
        public static void SeedHostDb(IIocResolver iocResolver)
        {
            WithDbContext<MESDbContext>(iocResolver, SeedHostDb);
        }

        public static void SeedHostDb(MESDbContext context)
        {
            //context.Database.Migrate();
            context.SuppressAutoSetTenantId = true;
            //构建默认提醒策略
            BuildDefaultHostTactic(context);

            context.Database.ExecuteSqlCommand("update part set PartSource=4,enableprocess=1 where partsource=0");

        }

        private static void BuildDefaultHostTactic(MESDbContext context)
        {
            if(!context.Tactic.Any(o=>o.TacticName=="注册提醒" && o.TenantId == null))
            {
                //注册提醒
                var tactic = new Tactic()
                {
                    TenantId = null,
                    TacticName = "注册提醒",
                    TacticType = TacticType.Host,
                    IsActive = true
                };
                context.Tactic.Add(tactic);
                context.SaveChanges();
            }
            
        }
        private static void WithDbContext<TDbContext>(IIocResolver iocResolver, Action<TDbContext> contextAction)
            where TDbContext : DbContext
        {
            using (var uowManager = iocResolver.ResolveAsDisposable<IUnitOfWorkManager>())
            {
                using (var uow = uowManager.Object.Begin(TransactionScopeOption.Suppress))
                {
                    var context = uowManager.Object.Current.GetDbContext<TDbContext>(MultiTenancySides.Host);

                    contextAction(context);

                    uow.Complete();
                }
            }
        }
    }
}
