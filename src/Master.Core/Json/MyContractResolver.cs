using Abp.Json;
using Master.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Master.Json
{
    /// <summary>
    /// 自定义序列化解析，驼峰形式，且转换日期
    /// </summary>
    public class MyContractResolver : AbpCamelCasePropertyNamesContractResolver
    {

        protected override void ModifyProperty(MemberInfo member, JsonProperty property)
        {
            if (property.PropertyType == typeof(DateTime) || property.PropertyType == typeof(DateTime?))
            {
                property.Converter = new AbpDateTimeConverter()
                { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" };
            }
            //else if (property.PropertyType == typeof(bool) || property.PropertyType == typeof(bool?))
            //{
            //    property.Converter = new BoolConvert();
            //}
            
        }
        protected override string ResolvePropertyName(string propertyName)
        {
            ////需要驼峰序列化的属性名
            //var camelPropertyNames = new string[] { "Success","Result","TargetUrl","Error","UnAuthorizedRequest","Code","Message","Details","ValidationErrors","Members" };
            //if (camelPropertyNames.Contains(propertyName))
            //{
            //    return propertyName.Substring(0, 1).ToLower() + propertyName.Substring(1);
            //}
            return base.ResolvePropertyName(propertyName);
        }
    }
}
