using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Case
{
    [AutoMap(typeof(CaseNode))]
    public class CaseNodeDto
    {
        public int Id { get; set; }
        public string KeyName { get; set; }
        /// <summary>
        /// 具体的值
        /// </summary>
        public string KeyValue { get; set; }
        /// <summary>
        /// 对应节点id
        /// </summary>
        public int KeyNodeId { get; set; }
    }
}
