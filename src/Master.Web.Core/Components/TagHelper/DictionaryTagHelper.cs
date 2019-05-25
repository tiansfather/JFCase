using Abp.Dependency;
using Abp.Localization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Master.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Master.Configuration.Dictionaries;

namespace Master.Web.Components
{
    /// <summary>
    /// 用于解析select和radio标签
    /// </summary>
    [HtmlTargetElement(Attributes = "dictionary-name")]
    public class DictionaryTagHelper : TagHelper
    {

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }
        public string DictionaryName { get; set; }

        public string Value { get; set; }

        public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {

            using(var localizationManagerWrapper= IocManager.Instance.ResolveAsDisposable<ILocalizationManager>())
            {
                var localizationManager = localizationManagerWrapper.Object;
                var localizationSource = localizationManager.GetSource(MasterConsts.LocalizationSourceName);

                var dic = GetDictionary(DictionaryName);
                return Task.Run(() =>
                {
                    if (context.TagName == "select")
                    {
                        foreach (KeyValuePair<string, string> item  in dic)
                        {
                            output.Content.AppendHtml($"<option value='{item.Key}' {(Value == item.Key ? "selected" : "")}>{localizationSource.GetString(item.Value)}</option>");
                        }
                    }
                    else if (context.TagName == "radio")
                    {
                        output.TagName = "div";
                        TagHelperAttribute nameAttribute = null;
                        context.AllAttributes.TryGetAttribute("name", out nameAttribute);
                        foreach (KeyValuePair<string, string> item in dic)
                        {
                            output.PostContent.AppendHtml($"<input type='radio' name='{nameAttribute?.Value}' value='{ item.Key}' {(Value == item.Key ? "checked" : "")} title='{localizationSource.GetString(item.Value)}' >");
                        }
                    }
                    else if (context.TagName == "checkbox")
                    {
                        output.TagName = "div";
                        TagHelperAttribute nameAttribute = null;
                        TagHelperAttribute skinAttribute = null;
                        context.AllAttributes.TryGetAttribute("name", out nameAttribute);
                        context.AllAttributes.TryGetAttribute("skin", out skinAttribute);

                        var value_arr = Value.Split(',').ToList();

                        foreach (KeyValuePair<string, string> item in dic)
                        {
                            output.Content.AppendHtml($"<input type='checkbox' name='{nameAttribute?.Value}' value='{ item.Key}'{(value_arr.Contains(item.Key) ? "checked" : "")} title='{localizationSource.GetString(item.Value)}' lay-skin='{skinAttribute?.Value}'>");
                        }
                    }


                });
            }
            
            

            
        }

        private Dictionary<string,string> GetDictionary(string dictionaryname)
        {
            
            using (var dictionaryManagerWrapper = IocManager.Instance.ResolveAsDisposable<IDictionaryManager>())
            {
                var dictionaryManager = dictionaryManagerWrapper.Object;
                var allDics = dictionaryManager.GetAllDictionaries().Result;
                if (allDics.ContainsKey(dictionaryname))
                {
                    return allDics[dictionaryname];
                }

                //尝试直接反序列化
                try
                {
                    var dic = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(dictionaryname);
                    return dic;
                }
                catch
                {
                    return new Dictionary<string, string>();
                }
                

            }
        }
    }
}
