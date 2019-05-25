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
        [InterColumn(ColumnName ="标题")]
        public string Title { get; set; }
        /// <summary>
        /// 纠纷类型
        /// </summary>
        public int JiuFenLeiXingId { get; set; }
        public virtual BaseTree JiuFenLeiXing { get; set; }
        /// <summary>
        /// 纠纷原因
        /// </summary>
        public int JiuFenYuanYinId { get; set; }
        public virtual BaseTree JiuFenYuanYin { get; set; }
        /// <summary>
        /// 判决结果
        /// </summary>
        public int PanJueJieGuoId { get; set; }
        public virtual BaseTree PanJueJieGuo { get; set; }
        /// <summary>
        /// 专题
        /// </summary>
        public int ZhuanTiId { get; set; }
        public virtual BaseTree ZhuanTi { get; set; }
        public string Keywords { get; set; }
        public string Labels { get; set; }
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
        public DateTime? PublisDate { get; set; }
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
    }

    public enum CaseStatus
    {
        加工中,
        展示中,
        退回,
        下架,
        推荐,
        须修整
    }
}
