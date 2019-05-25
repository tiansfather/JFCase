using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.Threading.BackgroundWorkers;
using Abp.Threading.Timers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Master.Web.Startup.Jobs
{
    public class DeleteOldFileJobs : PeriodicBackgroundWorkerBase, ISingletonDependency
    {
        public DeleteOldFileJobs(AbpTimer timer,
           IBackgroundJobManager backgroundJobManager)
            : base(timer)
        {
            Timer.Period = 60 * 1000 * 60 * 2; //2小时执行一次
        }
        protected override void DoWork()
        {
            //删除temp文件夹下超过1天的文件
            var files = System.IO.Directory.GetFiles(Common.PathHelper.VirtualPathToAbsolutePath("/Temp"));
            foreach(var file in files)
            {
                var fileInfo = new System.IO.FileInfo(file);
                if ((DateTime.Now - fileInfo.CreationTime).TotalDays > 1)
                {
                    System.IO.File.Delete(file);
                }
            }
        }
    }
}
