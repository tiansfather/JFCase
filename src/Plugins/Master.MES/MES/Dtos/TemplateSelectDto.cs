using System;
using System.Collections.Generic;
using System.Text;

namespace Master.MES.Dtos
{
    /// <summary>
    /// 选择模板Dto
    /// </summary>
    public class TemplateSelectDto
    {
        /// <summary>
        /// 如果id为空，表示使用默认模板
        /// </summary>
        public int? Id { get; set; }
        public string TemplateName { get; set; }
    }
}
