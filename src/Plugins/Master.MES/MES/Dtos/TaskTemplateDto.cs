using System;
using System.Collections.Generic;
using System.Text;

namespace Master.MES.Dtos
{
    /// <summary>
    /// 账套加工单模板
    /// </summary>
    public class TaskTemplateDto
    {
        public int TenantId { get; set; }
        public int Id { get; set; }
        public string TemplateContent { get; set; }
    }
}
