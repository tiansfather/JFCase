using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Master.Module
{
    /// <summary>
    /// 字段类型
    /// </summary>
    public enum ColumnTypes
    {
        [Display(Name = "文本")]
        Text = 1,
        [Display(Name = "多行文本")]
        TextArea,
        [Display(Name = "日期时间")]
        DateTime,
        [Display(Name = "数字")]
        Number,
        [Display(Name = "选择")]
        Select,
        [Display(Name = "开关")]
        Switch,
        [Display(Name = "多选")]
        MultiSelect,
        [Display(Name = "网页")]
        Html,
        [Display(Name = "附件")]
        Files,
        [Display(Name = "图片")]
        Images,
        [Display(Name = "引用")]
        Reference,
        [Display(Name = "嵌入")]
        Embed,
        [Display(Name = "编码")]
        Code,
        [Display(Name = "系统")]
        System,
        [Display(Name = "自定义")]
        Customize,
        [Display(Name = "分隔符")]
        Separator
    }
}
