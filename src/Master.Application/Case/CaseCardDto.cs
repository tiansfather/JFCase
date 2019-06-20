using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Case
{
    [AutoMap(typeof(CaseCard))]
    public class CaseCardDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Remarks { get; set; }
        public CaseStatus CaseStatus { get; set; }
    }
}
