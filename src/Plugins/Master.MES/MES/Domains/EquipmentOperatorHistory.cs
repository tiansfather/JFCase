using Master.Authentication;
using Master.Entity;
using Master.Module.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Master.MES
{
    /// <summary>
    /// 交接班记录
    /// </summary>
    [InterModule("交接班记录")]
    public class EquipmentOperatorHistory:BaseFullEntityWithTenant
    {
        [InterColumn(ColumnName ="设备名称",ValuePath ="Equipment.EquipmentSN")]
        public int EquipmentId { get; set; }
        public virtual Equipment Equipment { get; set; }
        [InterColumn(ColumnName = "人员", ValuePath = "Operator.Name")]
        public long OperatorId { get; set; }
        [ForeignKey("OperatorId")]
        public virtual User Operator { get; set; }
        [InterColumn(ColumnName = "交接类型",Templet = "{{d.equipmentTransitionType==1?'绑定':'解绑'}}")]
        public EquipmentTransitionType EquipmentTransitionType { get; set; }

    }

    public enum EquipmentTransitionType
    {
        In=1,
        Out=2
    }
}
