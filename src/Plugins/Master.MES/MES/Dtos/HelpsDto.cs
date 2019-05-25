using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.MES.Dtos
{
    [AutoMap(typeof(MESHelps))]
    public class HelpsDto
    {
        public int Id { get; set; }
        public string HelpTitle { get; set; }
        public string HelpContent { get; set; }
    }
}
