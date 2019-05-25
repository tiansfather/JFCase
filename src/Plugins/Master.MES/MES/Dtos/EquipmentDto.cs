using Abp.AutoMapper;
using Master.Entity;
using Master.Resources;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.MES.Dtos
{
    /// <summary>
    /// 
    /// </summary>
    [AutoMap(typeof(Equipment))]
    public class EquipmentDto
    {
        public int Id { get; set; }
        public int? UnitId { get; set; }
        //public virtual Unit Unit { get; set; }
        public string Status { get; set; }
        /// <summary>
        /// 设备编号
        /// </summary>
        public string EquipmentSN { get; set; }
        /// <summary>
        /// 品牌
        /// </summary>
        public string Brand { get; set; }
        /// <summary>
        /// 日产能
        /// </summary>
        public decimal? DayCapacity { get; set; }
        /// <summary>
        /// 行程
        /// </summary>
        public string Range { get; set; }
        /// <summary>
        /// 加工单价
        /// </summary>
        public decimal? Price { get; set; }
        /// <summary>
        /// 购置年份
        /// </summary>
        public int? BuyYear { get; set; }
        /// <summary>
        /// 购置金额 
        /// </summary>
        public decimal? BuyCost { get; set; }
        public string EquipmentPicPath { get; set; }
        public long? ProgrammerId { get; set; } 

        public long? ArrangerId { get; set; } 
        public long? OperatorId { get; set; } 

        public bool IsActive { get; set; } = true;

        public List<int> TypeID { get; set; }

         
    }
}
