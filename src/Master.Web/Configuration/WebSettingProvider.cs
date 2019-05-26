using Abp.Configuration;
using Abp.Localization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Configuration
{
    public class WebSettingNames
    {
        
    }
    public class WebSettingProvider : Abp.Configuration.SettingProvider
    {
        public override IEnumerable<SettingDefinition> GetSettingDefinitions(SettingDefinitionProviderContext context)
        {
            
            return new SettingDefinition[]
            {
                
            };
        }

        private static LocalizableString L(string name)
        {
            return new LocalizableString(name, MasterConsts.LocalizationSourceName);
        }
    }
}
