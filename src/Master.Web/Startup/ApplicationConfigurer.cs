using Abp.Dependency;
using Abp.Reflection;
using Master.Web.Configuration;
using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Master.Web.Startup
{
    public static class ApplicationConfigurer
    {
        public static void Configure(IApplicationBuilder app)
        {
            using(var typeFinderWrapper = IocManager.Instance.ResolveAsDisposable<ITypeFinder>())
            {
                var types=typeFinderWrapper.Object.Find(o => typeof(IApplicationBuilderConfigurer).IsAssignableFrom(o) && o.IsClass);
                foreach(var type in types)
                {
                    (Activator.CreateInstance(type) as IApplicationBuilderConfigurer).Configure(app);
                }
            }
        }
    }
}
