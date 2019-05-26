using Master.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Case
{
    /// <summary>
    /// 与初加工精加工关联的标签
    /// </summary>
    public class CaseKey:BaseFullEntityWithTenant
    {
        /// <summary>
        /// 纠纷原因、关键字、标签
        /// </summary>
        public string KeyName { get; set; }
        /// <summary>
        /// 具体的值
        /// </summary>
        public string KeyValue { get; set; }
        /// <summary>
        /// 对应节点id
        /// </summary>
        public int KeyNodeId { get; set; }
        public virtual BaseTree KeyNode { get; set; }
        public int? CaseInitialId { get; set; }
        public virtual CaseInitial CaseInitial { get; set; }
        public int? CaseFineId { get; set; }
        public virtual CaseFine CaseFine { get; set; }
    }
}
