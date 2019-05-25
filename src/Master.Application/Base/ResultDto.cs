using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Dto
{
    public class ResultDto
    {
        public int code { get; set; }
        public string msg { get; set; }
        public object data { get; set; }
    }
}
