using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Case
{
    [AutoMap(typeof(CaseSourceHistory))]
    public class CaseSourceHistoryDto
    {
        public int Id { get; set; }
        public string Reason { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
