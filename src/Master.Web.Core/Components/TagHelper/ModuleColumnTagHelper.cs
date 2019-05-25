using Abp.Domain.Entities;
using Abp.Extensions;
using Master.Module;
using Microsoft.AspNetCore.Mvc.Rendering;

using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Master.Web.Components
{
    /// <summary>
    /// 表头组件
    /// </summary>
    [HtmlTargetElement("th",Attributes ="column-info")]
    public class ModuleColumnTagHelper:TagHelper
    {
        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }
        public ColumnInfo ColumnInfo { get; set; }

        public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {

            return Task.Run(() => {
                var laydata = ColumnInfo.ToLayData();
                //var laydata = BuildLayData();
                output.Attributes.Add("lay-data", Newtonsoft.Json.JsonConvert.SerializeObject(laydata));
                output.Content.SetContent(ColumnInfo.ColumnName);
            });
        }

        //private Dictionary<string,object> BuildLayData()
        //{
        //    var dic = new Dictionary<string, object>();
        //    //模板
        //    if (!ColumnInfo.Templet.IsNullOrWhiteSpace() || ColumnInfo.ColumnType == ColumnTypes.Images || ColumnInfo.ColumnType == ColumnTypes.Files)
        //    {
        //        dic.Add("templet", $"#templet_{ColumnInfo.ModuleInfo.ModuleKey}_{ColumnInfo.ColumnKey}");
        //    }
            
        //    //启用排序
        //    if (ColumnInfo.IsEnableSort)
        //    {
        //        dic.Add("sort", true);
        //    }
        //    //如果是操作列
        //    if (ColumnInfo.IsOperationColumn)
        //    {
        //        dic.Add("toolbar", $"#{ColumnInfo.ColumnKey}");
        //        dic.Add("minWidth", "100");
        //    }
        //    else
        //    {
        //        //非操作列需要绑定数据
        //        dic.Add("field", ColumnInfo.ColumnKey);
        //    }
        //    var align = ColumnInfo.GetData<string>("align");
        //    if (!align.IsNullOrWhiteSpace())
        //    {
        //        dic.Add("align", align);
        //    }
        //    var style = ColumnInfo.GetData<string>("style");
        //    if (!style.IsNullOrWhiteSpace())
        //    {
        //        dic.Add("style", style);
        //    }
        //    var width= ColumnInfo.GetData<string>("width");
        //    if (!width.IsNullOrWhiteSpace())
        //    {
        //        dic.Add("width", width);
        //    }
        //    var fix = ColumnInfo.GetData<string>("fixed");
        //    if(fix=="left" || fix == "right")
        //    {
        //        dic["fixed"] = fix;
        //    }
            
        //    return dic;
        //}
    }
}
