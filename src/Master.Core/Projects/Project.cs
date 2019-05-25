using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Abp.Domain.Entities;
using Master.Entity;
using Master.Module.Attributes;
using Master.Orders;
using Master.Units;

namespace Master.Projects
{
    [InterModule("项目列表")]
    public class Project : BaseFullEntityWithTenant, IHaveStatus, IPassivable
    {
        public string Status { get; set; }
        public virtual Order Order { get; set; }
        public int? OrderId { get; set; }
        public virtual Unit Unit { get; set; }
        [InterColumn(ColumnName ="客户",ColumnType =Module.ColumnTypes.Reference,RelativeDataType =Module.RelativeDataType.Module,RelativeDataString =nameof(Unit),MaxReferenceNumber ="1",ReferenceItemTpl ="unitName",ReferenceSearchColumns ="unitName", ReferenceSearchWhere= "{\"where\":\"UnitNature = 1 or UnitNature = 3\"}", Sort =-1,DisplayPath ="Unit.UnitName",Templet ="{{d.unitId_display==null?'':d.unitId_display}}")]
        public int? UnitId { get; set; }
        /// <summary>
        /// 项目编号
        /// </summary>
        [InterColumn(ColumnName ="项目编号",Sort =0,VerifyRules ="required")]
        public string ProjectSN { get; set; }
        /// <summary>
        /// 客户项目编号
        /// </summary>
        [InterColumn(ColumnName ="客户项目编号",Sort =1)]
        public string CustomerProjectSN { get; set; }
        public string Discriminator { get; set; } = nameof(Project);
        /// <summary>
        /// 项目类型
        /// </summary>
        [InterColumn(ColumnName ="项目类别",ColumnType =Module.ColumnTypes.Select,DictionaryName =StaticDictionaryNames.ProjectType)]
        public string ProjectType { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        [InterColumn(ColumnName = "项目名称",Sort =1)]
        public string ProjectName { get; set; }
        [NotMapped]
        [InterColumn(ColumnName = "项目图片", ColumnType = Module.ColumnTypes.Images, MaxFileNumber = 1, ValuePath = "Property", Sort = 2)]
        public int? ProjectPic
        {
            get
            {
                return this.GetPropertyValue<int?>("ProjectPic");
            }
            set
            {
                this.SetPropertyValue("ProjectPic", value);
            }
        }
        [InterColumn(ColumnName = "数量", ColumnType = Module.ColumnTypes.Number, Sort = 3, VerifyRules = "required", DefaultValue = "1")]
        public int Number { get; set; } = 1;
        [InterColumn(ColumnName = "下单日期", ColumnType = Module.ColumnTypes.DateTime, Sort = 4)]
        public DateTime? OrderDate { get; set; }
        [InterColumn(ColumnName ="要求完成日期",ColumnType =Module.ColumnTypes.DateTime,Sort =5)]
        public DateTime? RequireDate { get; set; }
        [InterColumn(ColumnName = "启用", ColumnType = Module.ColumnTypes.Switch, IsShownInAdd = false, IsShownInEdit = false, IsShownInView = false,IsShownInList =false,Sort =10)]
        public virtual bool IsActive { get; set; } = true;
    }
}
