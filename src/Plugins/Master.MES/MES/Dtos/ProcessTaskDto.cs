using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.MES.Dtos
{
    /// <summary>
    /// 加工任务录入,此类尽量不要修改
    /// </summary>
    [AutoMap(typeof(ProcessTask))]
    public class ProcessTaskDto
    {
        public int Id { get; set; }
        public string ProjectSN { get; set; }
        public string ProcessSN { get; set; }
        public string PartSN { get; set; }
        public string PartName { get; set; }
        public string PartSpecification { get; set; }
        public int PartNum { get; set; }
        public string UnitName { get; set; }
        public string EquipmentSN { get; set; }

        public string Reason { get; set; }
        /// <summary>
        /// 预计工时
        /// </summary>
        public decimal? EstimateHours { get; set; }
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
        /// 工艺名称
        /// </summary>
        public string ProcessTypeName { get; set; }
        /// <summary>
        /// 要求完成时间
        /// </summary>
        public DateTime? RequireDate { get; set; }
        /// <summary>
        /// 预约时间
        /// </summary>
        public DateTime? AppointDate { get; set; }
        /// <summary>
        /// 安排时间
        /// </summary>
        //public DateTime? ArrangeDate { get; set; }
        ///// <summary>
        ///// 实际开始时间
        ///// </summary>
        //public DateTime? StartDate { get; set; }
        ///// <summary>
        ///// 实际结束时间
        ///// </summary>
        //public DateTime? EndDate { get; set; }
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
        /// 上传附件
        /// </summary>
        public List<UploadFileInfo> Files { get; set; }
        /// <summary>
        /// 加工图片
        /// </summary>
        public UploadFileInfo SheetFile { get; set; }
        /// <summary>
        /// 加急
        /// </summary>
        public bool Emergency { get; set; }
        /// <summary>
        /// 厂内
        /// </summary>
        public bool Inner { get; set; }
        /// <summary>
        /// 插单
        /// </summary>
        public bool Cha { get; set; }
        /// <summary>
        /// 修模
        /// </summary>
        public bool Xiu { get; set; }
        /// <summary>
        /// 手机开单
        /// </summary>
        public bool FromMobile { get; set; }
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
        /// 费用明细清单
        /// </summary>
        public List<ProcessTaskDetail> ProcessTaskDetails { get; set; }
        /// <summary>
        /// 用于批量开单的零件集合
        /// </summary>
        public List<ProcessTaskPartDto> ProcessTaskParts { get; set; }
    }

    public class ProcessTaskPartDto
    {
        public string PartSN { get; set; }
        public string PartName { get; set; }
        public string PartSpecification { get; set; }
        public int PartNum { get; set; }
        public UploadFileInfo SheetFile { get; set; }
    }
    /// <summary>
    /// 加工任务信息
    /// </summary>
    [AutoMap(typeof(ProcessTask))]
    public class ProcessTaskViewDto
    {
        public int Id { get; set; }
        public string ProjectSN { get; set; }
        public string ProcessSN { get; set; }
        public string PartSN { get; set; }
        public string PartName { get; set; }
        public string PartSpecification { get; set; }
        public int PartNum { get; set; }
        public string UnitName { get; set; }
        public string EquipmentSN { get; set; }
        /// <summary>
        /// 预计工时
        /// </summary>
        public decimal? EstimateHours { get; set; }
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
        /// 工艺名称
        /// </summary>
        public string ProcessTypeName { get; set; }
        /// <summary>
        /// 要求完成时间
        /// </summary>
        public DateTime? RequireDate { get; set; }
        /// <summary>
        /// 预约时间
        /// </summary>
        public DateTime? AppointDate { get; set; }
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
        public ProcessTaskProgressInfo ProcessTaskProgressInfo { get; set; }

        /// <summary>
        /// 上传附件
        /// </summary>
        public List<UploadFileInfo> Files { get; set; }
        /// <summary>
        /// 加工图片
        /// </summary>
        public UploadFileInfo SheetFile { get; set; }
        /// <summary>
        /// 加急
        /// </summary>
        public bool Emergency { get; set; }
        /// <summary>
        /// 厂内
        /// </summary>
        public bool Inner { get; set; }
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
        public ProcessTaskStatus ProcessTaskStatus { get; set; }
        public DateTime? ArrangeDate { get; set; }
        public decimal? ActualHours { get; set; }
    }
}
