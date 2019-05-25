using System;
using System.Collections.Generic;
using System.Text;
using Master.Entity;

namespace Master.MES
{
    /// <summary>
    /// 加工工艺
    /// </summary>
    public class ProcessType:BaseFullEntityWithTenant,IHaveSort
    {
        /// <summary>
        /// 工艺名称
        /// </summary>
        public string ProcessTypeName { get; set; }
        /// <summary>
        /// 单价
        /// </summary>
        public decimal? Price { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
    }
}
