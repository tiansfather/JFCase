using Abp.Configuration.Startup;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Configuration
{
    public class MasterConfiguration
    {
        public Dictionary<string, Dictionary<string, string>> Dictionaries { get; private set; } = new Dictionary<string, Dictionary<string, string>>();
        /// <summary>
        /// 关联数据提供者，用于提供模块中的数据
        /// </summary>
        public Dictionary<Type, List<Type>> RelativeDataProviders { get; private set; } = new Dictionary<Type, List<Type>>();
        /// <summary>
        /// 实体标记数据提供者，用于定义实体标记
        /// </summary>
        public Dictionary<Type, List<StatusDefinition>> EntityStatusDefinitions { get; private set; } = new Dictionary<Type, List<StatusDefinition>>();
    }

    public static class MasterConfigurationExtension
    {
        public static MasterConfiguration Core(this IModuleConfigurations moduleConfigurations)
        {
            return moduleConfigurations.AbpConfiguration.Get<MasterConfiguration>();
        }
    }
}
