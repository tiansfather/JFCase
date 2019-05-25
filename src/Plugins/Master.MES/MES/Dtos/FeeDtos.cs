using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.MES.Dtos
{
    /// <summary>
    /// 录入提交Dto
    /// </summary>
    [AutoMap(typeof(ProcessTask))]
    public class FeeDto
    {
        public int Id { get; set; }
        public decimal? EstimateHours { get; set; }
        public decimal? FeeFactor { get; set; }
        public decimal? Price { get; set; }
        public decimal? JobFee { get; set; }
        public decimal? CheckFee { get; set; }
        public string MakeInvoice { get; set; }
    }
}
