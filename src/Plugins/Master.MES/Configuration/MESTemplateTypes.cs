using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Configuration
{
    /// <summary>
    /// 模板类型
    /// </summary>
    public class MESTemplateSetting
    {
        /// <summary>
        /// 默认加工单模板名称，如果账套设置了此名称模板，会替换默认模板
        /// </summary>
        public const string TemplateName_DefaultProcessTask = "加工单";
        /// <summary>
        /// 加工单模板类型
        /// </summary>
        public const string TemplateType_ProcessTask = "ProcessTask";
        /// <summary>
        /// 过程卡模板类型
        /// </summary>
        public const string TemplateType_PartSheet = "PartSheet";
        /// <summary>
        /// 工艺设定模板类型
        /// </summary>
        public const string TemplateType_PartTaskSetting = "PartTaskSetting";
    }
}
