using System;
using System.Collections.Generic;
using System.Text;

namespace Master.MES.Dtos
{
    /// <summary>
    /// 任务报工信息Dto
    /// </summary>
    public class TaskReportInfoDto
    {
        public int Id { get; set; }
        public DateTime? ReceiveDate { get; set; }
        public bool ReceiveDateFromReport { get; set; }
        public DateTime? StartDate { get; set; }
        public bool StartDateFromReport { get; set; }
        public DateTime? EndDate { get; set; }
        public bool EndDateFromReport { get; set; }
    }
}
