using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.MES.Dtos
{
    /// <summary>
    /// 直接工序选择方式提交的任务
    /// </summary>
    [AutoMap(typeof(ProcessTask))]
    public class SimpleSubmitTaskDto
    {
        public int PartId { get; set; }
        public int ProcessTypeId { get; set; }
        /// <summary>
        /// 要求完成时间
        /// </summary>
        public DateTime? RequireDate { get; set; }
        /// <summary>
        /// 预约时间
        /// </summary>
        public DateTime? AppointDate { get; set; }
        /// <summary>
        /// 预计工时
        /// </summary>
        public decimal? EstimateHours { get; set; }
        /// <summary>
        /// 任务说明
        /// </summary>
        public string TaskInfo { get; set; }
    }
}
