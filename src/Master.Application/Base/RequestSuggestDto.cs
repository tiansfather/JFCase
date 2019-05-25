using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Dto
{
    public class RequestSuggestDto:RequestDto
    {
        public string ColumnKey { get; set; }
        public string Keyword { get; set; }
    }
}
