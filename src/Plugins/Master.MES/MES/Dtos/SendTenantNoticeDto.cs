using System;
using System.Collections.Generic;
using System.Text;

namespace Master.MES.Dtos
{
    /// <summary>
    /// 发送往来单位公告
    /// </summary>
    public class SendTenantNoticeDto
    {
        public int[] UnitIds { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }
}
