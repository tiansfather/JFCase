using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Case
{
    [AutoMap(typeof(CaseLabel))]
    public class CaseLabelDto
    {
        /// <summary>
        /// 初加工、精加工
        /// </summary>
        public string RelType { get; set; }
        /// <summary>
        /// 节点名称:关键词、标签
        /// </summary>
        public string RelName { get; set; }
        /// <summary>
        /// 具体的值
        /// </summary>
        public string RelValue { get; set; }
        /// <summary>
        /// 对应标签id
        /// </summary>
        public int LabelId { get; set; }
    }
}
