﻿using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Case
{
    [AutoMap(typeof(CaseInitial))]
    public class CaseInitialDto
    {
        public int Id { get; set; }
        public int CaseSourceId { get; set; }
        public int? SubjectId { get; set; }
        public string SubjectDisplayName { get; set; }
        /// <summary>
        /// 焦点
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
        public string Status { get; set; }
        public DateTime? PublisDate { get; set; }
        /// <summary>
        /// 阅读量
        /// </summary>
        public int ReadNumber { get; set; }
        /// <summary>
        /// 点赞数
        /// </summary>
        public int PraiseNumber { get; set; }
        /// <summary>
        /// 拍砖数
        /// </summary>
        public int BeatNumber { get; set; }

        public CaseStatus CaseStatus { get; set; }

        public virtual ICollection<CaseKeyDto> CaseKeys { get; set; }
        public virtual ICollection<CaseFineDto> CaseFines { get; set; }
        public virtual ICollection<CaseCardDto> CaseCards { get; set; }

        /// <summary>
        /// 诉情及判决
        /// </summary>
        public JudgeInfo JudgeInfo { get; set; }
        public string Remarks { get; set; }
    }
}