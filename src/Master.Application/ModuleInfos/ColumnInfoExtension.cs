using Abp.Domain.Entities;
using Abp.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Module
{
    public static class ColumnInfoExtension
    {
        /// <summary>
        /// 将列信息转化为laytable的列数据格式{field:'a',align:'left'}
        /// </summary>
        /// <param name="columnInfo"></param>
        /// <returns></returns>
        public static Dictionary<string,object> ToLayData(this ColumnInfo columnInfo)
        {
            var dic = new Dictionary<string, object>();
            dic.Add("title", columnInfo.ColumnName);
            dic.Add("sortNum", columnInfo.Sort);
            //模板
            if ( columnInfo.ColumnType == ColumnTypes.Images || columnInfo.ColumnType == ColumnTypes.Files)
            {
                dic.Add("templet", $"#templet_{columnInfo.ModuleInfo.ModuleKey}_{columnInfo.ColumnKey}");
            }
            //采用行间模板写法
            else if (!columnInfo.Templet.IsNullOrWhiteSpace())
            {
                dic.Add("templet", "<div>" + columnInfo.Templet + "</div>");
            }
            //启用排序
            if (columnInfo.IsEnableSort)
            {
                dic.Add("sort", true);
            }
            //启用汇总行
            if(columnInfo.ColumnType==ColumnTypes.Number && columnInfo.EnableTotalRow)
            {
                dic.Add("totalRow", true);
            }
            //搜索
            if (columnInfo.IsShownInAdvanceSearch && columnInfo.ColumnType!=ColumnTypes.Files && columnInfo.ColumnType!=ColumnTypes.Images)
            {
                //是否日期类型
                if (columnInfo.ColumnType == ColumnTypes.DateTime)
                {
                    dic.Add("filter", new { type= "date[yyyy-MM-dd HH:mm:ss]" } );
                }
                else
                {
                    dic.Add("filter", true);
                }
                
            }
            //如果是操作列
            if (columnInfo.IsOperationColumn)
            {
                dic.Add("toolbar", $"#{columnInfo.ColumnKey}");
                dic.Add("minWidth", "100");
            }
            else
            {
                //非操作列需要绑定数据
                dic.Add("field", columnInfo.ColumnKey.ToCamelCase());
            }
            var align = columnInfo.GetData<string>("align");
            if (!align.IsNullOrWhiteSpace())
            {
                dic.Add("align", align);
            }
            var style = columnInfo.GetData<string>("style");
            if (!style.IsNullOrWhiteSpace())
            {
                dic.Add("style", style);
            }
            var width = columnInfo.GetData<string>("width");
            if (!width.IsNullOrWhiteSpace())
            {
                dic.Add("width", width);
            }
            var fix = columnInfo.GetData<string>("fixed");
            if (fix == "left" || fix == "right")
            {
                dic["fixed"] = fix;
            }

            return dic;
        }
    }
}
