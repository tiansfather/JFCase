using System;
using System.Collections.Generic;
using System.Text;

namespace Master.MES.Dtos
{
    /// <summary>
    /// 加工点报表数据
    /// </summary>
    public class MESUnitReportRankDto
    {
        public int UnitId { get; set; }
        /// <summary>
        /// 加工点名称
        /// </summary>
        public string UnitName { get; set; }
        /// <summary>
        /// 总任务数
        /// </summary>
        public int TaskCount { get; set; }
        /// <summary>
        /// 报工总数
        /// </summary>
        public int ReportCount { get; set; }
        /// <summary>
        /// 有报工任务数
        /// </summary>
        public int ReportTaskCount { get; set; }
        /// <summary>
        /// 已完成的报工任务数
        /// </summary>
        public int CompletedCount { get; set; }
        /// <summary>
        /// 延期上机任务数
        /// </summary>
        public int DelayStartCount { get; set; }
        /// <summary>
        /// 延期下机任务数
        /// </summary>
        public int DelayEndCount { get; set; }
        /// <summary>
        /// 正常上下机任务数
        /// </summary>
        public int NoDelayCount { get; set; }
        /// <summary>
        /// 超时任务数
        /// </summary>
        public int OverHourCount { get; set; }
        /// <summary>
        /// 不合格任务数
        /// </summary>
        public int NGCount { get; set; }
    }
}
