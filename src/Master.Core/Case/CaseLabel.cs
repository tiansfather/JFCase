using Abp.Domain.Entities.Auditing;
using Master.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Case
{
    /// <summary>
    /// 与初加工精加工关联的标签
    /// </summary>
    public class CaseLabel : CreationAuditedEntity<int>
    {
        /// <summary>
        /// 初加工、精加工
        /// </summary>
        public string RelType { get; set; }
        /// <summary>
        /// 节点名称
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
        public virtual Label Label { get; set; }
        public int? CaseInitialId { get; set; }
        public virtual CaseInitial CaseInitial { get; set; }
    }
}
