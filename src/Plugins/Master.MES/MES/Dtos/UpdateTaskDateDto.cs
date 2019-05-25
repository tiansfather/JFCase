using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.MES.Dtos
{
    /// <summary>
    /// 系统中直接提交报工时间
    /// </summary>   
    [AutoMap(typeof(ProcessTask))]
    public class UpdateTaskDateDto
    {
        public int Id { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? ReceiveDate { get; set; }
        public DateTime? ArrangeDate { get; set; }
    }
}
