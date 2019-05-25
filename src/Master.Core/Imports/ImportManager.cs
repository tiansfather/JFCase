using Abp.AutoMapper;
using Abp.Reflection;
using Abp.Reflection.Extensions;
using Abp.UI;
using Master.Configuration.Dictionaries;
using Master.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Master.Imports
{
    /// <summary>
    /// 导入管理
    /// </summary>
    public class ImportManager:DomainServiceBase
    {
        //public DictionaryManager DictionaryManager { get; set; }
        public ITypeFinder TypeFinder { get; set; }
        /// <summary>
        /// 获取导入类型
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public Type GetImportType(string typeName)
        {
            var types = TypeFinder.Find(o => o.FullName == typeName);
            if (types.Length == 0)
            {
                throw new UserFriendlyException(L("未找到对应导入类型" + typeName));
            }
            return types[0];
        }
        /// <summary>
        /// 从导入dto类中获取导入项
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<ImportFieldInfo> GetImportFieldInfosFromType(string typeName)
        {
            var type = GetImportType(typeName);

            var allDics = Resolve<DictionaryManager>().GetAllDictionaries().Result;

            var fields = new List<ImportFieldInfo>();
            foreach(var property in type.GetProperties())
            {
                var attr = property.GetSingleAttributeOrNull<DisplayNameAttribute>();
                if (attr != null)
                {
                    //导入字段定义
                    var fieldInfo = new ImportFieldInfo()
                    {
                        DisplayName = attr.DisplayName,
                        FieldName = property.Name,
                    };
                    //
                    var fieldAttr= property.GetSingleAttributeOrNull<ImportFieldAttribute>();
                    if (fieldAttr != null)
                    {
                        fieldAttr.MapTo(fieldInfo);
                        if (!string.IsNullOrEmpty(fieldAttr.DictionaryName))
                        {
                            if (allDics.ContainsKey(fieldAttr.DictionaryName))
                            {
                                fieldInfo.AvailableValues = allDics[fieldAttr.DictionaryName];
                            }
                            else
                            {
                                fieldInfo.AvailableValues = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(fieldAttr.DictionaryName);
                            }
                        }
                    }
                    fields.Add(fieldInfo);
                }
            }

            return fields;
        }

        /// <summary>
        /// 获取导入数据的表头数组
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public string[] GetHeadersFromContent(string content)
        {
            //去除表头数据中的换行
            var regex = new System.Text.RegularExpressions.Regex("\"(.*?)\n(.*?)\"");
            var _content = regex.Replace(content, "$1$2");

            var rows = _content.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            if (rows.Count() == 0)
            {
                throw new UserFriendlyException(L("没有找到数据"));
            }

            return rows[0].Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries); ;
        }
    }
}
