using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Search
{
    /// <summary>
    /// SoulTable过滤
    /// </summary>
    public class SoulTableConditionDto
    {
        public int Id { get; set; }
        public bool Head { get; set; }
        public string Prefix { get; set; }
        public string Mode { get; set; }
        public string Field { get; set; }
        public List<string> Values { get; set; }
        public List<SoulTableConditionDto> Children { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
        public int? GroupId { get; set; }
    }
}
