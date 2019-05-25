using System;
using System.Collections.Generic;
using System.Text;

namespace Master.MES.Dtos
{
    /// <summary>
    /// 接收从云加工平台传递的汇报信息
    /// </summary>
    public class ReceiveMESTaskDto
    {
        public int TaskId { get; set; }
        public ReceiveMESTaskReportType ReceiveMESTaskReportType { get; set; }
        public DateTime ReportTime { get; set; }
        public string Message { get; set; }
    }

    public enum ReceiveMESTaskReportType
    {
        接收=0,
        上机=1,
        加工=2,
        下机=3
    }
}
