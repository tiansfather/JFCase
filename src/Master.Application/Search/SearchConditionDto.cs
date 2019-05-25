using Master.Dto;
using SkyNet.Master.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkyNet.Master.Dto
{
    public class SearchConditionDto
    {
        public string LeftBracket { get; set; }
        public string RightBracket { get; set; }
        public string Operator { get; set; }
        public string Value { get; set; }
        public string Joiner { get; set; }
        public SearchColumnInfoDto Column { get; set; }
    }
}
