using Abp.Dependency;
using Abp.Modules;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Master.Web.Startup
{
    public class SwaggerDocHelper
    {
        public static void ConfigXmlCommentsPath(SwaggerGenOptions config)
        {
            var docs = GetXmlCommentsPath();
            foreach (var doc in docs)
            {
                if (System.IO.File.Exists(doc))
                {
                    config.IncludeXmlComments(doc);
                }
            }
            //config.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

        }
        private static List<string> GetXmlCommentsPath()
        {
            List<string> docs = new List<string>();
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var pluginDirectory = Path.Combine(baseDirectory, "Plugin");
            using (var moduleManagerWrapper = IocManager.Instance.ResolveAsDisposable<IAbpModuleManager>())
            {
                var modules = moduleManagerWrapper.Object.Modules;
                foreach (var module in modules)
                {
                    var commentsFileName = module.Assembly.GetName().Name.Replace(".dll", "") + ".XML";
                    var commentsFile = Path.Combine(baseDirectory, commentsFileName);
                    var commentsPluginFile= Path.Combine(pluginDirectory, commentsFileName);
                    docs.Add(commentsFile);
                    docs.Add(commentsPluginFile);
                }
            }
            //var modules = AbpModule.FindDependedModuleTypesRecursivelyIncludingGivenModule(typeof(MasterWebModule));
            

            

            return docs;
            //return String.Format(@"{0}\bin\SwaggerUi.XML", System.AppDomain.CurrentDomain.BaseDirectory);
        }
    }
}
