using Master.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.MES
{
    /// <summary>
    /// 人员
    /// </summary>
    public class Person : BaseFullEntity, IHaveStatus
    {
        public string Status { get; set; }
        /// <summary>
        /// 人员名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 人员来源
        /// </summary>
        public PersonSource PersonSource { get; set; } = PersonSource.MLMW;
    }

    public enum PersonSource
    {
        /// <summary>
        /// 云报工
        /// </summary>
        MLMW=1,
        /// <summary>
        /// 原云加工平台
        /// </summary>
        MES=2
    }
}
