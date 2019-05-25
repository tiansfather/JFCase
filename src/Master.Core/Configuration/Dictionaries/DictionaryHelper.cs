using Abp.Dependency;
using Abp.Extensions;
using Abp.Localization;
using Master.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Master.Configuration.Dictionaries
{
    /// <summary>
    /// 字典项帮助类，用于从系统字典中获取值
    /// </summary>
    public static class DictionaryHelper
    {
        public static string GetDictionaryValue(string dictionaryName, string key)
        {
            if (dictionaryName.IsNullOrEmpty() || key.IsNullOrEmpty())
            {
                return string.Empty;
            }

            using (var localizationManagerWrapper = IocManager.Instance.ResolveAsDisposable<ILocalizationManager>())
            {
                var localizationManager = localizationManagerWrapper.Object;
                var localizationSource = localizationManager.GetSource(MasterConsts.LocalizationSourceName);

                using(var dictionaryManagerWrapper = IocManager.Instance.ResolveAsDisposable<IDictionaryManager>())
                {
                    var dictionaryManager = dictionaryManagerWrapper.Object;
                    var allDics = dictionaryManager.GetAllDictionaries().Result;
                    var dic = allDics.ContainsKey(dictionaryName) ? allDics[dictionaryName] : new Dictionary<string, string>();

                    return localizationSource.GetString(dic.GetDataOrEmpty(key));
                }

                //using (var configurationWrapper = IocManager.Instance.ResolveAsDisposable<MasterConfiguration>())
                //{
                //    var coreConfiguration = configurationWrapper.Object;
                //    var dic = coreConfiguration.Dictionaries[dictionaryName] ?? new Dictionary<string, string>();

                //    return localizationSource.GetString(dic.GetDataOrEmpty(key));
                //}
            }
        }

        public static IEnumerable<string> GetDictionaryValues(string dictionaryName, IEnumerable<string> keys)
        {
            if(keys==null || keys.Count() == 0)
            {
                return new List<string>();
            }
            using (var localizationManagerWrapper = IocManager.Instance.ResolveAsDisposable<ILocalizationManager>())
            {
                var localizationManager = localizationManagerWrapper.Object;
                var localizationSource = localizationManager.GetSource(MasterConsts.LocalizationSourceName);


                using (var dictionaryManagerWrapper = IocManager.Instance.ResolveAsDisposable<IDictionaryManager>())
                {
                    var dictionaryManager = dictionaryManagerWrapper.Object;
                    var allDics = dictionaryManager.GetAllDictionaries().Result;
                    var dic = allDics.ContainsKey(dictionaryName) ? allDics[dictionaryName] : new Dictionary<string, string>();

                    var result = new List<string>();
                    foreach (var key in keys)
                    {
                        result.Add(localizationSource.GetString(dic.GetDataOrEmpty(key)));
                    }

                    return result;
                }
                //using (var configurationWrapper = IocManager.Instance.ResolveAsDisposable<MasterConfiguration>())
                //{
                //    var coreConfiguration = configurationWrapper.Object;
                //    var dic = coreConfiguration.Dictionaries[dictionaryName] ?? new Dictionary<string, string>();

                //    var result = new List<string>();
                //    foreach (var key in keys)
                //    {
                //        result.Add(localizationSource.GetString(dic.GetDataOrEmpty(key)));
                //    }

                //    return result;
                //}
            }
        }
    }
}
