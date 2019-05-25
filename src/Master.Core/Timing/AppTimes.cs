using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Timing
{
    public class AppTimes : ISingletonDependency
    {
        /// <summary>
        /// 获取应用启动时间
        /// </summary>
        public DateTime StartupTime { get; set; }
    }
}
