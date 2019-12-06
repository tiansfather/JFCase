using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Case
{
    /// <summary>
    /// 初加工提交
    /// </summary>
    [AutoMap(typeof(CaseInitial))]
    public class CaseInitialUpdateDto
    {
        public int Id { get; set; }
        public int? SubjectId { get; set; }
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
        public virtual ICollection<CaseNodeDto> CaseNodes { get; set; } = new List<CaseNodeDto>();
        public virtual ICollection<CaseLabelDto> CaseLabels { get; set; } = new List<CaseLabelDto>();
        public JudgeInfo JudgeInfo { get; set; } = new JudgeInfo();
        public string Remarks { get; set; }
    }
}
