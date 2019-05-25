using System;
using System.Collections.Generic;
using System.Text;

namespace Master.MES.Domains
{
    /// <summary>
    /// 加工点信息
    /// </summary>
    public class ProcessorInfo
    {
        /// <summary>
        /// 联系人
        /// </summary>
        public string Charger { get; set; }
        /// <summary>
        /// 联系方式
        /// </summary>
        public string ContactInfo { get; set; }
        /// <summary>
        /// 位置信息
        /// </summary>
        public string Location { get; set; }
        /// <summary>
        /// 可加工工艺类型
        /// </summary>
        public List<string> ProcessTypes { get; set; }
        /// <summary>
        /// 从业开始年份
        /// </summary>
        public int JobStartYear { get; set; }
        /// <summary>
        /// 员工数量
        /// </summary>
        public EmployeeNumber EmployeeNumber { get; set; }
        /// <summary>
        /// 开票税率3,6,13,16
        /// </summary>
        public int InvoiceTax { get; set; }
        /// <summary>
        /// 产值规模
        /// </summary>
        public OutputValue OutputValue { get; set; }
        /// <summary>
        /// 编程软件
        /// </summary>
        public List<string> UsingSoftwares { get; set; }
    }

}
