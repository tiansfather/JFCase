using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.MES.Dtos
{
    [AutoMap(typeof(Equipment))]
    public class EquipmentInfoDto
    {
        public int Id { get; set; }
        public string ProcessTypeName { get; set; }
        public string EquipmentSN { get; set; }
        public string Brand { get; set; }
        public string Range { get; set; }
        public decimal? Price { get; set; }
        public int? BuyYear { get; set; }
        public decimal? BuyCost { get; set; }
        /// <summary>
        /// 未完成任务数量
        /// </summary>
        public int TaskCount { get; set; }
    }
}
