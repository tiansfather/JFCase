using System;
using System.Collections.Generic;
using System.Text;

namespace Master.MES.Dtos
{
    /// <summary>
    /// 零件工艺设定提交Dto
    /// </summary>
    public class SubmitPartTaskInfoDto
    {
        public int Id { get; set; }
        public List<SubmitPartTaskInfoDetailDto> Tasks { get; set; }
    }
    public class SubmitPartTaskInfoDetailDto
    {
        public int Id { get; set; }
        public int ProcessTypeId { get; set; }
        public DateTime? PlanStartDate { get; set; }
        public DateTime? PlanEndDate { get; set; }
        public decimal? EstimateHours { get; set; }
        public string TaskInfo { get; set; }
        public bool Inner { get; set; }
        public bool Emergency { get; set; }
        public bool Cha { get; set; }
        public bool Xiu { get; set; }

        public string ProcessTypeName { get; set; }
        public int Sort { get; set; }
    }
}
