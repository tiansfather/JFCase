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
        /// <summary>
        /// 初加工、精加工
        /// </summary>
        public string RelType { get; set; }
        /// <summary>
        /// 节点名称：纠纷原因
        /// </summary>
        public string RelName { get; set; }
        /// <summary>
        /// 具体的值：
        /// </summary>
        public string RelValue { get; set; }
        /// <summary>
        /// 对应分类树节点id
        /// </summary>
        public int BaseTreeId { get; set; }
    }
}
