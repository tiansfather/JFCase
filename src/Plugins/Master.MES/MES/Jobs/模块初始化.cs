using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.Domain.Uow;
using Master.Module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Master.MES.Jobs
{
    public class InitModuleJobs : BackgroundJob<InitModuleJobsArgs>, ITransientDependency
    {
        public IModuleInfoManager ModuleInfoManager { get; set; }
        public MESTenantManager MESTenantManager { get; set; }
        [UnitOfWork]
        public override void Execute(InitModuleJobsArgs args)
        {
            //如果账套还没有模块,则初始化模块
            if (! ModuleInfoManager.GetAll().Any(o => o.TenantId == args.TenantId))
            {
                MESTenantManager.InitModule(new int[] { args.TenantId }).GetAwaiter().GetResult();
            }
            MESTenantManager.InitAdminRole(new int[] { args.TenantId }).GetAwaiter().GetResult();
        }
    }

    public class InitModuleJobsArgs
    {
        public int TenantId { get; set; }
    }
}
