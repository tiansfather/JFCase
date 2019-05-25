using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.MES.Dtos
{
    /// <summary>
    /// 设备的现场信息
    /// </summary>
    [AutoMap(typeof(Equipment))]
    public class EquipmentProcessInfoDto
    {
        public int Id { get; set; }
        public string ProcessTypeName { get; set; }
        public string EquipmentSN { get; set; }
        public int? EquipmentPic { get; set; }
        public int? TaskId { get; set; }
        public string ProjectSN { get; set; }
        public string PartSN { get; set; }
        public string PartName { get; set; }
        public string Operator { get; set; }
        public ProcessTaskProgressInfo ProcessTaskProgressInfo { get; set; }
        public int TaskNumber { get; set; }
        public IEnumerable<ProcessTaskViewDto> Tasks { get; set; } = new List<ProcessTaskViewDto>();
        public EquipmentLoadInfo EquipmentLoadInfo { get; set; }
    }
}
