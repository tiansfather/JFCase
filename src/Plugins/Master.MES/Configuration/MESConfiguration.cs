using Abp.Configuration.Startup;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Configuration
{
    public class MESConfiguration
    {
        
    }

    public static class MESConfigurationExtension
    {
        public static MESConfiguration MES(this IModuleConfigurations moduleConfigurations)
        {
            return moduleConfigurations.AbpConfiguration.Get<MESConfiguration>();
        }
    }
}
