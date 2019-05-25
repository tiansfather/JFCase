using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Abp.Domain.Entities;
using Master.Entity;
using Master.Projects;

namespace Master.MES
{
    /// <summary>
    /// 零件
    /// </summary>
    public class Part : BaseFullEntityWithTenant, IHaveStatus,IHaveSort
    {
        public string Status { get; set; }
        public int ProjectId { get; set; }
        public virtual Project Project { get; set; }
        /// <summary>
        /// 父级Id
        /// </summary>
        public int? ParentId { get; set; }
        /// <summary>
        /// 父零件
        /// </summary>
        public virtual Part Parent { get; set; }
        /// <summary>
        /// 物料编码
        /// </summary>
        public string MaterialCode { get; set; }
        /// <summary>
        /// 零件编码
        /// </summary>
        public string PartSN { get; set; }
        /// <summary>
        /// 零件名称
        /// </summary>
        public string PartName { get; set; }
        /// <summary>
        /// 零件规格
        /// </summary>
        public string PartSpecification { get; set; }
        /// <summary>
        /// 零件数量
        /// </summary>
        public int PartNum { get; set; }
        /// <summary>
        /// 材质
        /// </summary>
        public string Material { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public string MeasureMentUnit { get; set; }
        /// <summary>
        /// BOM来源
        /// </summary>
        public PartSource PartSource { get; set; }
        /// <summary>
        /// 级别:A,B
        /// </summary>
        public PartRank PartRank { get; set; }
        /// <summary>
        /// 需求日期
        /// </summary>
        public DateTime? RequireDate { get; set; }
        /// <summary>
        /// 启用生产-->对应生产BOM
        /// </summary>
        public bool EnableProcess { get; set; }
        /// <summary>
        /// 启用采购-->对应采购BOM
        /// </summary>
        public bool EnableBuy { get; set; }
        /// <summary>
        /// 启用仓库-->对应出库BOM
        /// </summary>
        public bool EnableStorage { get; set; }

        public virtual ICollection<ProcessTask> ProcessTasks { get; set; }

        /// <summary>
        /// 零件图片路径
        /// </summary>
        [NotMapped]
        public string PartImg
        {
            get
            {
                return this.GetData<string>("PartImg");
            }
            set
            {
                this.SetData("PartImg", value);
            }
        }

        public int Sort { get; set; }
    }

    public enum PartSource
    {
        设计=1,
        采购=2,
        仓库=3,
        生产=4,
    }

    public enum PartRank
    {
        A,
        B
    }
}
