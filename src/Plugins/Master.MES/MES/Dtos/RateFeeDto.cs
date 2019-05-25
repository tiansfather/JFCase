using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.MES.Dtos
{
    [AutoMap(typeof(RateFeeInfo))]
    public class RateFeeDto
    {
        public int Id { get; set; }
        public decimal Fee { get; set; }
        public int Rate { get; set; }
        public QuanlityType QuanlityType { get; set; }
        /// <summary>
        /// 评语
        /// </summary>
        public string RateInfo { get; set; }
    }
}
