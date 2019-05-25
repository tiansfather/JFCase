using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.MES.Dtos
{
    #region 提交
    [AutoMap(typeof(ProcessQuote))]
    public class ProcessQuoteSubmitDto
    {
        public int Id { get; set; }
        public string QuoteName { get; set; }
        public QuoteScope QuoteScope { get; set; }
        public QuotePayType QuotePayType { get; set; }
        public DateTime ExpireDate { get; set; }
        public List<int> UnitIds { get; set; }
        public List<ProcessQuoteTaskSubmitDto> ProcessQuoteTasks { get; set; } = new List<ProcessQuoteTaskSubmitDto>();
        public List<UploadFileInfo> Files { get; set; }
        public string Remarks { get; set; }
    }

    [AutoMap(typeof(ProcessQuoteTask))]
    public class ProcessQuoteTaskSubmitDto
    {
        public int Id { get; set; }
        public int? ProcessTaskId { get; set; }
        public string ProcessSN { get; set; }
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
    #endregion

    #region 查看
    [AutoMap(typeof(ProcessQuote))]
    public class ProcessQuoteDto
    {
        public int Id { get; set; }
        public string QuoteSN { get; set; }
        public string QuoteName { get; set; }
        public QuoteScope QuoteScope { get; set; }
        public QuotePayType QuotePayType { get; set; }
        public DateTime ExpireDate { get; set; }
        public DateTime? PublishDate { get; set; }
        public List<ProcessQuoteBidDto> ProcessQuoteBids { get; set; }
        public List<ProcessQuoteTaskSubmitDto> ProcessQuoteTasks { get; set; } 
        public List<UploadFileInfo> Files { get; set; }
        public string Remarks { get; set; }
        public string TenancyName { get; set; }
        public QuoteStatus QuoteStatus { get; set; }
    }
    [AutoMap(typeof(ProcessQuoteBid))]
    public class ProcessQuoteBidDto
    {
        public int Id { get; set; }
        /// <summary>
        /// 对应往来单位
        /// </summary>
        public int? UnitId { get; set; }
        public string UnitUnitName { get; set; }
        /// <summary>
        /// 投标方账套
        /// </summary>
        public int? ToTenantId { get; set; }
        /// <summary>
        /// 投标日期
        /// </summary>
        public DateTime? BidDate { get; set; }
        /// <summary>
        /// 1:投标,0:放弃,null，未投
        /// </summary>
        public int? BidType { get; set; }
        public string Remarks { get; set; }
        /// <summary>
        /// 加工点询价状态
        /// </summary>
        public QuoteBidStatus QuoteBidStatus { get; set; }
        public BidData BidData { get; set; }
        public decimal? Cost
        {
            get
            {
                return BidData.Cost;
            }
        }
    }
    #endregion

    #region 投标
    public class ProcessorBidDto
    {
        public int ProcessQuoteId { get; set; }
        public int BidType { get; set; }
        public decimal? Cost { get; set; }
        public string Remarks { get; set; }
    }
    #endregion

}
