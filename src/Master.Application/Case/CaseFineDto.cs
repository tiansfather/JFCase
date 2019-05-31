using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Case
{
    [AutoMap(typeof(CaseFine))]
    public class CaseFineDto
    {
        public int Id { get; set; }
        public virtual ICollection<CaseNodeDto> CaseNodes { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string MediaPath { get; set; }
        public string Remarks { get; set; }
    }
}
