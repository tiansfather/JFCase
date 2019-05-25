using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
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
    [Table("Equipment")]
    [InterModule("设备管理")]
    public class Equipment:BaseFullEntityWithTenant,IHaveStatus,IPassivable
    {
        public int? UnitId { get; set; }
        public virtual Unit Unit { get; set; }
        public string Status { get;set; }        
        /// <summary>
        /// 设备编号
        /// </summary>
        [InterColumn(ColumnName ="设备名称",ColumnType =Module.ColumnTypes.Text,VerifyRules ="required",Sort =1,Templet = "<a dataid=\"1\" class=\"layui-btn layui-btn-xs layui-btn-normal\" buttonname=\"二维码\" params=\"{&quot;area&quot;: [&quot;500px&quot;, &quot;500px&quot;],&quot;btn&quot;:[]}\"   buttonactiontype=\"Form\" buttonactionurl=\"/Home/Show?name=../MES/EquipmentCode&id={{d.id}}\" onclick=\"func.callModuleButtonEvent()\">{{d.equipmentSN}}</a>")]
        public string EquipmentSN { get; set; }
        /// <summary>
        /// 品牌
        /// </summary>
        [InterColumn(ColumnName = "品牌", ColumnType = Module.ColumnTypes.Text,Sort =2)]
        public string Brand { get; set; }
        /// <summary>
        /// 日产能
        /// </summary>
        [InterColumn(ColumnName = "日产能", ColumnType = Module.ColumnTypes.Number, DisplayFormat = "0", Sort = 3,DefaultValue ="24")]
        public decimal? DayCapacity { get; set; }
        /// <summary>
        /// 行程
        /// </summary>
        [InterColumn(ColumnName = "行程", ColumnType = Module.ColumnTypes.Text,Sort =4)]
        public string Range { get; set; }
        /// <summary>
        /// 加工单价
        /// </summary>
        [InterColumn(ColumnName = "加工单价", ColumnType = Module.ColumnTypes.Number,DisplayFormat ="0.00",Sort =5,VerifyRules ="required")]
        public decimal? Price { get; set; }
        /// <summary>
        /// 购置年份
        /// </summary>
        [InterColumn(ColumnName = "购置年份", ColumnType = Module.ColumnTypes.Number, DisplayFormat = "0",Sort =6)]
        public int? BuyYear { get; set; }
        /// <summary>
        /// 购置金额 
        /// </summary>
        [InterColumn(ColumnName = "购置金额", ColumnType = Module.ColumnTypes.Number, DisplayFormat = "0.00", Sort =7)]
        public decimal? BuyCost { get; set; }
        [NotMapped]
        [InterColumn(ColumnName ="设备图片",ColumnType =Module.ColumnTypes.Images,MaxFileNumber =1,ValuePath ="Property",Sort =8)]
        public int? EquipmentPic
        {
            get
            {
                return this.GetPropertyValue<int?>("EquipmentPic");
            }
            set
            {
                this.SetPropertyValue("EquipmentPic", value);
            }
        }        

        [InterColumn(ColumnName = "编程员", ColumnType = Module.ColumnTypes.Reference, RelativeDataType = Module.RelativeDataType.Module, RelativeDataString = nameof(User), MaxReferenceNumber = "1", ReferenceItemTpl = "name", ReferenceSearchColumns = "name", ReferenceSearchWhere = "{\"where\":\"isActive=true\"}", Sort = 6, DisplayPath = "Programmer.Name", Templet = "{{d.programmerId_display||''}}")]
        public long? ProgrammerId { get; set; }
        [ForeignKey("ProgrammerId")]
        public virtual User Programmer { get; set; }

        [InterColumn(ColumnName = "负责人", ColumnType = Module.ColumnTypes.Reference, RelativeDataType = Module.RelativeDataType.Module, RelativeDataString = nameof(User), MaxReferenceNumber = "1", ReferenceItemTpl = "name", ReferenceSearchColumns = "name", ReferenceSearchWhere = "{\"where\":\"isActive=true\"}", Sort = 6, DisplayPath = "Arranger.Name", Templet = "{{d.arrangerId_display||''}}")]
        public long? ArrangerId { get; set; }
        [ForeignKey("ArrangerId")]
        public virtual User Arranger { get; set; }

        [InterColumn(ColumnName = "操作工", ColumnType = Module.ColumnTypes.Reference, RelativeDataType = Module.RelativeDataType.Module, RelativeDataString = nameof(User), MaxReferenceNumber = "1", ReferenceItemTpl = "name", ReferenceSearchColumns = "name", ReferenceSearchWhere = "{\"where\":\"isActive=true\"}", Sort = 6, DisplayPath = "Operator.Name", Templet = "{{d.operatorId_display==null?'':d.operatorId_display}}", IsShownInAdd = false, IsShownInEdit = false)]
        public long? OperatorId { get; set; }
        [ForeignKey("OperatorId")]
        public virtual User Operator { get; set; }

        public bool IsActive { get; set; } = true;

        public virtual ICollection<EquipmentProcessType> EquipmentProcessTypes { get; set; }
    }

    [Table("EquipmentProcessType")]
    public class EquipmentProcessType : CreationAuditedEntity<int>,IMustHaveTenant
    {
        public int EquipmentId { get; set; }
        public virtual Equipment Equipment { get; set; }
        public int ProcessTypeId { get; set; }
        public virtual ProcessType ProcessType{get;set;}
        public int TenantId { get; set; }
        public virtual Tenant Tenant { get; set; }
    }

    /// <summary>
    /// 设备负荷信息
    /// </summary>
    public class EquipmentLoadInfo
    {
        public int EquipmentId { get; set; }
        /// <summary>
        /// 开始日期
        /// </summary>
        public DateTime DateStart { get; set; }
        /// <summary>
        /// 结束日期
        /// </summary>
        public DateTime DateEnd { get; set; }
        /// <summary>
        /// 总产能
        /// </summary>
        public decimal TotalCapacity { get; set; }
        /// <summary>
        /// 占用时长
        /// </summary>
        public decimal OccupyHours { get; set; }
        /// <summary>
        /// 任务数
        /// </summary>
        public int TaskCount { get; set; }
        public int[] TaskIds { get; set; }
        public List<EquipmentLoadInfo> DayDetails { get; set; } = new List<EquipmentLoadInfo>();
    }
}
