using System.Reflection;
using Abp.Configuration.Startup;
using Abp.Localization;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Json;
using Abp.Localization.Sources;
using Abp.Reflection.Extensions;

namespace Master.Localization
{
    public static class MESLocalizationConfigurer
    {
        public static void Configure(ILocalizationConfiguration localizationConfiguration)
        {
            localizationConfiguration.Sources.Extensions.Add(
                new LocalizationSourceExtensionInfo(MasterConsts.LocalizationSourceName,
                    new JsonEmbeddedFileLocalizationDictionaryProvider(
                        Assembly.GetExecutingAssembly(),
                        "Master.Localization.SourceFiles"
                    )
                )
            );
        }
    }
}