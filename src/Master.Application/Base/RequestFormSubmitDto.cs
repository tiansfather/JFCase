using Abp.UI;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Dto
{
    /// <summary>
    /// 通用表单提交参数
    /// </summary>
    public class FormSubmitRequestDto:RequestDto
    {
        /// <summary>
        /// 对应的提交行为标记
        /// </summary>
        public string Action { get; set; }
        public IDictionary<string,string> Datas { get; set; }

    }
}
