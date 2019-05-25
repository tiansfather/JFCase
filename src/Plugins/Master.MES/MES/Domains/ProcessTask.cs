using Master.Units;
using Master.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using Master.Projects;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.AutoMapper;
using Master.MES.Dtos;

namespace Master.MES
{
    /// <summary>
    /// 加工任务
    /// </summary>
    public class ProcessTask : BaseFullEntityWithTenant, IHaveStatus,IHaveSort
    {
        #region 任务状态记号常量
        /// <summary>
        /// 核算确认标记
        /// </summary>
        public const string Status_AccountingPass = "AccountingConfirmed";
        /// <summary>
        /// 核算确认标记
        /// </summary>
        public const string Status_AccountingDeny = "AccountingDeny";
        /// <summary>
        /// 是否加急标记
        /// </summary>
        public const string Status_Emergency = "Emergency";
        /// <summary>
        /// 是否开票标记
        /// </summary>
        public const string Status_MakeInvoice= "MakeInvoice";
        /// <summary>
        /// 加工审核标记
        /// </summary>
        public const string Status_ProcessConfirm = "ProcessConfirm";
        /// <summary>
        /// 回单审核标记
        /// </summary>
        public const string Status_Verify = "Verify";
        /// <summary>
        /// 对账标记
        /// </summary>
        public const string Status_Checked = "Checked";
        /// <summary>
        /// 厂内加工标记
        /// </summary>
        public const string Status_Inner = "Inner";
        /// <summary>
        /// 已打印标记
        /// </summary>
        public const string Status_Print = "Print";
        /// <summary>
        /// 发送外协标记
        /// </summary>
        public const string Status_SendProcessor = "SendProcessor";
        /// <summary>
        /// 外协已查看标记
        /// </summary>
        public const string Status_ProcessorReaded = "ProcessorReaded";
        /// <summary>
        /// 插单标记
        /// </summary>
        public const string Status_Cha = "Cha";
        /// <summary>
        /// 修模标记 
        /// </summary>
        public const string Status_Xiu = "Xiu";
        /// <summary>
        /// 手机开单标记
        /// </summary>
        public const string Status_FromMobile = "FromMobile";
        /// <summary>
        /// 已询价标记
        /// </summary>
        public const string Status_Quoted = "Quoted";
        #endregion

        #region 数据绑定字段

        public string Status { get; set; } = "";
        public int? SupplierId { get; set; }
        [ForeignKey("SupplierId")]
        public virtual Unit Supplier { get; set; }

        public int? EquipmentId { get; set; }
        public virtual Equipment Equipment { get; set; }
        /// <summary>
        /// 加工单号
        /// </summary>
        public string ProcessSN { get; set; }
        public int PartId { get; set; }
        /// <summary>
        /// 加工零件
        /// </summary>
        public virtual Part Part { get; set; }
        public int ProcessTypeId { get; set; }
        /// <summary>
        /// 加工工艺
        /// </summary>
        public virtual ProcessType ProcessType { get; set; }
        /// <summary>
        /// 预计工时
        /// </summary>
        public decimal? EstimateHours { get; set; }
        /// <summary>
        /// 实际工时
        /// </summary>
        public decimal? ActualHours { get; set; }
        /// <summary>
        /// 计价方式
        /// </summary>
        public FeeType FeeType { get; set; }
        /// <summary>
        /// 计算因子,如长度\平方\重量\时间\数量
        /// </summary>
        public decimal? FeeFactor { get; set; }
        /// <summary>
        /// 承包金额 
        /// </summary>
        public decimal? JobFee { get; set; }
        /// <summary>
        /// 实际金额 
        /// </summary>
        public decimal? Fee { get; set; }
        /// <summary>
        /// 对账金额
        /// </summary>
        public decimal? CheckFee { get; set; }
        /// <summary>
        /// 要求完成时间|计划结束时间
        /// </summary>
        public DateTime? RequireDate { get; set; }
        /// <summary>
        /// 预约时间|计划开始时间
        /// </summary>
        public DateTime? AppointDate { get; set; }
        /// <summary>
        /// 安排上机时间
        /// </summary>
        public DateTime? ArrangeDate { get; set; }
        /// <summary>
        /// 安排下机时间
        /// </summary>
        public DateTime? ArrangeEndDate { get; set; }
        /// <summary>
        /// 到料时间
        /// </summary>
        public DateTime? ReceiveDate { get; set; }
        /// <summary>
        /// 实际开始时间
        /// </summary>
        public DateTime? StartDate { get; set; }
        /// <summary>
        /// 实际结束时间
        /// </summary>
        public DateTime? EndDate { get; set; }
        /// <summary>
        /// 任务说明
        /// </summary>
        public string TaskInfo { get; set; }
        /// <summary>
        /// 加工单价
        /// </summary>
        public decimal? Price { get; set; }
        /// <summary>
        /// 进度
        /// </summary>
        public decimal Progress { get; set; }
        /// <summary>
        /// 任务状态
        /// </summary>
        public ProcessTaskStatus ProcessTaskStatus { get; set; } = ProcessTaskStatus.Inputed;
        /// <summary>
        /// 开单人
        /// </summary>
        public string Poster { get; set; }
        /// <summary>
        /// 模具组长
        /// </summary>
        public string ProjectCharger { get; set; }
        /// <summary>
        /// 工艺师
        /// </summary>
        public string CraftsMan { get; set; }
        /// <summary>
        /// 审核
        /// </summary>
        public string Verifier { get; set; }
        /// <summary>
        /// 检验
        /// </summary>
        public string Checker { get; set; }
        /// <summary>
        /// 报工记录
        /// </summary>
        public virtual ICollection<ProcessTaskReport> ProcessTaskReports { get; set; }   
        /// <summary>
        /// 序号
        /// </summary>
        public int Sort { get; set; }
        /// <summary>
        /// 开单日期
        /// </summary>
        public DateTime? KaiDate { get; set; }
        /// <summary>
        /// 接收方账套Id
        /// </summary>
        public int? ToTenantId { get; set; }
        /// <summary>
        /// 接收方预计工时
        /// </summary>
        public decimal? ToTenantEstimateHours { get; set; }

        #endregion

        #region 相关文件

        /// <summary>
        /// 附件
        /// </summary>
        [NotMapped]
        public List<UploadFileInfo> Files
        {
            get
            {
                var files= this.GetData<List<UploadFileInfo>>("files");
                return files ?? new List<UploadFileInfo>();
            }
            set
            {
                this.SetData("files", value);
            }
        }
        /// <summary>
        /// 回单审核图片
        /// </summary>
        [NotMapped]
        public List<UploadFileInfo> ReturnFiles
        {
            get
            {
                var files= this.GetData<List<UploadFileInfo>>("returnFiles");
                return files ?? new List<UploadFileInfo>();
            }
            set
            {
                this.SetData("returnFiles", value);
            }
        }
        /// <summary>
        /// 加工检测文件
        /// </summary>
        [NotMapped]
        public List<UploadFileInfo> CheckFiles
        {
            get
            {
                var files= this.GetData<List<UploadFileInfo>>("checkFiles");
                return files ?? new List<UploadFileInfo>();
            }
            set
            {
                this.SetData("checkFiles", value);
            }
        }
        /// <summary>
        /// 编程文件
        /// </summary>
        [NotMapped]
        public List<UploadFileInfo> ProgramFiles
        {
            get
            {
                var files= this.GetData<List<UploadFileInfo>>("programFiles");
                return files ?? new List<UploadFileInfo>();
            }
            set
            {
                this.SetData("programFiles", value);
            }
        }
        /// <summary>
        /// 加工图片
        /// </summary>
        [NotMapped]
        public UploadFileInfo SheetFile
        {
            get
            {
                return this.GetData<UploadFileInfo>("SheetFile");
            }
            set
            {
                this.SetData("SheetFile", value);
            }
        }
        #endregion

        #region 状态
        [NotMapped]
        public bool Emergency
        {
            get
            {
                return this.HasStatus(Status_Emergency);
            }
            set
            {
                if (value)
                {
                    this.SetStatus(Status_Emergency);
                }
                else
                {
                    this.RemoveStatus(Status_Emergency);
                }
                
            }
        }
        [NotMapped]
        public bool Inner
        {
            get
            {
                return this.HasStatus(Status_Inner);
            }
            set
            {
                if (value)
                {
                    this.SetStatus(Status_Inner);
                }
                else
                {
                    this.RemoveStatus(Status_Inner);
                }
            }
        }
        [NotMapped]
        public bool ProcessConfirm
        {
            get
            {
                return this.HasStatus(Status_ProcessConfirm);
            }
            set
            {
                if (value)
                {
                    this.SetStatus(Status_ProcessConfirm);
                }
                else
                {
                    this.RemoveStatus(Status_ProcessConfirm);
                }
            }
        }
        [NotMapped]
        public bool Cha
        {
            get
            {
                return this.HasStatus(Status_Cha);
            }
            set
            {
                if (value)
                {
                    this.SetStatus(Status_Cha);
                }
                else
                {
                    this.RemoveStatus(Status_Cha);
                }
            }
        }
        [NotMapped]
        public bool Xiu
        {
            get
            {
                return this.HasStatus(Status_Xiu);
            }
            set
            {
                if (value)
                {
                    this.SetStatus(Status_Xiu);
                }
                else
                {
                    this.RemoveStatus(Status_Xiu);
                }
            }
        }
        [NotMapped]
        public bool FromMobile
        {
            get
            {
                return this.HasStatus(Status_FromMobile);
            }
            set
            {
                if (value)
                {
                    this.SetStatus(Status_FromMobile);
                }
                else
                {
                    this.RemoveStatus(Status_FromMobile);
                }
            }
        }
        [NotMapped]
        public bool Quoted
        {
            get
            {
                return this.HasStatus(Status_Quoted);
            }
            set
            {
                if (value)
                {
                    this.SetStatus(Status_Quoted);
                }
                else
                {
                    this.RemoveStatus(Status_Quoted);
                }
            }
        }
        [NotMapped]
        public bool Checked
        {
            get
            {
                return this.HasStatus(Status_Checked);
            }
            set
            {
                if (value)
                {
                    this.SetStatus(Status_Checked);
                }
                else
                {
                    this.RemoveStatus(Status_Checked);
                }
            }
        }
        #endregion

        #region 加工费用明细
        /// <summary>
        /// 加工费用明细
        /// </summary>
        [NotMapped]
        public List<ProcessTaskDetail> ProcessTaskDetails
        {
            get
            {
                return this.GetData<List<ProcessTaskDetail>>("ProcessTaskDetails");
            }
            set
            {
                this.SetData("ProcessTaskDetails", value);
            }
        }
        #endregion]

        #region 回单审核明细
        /// <summary>
        /// 回单审核明细
        /// </summary>
        [NotMapped]
        public List<RateFeeInfo> RateFeeInfos
        {
            get
            {
                return this.GetData<List<RateFeeInfo>>("RateFeeInfos")?? new List<RateFeeInfo>();
            }
            set
            {
                this.SetData("RateFeeInfos", value);
            }
        }
        #endregion

        #region 进度计算
        /// <summary>
        /// 我们帮他算进度
        /// </summary>
        [NotMapped]
        public ProcessTaskProgressInfo ProcessTaskProgressInfo
        {
            get
            {
                var result = new ProcessTaskProgressInfo() ;
                if (Progress > 0)
                {
                    result.Progress = Progress;
                    result.ProgressType = 1;
                }
                 //已上机状态的任务
                else if (ProcessTaskStatus == ProcessTaskStatus.Processing)
                {
                    //有开始时间（报工上来的和在系统手填的）和开单时填了预计工时
                    if (StartDate.HasValue && EstimateHours.HasValue)
                    {
                        var ComputedProgress = Convert.ToDecimal((DateTime.Now - StartDate.Value).TotalHours) / EstimateHours.Value;
                        if (ComputedProgress > 1)
                        {
                            result.Progress = 1;
                            result.ProgressType = 3;

                        }
                        else
                        {
                            result.Progress = ComputedProgress;
                            result.ProgressType = 2;

                        }
                    }
                }
                return result;
            }
        }
        #endregion

        #region 时间轴时间
        /// <summary>
        /// 时间轴的开始日期，如果未上机，取安排时间，已上机，取上机时间
        /// </summary>
        [NotMapped]
        public DateTime TimeLineStartDate
        {
            get
            {
                return StartDate ?? (ArrangeDate??DateTime.Now);
            }
        }
        /// <summary>
        /// 时间轴的结束日期，如果已下机，取下机时间，否则使用预计工时计算
        /// </summary>
        [NotMapped]
        public DateTime TimeLineEndDate
        {
            get
            {
                return EndDate ?? TimeLineStartDate.AddHours(Convert.ToDouble(EstimateHours ?? 0));
            }
        }
        /// <summary>
        /// 任务是否在某时间段内
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public bool IsInTimeLine(DateTime from,DateTime to)
        {
            return !(TimeLineStartDate > to || TimeLineEndDate < from);
        }
        /// <summary>
        /// 任务在某时间段内的占用时间，用于计算设备占用时间
        /// </summary>
        public decimal CalcTimeLineOccupyTime(DateTime from,DateTime to)
        {
            //如果任务不在此时间段内，直接返回0
            if(!IsInTimeLine(from,to))
            {
                return 0;
            }
            else
            {
                var startDate = TimeLineStartDate;
                var endDate = TimeLineEndDate;
                var taskHours = Convert.ToDecimal((endDate - startDate).TotalHours);
                //如果任务超出了时间范围，则计算占用时长时需要将超出部分的任务时间减掉
                if (startDate < from)
                {
                    taskHours -= Convert.ToDecimal((from - startDate).TotalHours);
                }
                if (endDate > to)
                {
                    taskHours-= Convert.ToDecimal((endDate-to).TotalHours);
                }
                return Math.Round(taskHours,2);
            }
        }
        #endregion

        #region 进度表时间
        [NotMapped]
        public DateTime SchedulePlanStartDate
        {
            get
            {
                return Convert.ToDateTime((AppointDate ?? DateTime.Now).ToString("yyyy-MM-dd"));
            }
        }
        [NotMapped]
        public DateTime SchedulePlanEndDate
        {
            get
            {
                //0->0,12->1,24->1,36->2;工时转换为天数
                var durationDays = Convert.ToDouble(Math.Ceiling((EstimateHours ?? 0) / 24));
                return (RequireDate ?? SchedulePlanStartDate.AddDays(durationDays-1)).AddDays(1);
            }
        }
        #endregion

    }
    /// <summary>
    /// 承包明细
    /// </summary>
    public class ProcessTaskDetail
    {
        /// <summary>
        /// 类别
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public string Number { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public string MeasureMentUnit { get; set; }
        /// <summary>
        /// 单价
        /// </summary>
        public string Price { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public string Cost { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }

    }
    /// <summary>
    /// 进度，如果有报工进度，直接给，没有的话通过现在时间减去开始时间/预计工时
    /// </summary>
    public class ProcessTaskProgressInfo
    {
        /// <summary>
        /// 返回进度0-1
        /// </summary>
        public decimal Progress { get; set; } = 0;
        /// <summary>
        /// 0:什么都都没有1:正常汇报进度1:我们算出来的进度2:我们算出来超过了100%
        /// </summary>
        public int ProgressType { get; set; } = 0;
    }

    /// <summary>
    /// 回单审核明细
    /// </summary>
    public class RateFeeInfo
    {
        public decimal Fee { get; set; }
        public int Rate { get; set; }
        public QuanlityType QuanlityType { get; set; }
        /// <summary>
        /// 评语
        /// </summary>
        public string RateInfo { get; set; }
        public string Verifier { get; set; }
        public string VerifyTime { get; set; }
    }

    #region 相关类型

    
    public enum FeeType
    {
        承包=0,
        按时间=1,
        按平方=2,
        按长度=3,
        按重量=4,
        按数量=5
    }

    /// <summary>
    /// 品质类型
    /// </summary>
    public enum QuanlityType
    {
        未检=0,
        合格=1,
        不合格=2
    }

    public enum ProcessTaskStatus
    {
        /// <summary>
        /// 已录入待开单
        /// </summary>
        Inputed=0,
        /// <summary>
        /// 待上机|待加工
        /// </summary>
        WaitForProcess=1,
        /// <summary>
        /// 加工点已到料
        /// </summary>
        Received=2,
        /// <summary>
        /// 已上机|加工中
        /// </summary>
        Processing=3,
        /// <summary>
        /// 已完成
        /// </summary>
        Completed=4,
        /// <summary>
        /// 暂停中
        /// </summary>
        Suspended=5,
        /// <summary>
        /// 已取消
        /// </summary>
        Canceled=-1,
        /// <summary>
        /// 已对账
        /// </summary>
        //Checked=6
    }
    /// <summary>
    /// 延期类型
    /// </summary>
    public enum DelayType
    {
        /// <summary>
        /// 延期上机
        /// </summary>
        DelayStart = 1,
        /// <summary>
        /// 延期下机
        /// </summary>
        DelayEnd = 2,
        /// <summary>
        /// 到料未上机
        /// </summary>
        ReceiveNotStart=3,
        /// <summary>
        /// 工时超预计
        /// </summary>
        ExceedHour=4,
    }
    #endregion
}
