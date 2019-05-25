using Abp.Domain.Entities;
using Master.Authentication;
using Master.Entity;
using Master.Module.Attributes;
using Master.MultiTenancy;
using Master.Units;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Master.MES
{
    /// <summary>
    /// 询价单
    /// </summary>
    [InterModule("加工询价")]
    public class ProcessQuote:BaseFullEntityWithTenant
    {
        /// <summary>
        /// 询价单号
        /// </summary>
        [InterColumn(ColumnName ="询价单号")]
        public string QuoteSN { get; set; }
        /// <summary>
        /// 询价名称
        /// </summary>
        [InterColumn(ColumnName = "询价名称")]
        public string QuoteName { get; set; }
        /// <summary>
        /// 询价类型：邀标、公开
        /// </summary>
        [InterColumn(ColumnName = "询价类型",ColumnType =Module.ColumnTypes.Select,DictionaryName ="Master.MES.QuoteScope",Templet ="{{d.quoteScope_display}}")]
        public QuoteScope QuoteScope { get; set; }
        /// <summary>
        /// 付款方式
        /// </summary>
        [InterColumn(ColumnName = "付款方式", ColumnType = Module.ColumnTypes.Select, DictionaryName = "Master.MES.QuotePayType", Templet = "{{d.quotePayType_display}}")]
        public QuotePayType QuotePayType { get; set; }
        /// <summary>
        /// 发布日期
        /// </summary>
        [InterColumn(ColumnName = "发布日期", ColumnType = Module.ColumnTypes.DateTime)]
        public DateTime? PublishDate { get; set; }
        /// <summary>
        /// 截止日期
        /// </summary>
        [InterColumn(ColumnName = "截止日期", ColumnType = Module.ColumnTypes.DateTime)]
        public DateTime ExpireDate { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        [InterColumn(ColumnName = "状态" ,ColumnType = Module.ColumnTypes.Select, DictionaryName = "Master.MES.QuoteStatus", Templet = "{{d.quoteStatus_display}}")]
        public QuoteStatus QuoteStatus { get; set; } = QuoteStatus.草稿;

        /// <summary>
        /// 选标人
        /// </summary>
        [InterColumn(ColumnName = "选标人", ColumnType = Module.ColumnTypes.Text, ValuePath ="ChooseUser.Name", Templet = "{{d.chooseUserId_display||''}}")]
        public long? ChooseUserId { get; set; }
        public virtual User ChooseUser { get; set; }

        public virtual ICollection<ProcessQuoteTask> ProcessQuoteTasks { get; set; }
        public virtual ICollection<ProcessQuoteBid> ProcessQuoteBids { get; set; }
        
        
        
        

        #region 相关文件
        /// <summary>
        /// 附件
        /// </summary>
        [NotMapped]
        public List<UploadFileInfo> Files
        {
            get
            {
                var files = this.GetData<List<UploadFileInfo>>("files");
                return files ?? new List<UploadFileInfo>();
            }
            set
            {
                this.SetData("files", value);
            }
        }
        #endregion
    }

    /// <summary>
    /// 询价加工明细
    /// </summary>
    public class ProcessQuoteTask : BaseFullEntityWithTenant
    {
        public int ProcessQuoteId { get; set; }
        public virtual ProcessQuote ProcessQuote { get; set; }
        /// <summary>
        /// 对应加工单任务
        /// </summary>
        public int? ProcessTaskId { get; set; }
        public virtual ProcessTask ProcessTask { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }
        public string PartName { get; set; }
        public string PartSpecification { get; set; }
        public int PartNum { get; set; }
        public DateTime? RequireDate { get; set; }
        public decimal? EstimateHours { get; set; }
        public FeeType FeeType { get; set; }
        public string ProcessTypeName { get; set; }
        /// <summary>
        /// 计算因子,如长度\平方\重量\时间\数量
        /// </summary>
        public decimal? FeeFactor { get; set; }
        public string TaskInfo { get; set; }
    }


    /// <summary>
    /// 投标明细
    /// </summary>
    public class ProcessQuoteBid : BaseFullEntityWithTenant
    {
        public int ProcessQuoteId { get; set; }
        public virtual ProcessQuote ProcessQuote { get;set;}
        /// <summary>
        /// 对应往来单位
        /// </summary>
        public int? UnitId { get; set; }
        public virtual Unit Unit { get; set; }
        /// <summary>
        /// 投标方账套
        /// </summary>
        public int? ToTenantId { get; set; }
        public virtual Tenant ToTenant { get; set; }       
        /// <summary>
        /// 投标日期
        /// </summary>
        public DateTime? BidDate { get; set; }
        /// <summary>
        /// 1:投标,0:放弃,null，未投
        /// </summary>
        public int? BidType { get; set; }
        /// <summary>
        /// 加工点询价状态
        /// </summary>
        public QuoteBidStatus QuoteBidStatus { get; set; } = QuoteBidStatus.待投标;
        [NotMapped]
        public BidData BidData
        {
            get
            {
                var bidInfo= this.GetData<BidData>("BidData");
                return bidInfo ?? new BidData();
            }
            set
            {
                this.SetData("BidData", value);
            }
        }
    }

    /// <summary>
    /// 投标数据
    /// </summary>
    public class BidData
    {
        /// <summary>
        /// 总金额 
        /// </summary>
        public decimal? Cost { get; set; }
    }
    /// <summary>
    /// 付款方式
    /// </summary>
    public enum QuotePayType
    {
        协议=1,
        现金=2,
    }
    /// <summary>
    /// 投标范围
    /// </summary>
    public enum QuoteScope
    {
        /// <summary>
        /// 邀请投标
        /// </summary>
        邀请投标 = 1,
        /// <summary>
        /// 公开投标
        /// </summary>
        公开投标 = 2
    }
    /// <summary>
    /// 询价状态
    /// </summary>
    public enum QuoteStatus
    {
        /// <summary>
        /// 草稿
        /// </summary>
        草稿 = 0,
        /// <summary>
        /// 询价中
        /// </summary>
        询价中 = 1,
        /// <summary>
        /// 已截止
        /// </summary>
        已截止 = 2,
        /// <summary>
        /// 已选标
        /// </summary>
        已选标 = 3,
        /// <summary>
        /// 已流标
        /// </summary>
        已流标 = 4
    }

    public enum QuoteBidStatus
    {
        未加入=-1,
        未发送=0,
        待投标=1,
        已投标=2,
        已放弃=3,
        未中标 = 4,
        已中标 =5,
        
    }
}
