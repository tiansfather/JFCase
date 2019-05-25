using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.MES.Dtos
{
    /// <summary>
    /// 报工提交Dto
    /// </summary>
    [AutoMap(typeof(ProcessTaskReport))]
    public class ProcessTaskReportDto
    {
        public int ProcessTaskId { get; set; }
        public ReportType ReportType { get; set; }
        public DateTime ReportTime { get; set; }
        public string Remarks { get; set; }
        public List<UploadFileInfo> Files { get; set; }
        /// <summary>
        /// 微信素材id
        /// </summary>
        public List<string> MediaIds { get; set; }
        public decimal Progress { get; set; }
    }
}
