using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.MES.Dtos
{
    [AutoMap(typeof(ProcessType))]
    public class ProcessTypeDto
    {
        public int Id { get; set; }
        public string ProcessTypeName { get; set; }
        public decimal? Price { get; set; }
        public int Sort { get; set; }
    }
}
