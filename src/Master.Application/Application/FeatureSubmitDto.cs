using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Application
{
    public class FeatureSubmitDto
    {
        public string Type { get; set; }
        public int Data { get; set; }
        public Dictionary<string,string> Values { get; set; }
    }
}
