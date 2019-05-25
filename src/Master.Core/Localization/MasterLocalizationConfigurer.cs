using System.Reflection;
using Abp.Configuration.Startup;
using Abp.Localization;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Json;
using Abp.Localization.Sources;
using Abp.Reflection.Extensions;

namespace Master.Localization
{
    public static class MasterLocalizationConfigurer
    {
        public static void Configure(ILocalizationConfiguration localizationConfiguration)
        {
            localizationConfiguration.Languages.Add(new LanguageInfo("zh-cn", "中文", "", isDefault: true));
            localizationConfiguration.Languages.Add(new LanguageInfo("en", "English", ""));

            localizationConfiguration.Sources.Add(
                new DictionaryBasedLocalizationSource(MasterConsts.LocalizationSourceName,
                    new JsonEmbeddedFileLocalizationDictionaryProvider(
                        typeof(MasterLocalizationConfigurer).GetAssembly(),
                        "Master.Localization.SourceFiles"
                    )
                )
            );
            localizationConfiguration.Sources.Extensions.Add(
                new LocalizationSourceExtensionInfo(MasterConsts.LocalizationSourceName,
                    new JsonFileLocalizationDictionaryProvider(
                        Common.PathHelper.VirtualPathToAbsolutePath("/Localization/Master")
                    )
                )
            );
            //不对未找到的文本进行"[]"包裹
            localizationConfiguration.WrapGivenTextIfNotFound = false;
        }
    }
}