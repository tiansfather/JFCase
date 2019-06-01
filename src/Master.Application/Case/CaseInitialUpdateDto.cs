using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Case
{
    /// <summary>
    /// 初加工提交
    /// </summary>
    public class CaseInitialUpdateDto
    {
        public int Id { get; set; }
        /// <summary>
        /// 案例焦点
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 案例概述
        /// </summary>
        public string Introduction { get; set; }
        /// <summary>
        /// 法律法规
        /// </summary>
        public string Law { get; set; }
        /// <summary>
        /// 经验分享
        /// </summary>
        public string Experience { get; set; }
        /// <summary>
        ///律师说
        /// </summary>
        public string LawyerOpinion { get; set; }
        public virtual ICollection<CaseNodeDto> CaseNodes { get; set; }
        public JudgeInfo JudgeInfo { get; set; }
        public string Remarks { get; set; }
    }
}
