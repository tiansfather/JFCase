using Abp.Domain.Entities;
using Master.Entity;
using Master.Module.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Master.Case
{
    /// <summary>
    /// 初加工
    /// </summary>
    [InterModule("成品案例", GenerateDefaultColumns=false,GenerateDefaultButtons =false)]
    public class CaseInitial : BaseFullEntityWithTenant, IHaveStatus,IPassivable
    {
        [InterColumn(ColumnName ="案号",ValuePath="CaseSource.SourceSN")]
        [NotMapped]
        public string SourceSN {
            get
            {
                return CaseSource.SourceSN;
            }
        }
        [InterColumn(ColumnName = "城市", ValuePath = "CaseSource.City.DisplayName")]
        [NotMapped]
        public string City
        {
            get
            {
                return CaseSource.City.DisplayName;
            }
        }
        [InterColumn(ColumnName = "案由", ValuePath = "CaseSource.AnYou.DisplayName")]
        [NotMapped]
        public string AnYou
        {
            get
            {
                return CaseSource.AnYou.DisplayName;
            }
        }

        public int CaseSourceId { get; set; }
        public virtual CaseSource CaseSource { get; set; }
        public int? SubjectId { get; set; }
        public virtual BaseTree Subject { get; set; }
        [InterColumn(ColumnName ="标题")]
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
        [NotMapped]
        [InterColumn(ColumnName = "加工人",ValuePath = "CreatorUser.Name")]
        public string Processor
        {
            get
            {
                return CreatorUser.Name;
            }
        }
        /// <summary>
        /// 发布日期
        /// </summary>
        [InterColumn(ColumnName = "发布日期")]
        public DateTime? PublishDate { get; set; }
        /// <summary>
        /// 阅读量
        /// </summary>
        public int ReadNumber { get; set; }
        /// <summary>
        /// 点赞数
        /// </summary>
        [InterColumn(ColumnName = "点赞数")]
        public int PraiseNumber { get; set; }
        /// <summary>
        /// 拍砖数
        /// </summary>
        [InterColumn(ColumnName = "拍砖数")]
        public int BeatNumber { get; set; }
        
        public bool IsActive { get; set; }
        [InterColumn(ColumnName = "状态",ColumnType =Module.ColumnTypes.Select,DictionaryName = "Master.Case.CaseStatus")]
        public CaseStatus CaseStatus { get; set; }

        public virtual ICollection<CaseNode> CaseNodes { get; set; }
        public virtual ICollection<CaseLabel> CaseLabels { get; set; }
        public virtual ICollection<CaseFine> CaseFines { get; set; }
        public virtual ICollection<CaseCard> CaseCards { get; set; }

        /// <summary>
        /// 诉情及判决
        /// </summary>
        [NotMapped]
        public JudgeInfo JudgeInfo
        {
            get
            {
                return this.GetPropertyValue<JudgeInfo>("JudgeInfo");
            }
            set
            {
                this.SetPropertyValue("JudgeInfo", value);
            }
        }
    }

    public enum CaseStatus
    {
        加工中,
        展示中,
        退回,
        下架
    }

    public class JudgeInfo
    {
        /// <summary>
        /// 诉情
        /// </summary>
        public string JudgeContent { get; set; }
        /// <summary>
        /// 判决
        /// </summary>
        public string JudgeResult { get; set; }
        /// <summary>
        /// 支持率0-100
        /// </summary>
        public int SupportRate { get; set; }
        /// <summary>
        /// 是否有反诉
        /// </summary>
        public bool HasReverseJudge { get; set; }
        /// <summary>
        /// 反诉内容
        /// </summary>
        public string ReverseJudgeContent { get; set; }
        public string ReverseJudgeResult { get; set; }
        /// <summary>
        /// 反诉支持率0-100
        /// </summary>
        public int ReverseSupportRate { get; set; }
    }
}
