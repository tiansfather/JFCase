using Abp.Domain.Entities;
using Master.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Master.MES
{
    /// <summary>
    /// 任务报工记录
    /// </summary>
    public class ProcessTaskReport : BaseFullEntityWithTenant, IHaveStatus
    {
        public string Status { get; set; }
        public int ProcessTaskId { get; set; }
        /// <summary>
        /// 任务
        /// </summary>
        public virtual ProcessTask ProcessTask { get; set; }
        public int ReporterId { get; set; }
        /// <summary>
        /// 汇报人
        /// </summary>
        public virtual Person Reporter { get; set; }
        /// <summary>
        /// 填写的汇报时间
        /// </summary>
        public DateTime ReportTime { get; set; }
        /// <summary>
        /// 汇报类型
        /// </summary>
        public ReportType ReportType { get; set; }

        public decimal Progress { get; set; }
        [NotMapped]
        public List<UploadFileInfo> Files
        {
            get
            {
                try
                {
                    return this.GetData<List<UploadFileInfo>>("files");
                }
                catch
                {

                }
                return new List<UploadFileInfo>();
                
            }
            set
            {
                this.SetData("files", value);
            }
        }
    }

    public enum ReportType
    {
        /// <summary>
        /// 到料
        /// </summary>
        到料=1,
        /// <summary>
        /// 上机
        /// </summary>
        上机=2,
        /// <summary>
        /// 加工中
        /// </summary>
        加工=3,
        /// <summary>
        /// 暂停
        /// </summary>
        暂停=4,
        /// <summary>
        /// 结束
        /// </summary>
        下机=5,
        /// <summary>
        /// 暂停后重新开始
        /// </summary>
        重新开始=6,
    }
}
