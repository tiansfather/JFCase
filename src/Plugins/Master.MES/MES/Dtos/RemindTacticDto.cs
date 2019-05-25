using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.MES.Dtos
{
    /// <summary>
    /// 提醒策略Dto
    /// </summary>
    [AutoMap(typeof(RemindTacticInfo))]
    public class RemindTacticDto
    {
        public int Id { get; set; }
        public string TacticName { get; set; }
        /// <summary>
        /// 是否接收到料提醒
        /// </summary>
        public bool EnableReceiveRemind { get; set; }
        /// <summary>
        /// 是否接收上机提醒
        /// </summary>
        public bool EnableStartRemind { get; set; }
        /// <summary>
        /// 是否接收下机提醒
        /// </summary>
        public bool EnableEndRemind { get; set; }
        /// <summary>
        /// 是否接收暂停提醒
        /// </summary>
        public bool EnableSuspendRemind { get; set; }
        /// <summary>
        /// 是否接收加工中提醒
        /// </summary>
        public bool EnableProcessingRemind { get; set; }
        /// <summary>
        /// 提醒开单人
        /// </summary>
        public bool DynamicRemindPoster { get; set; }
        /// <summary>
        /// 提醒客户
        /// </summary>
        public bool DynamicRemindCustomer { get; set; }
        /// <summary>
        /// 提醒模具组长
        /// </summary>
        public bool DynamicRemindProjectCharger { get; set; }
        /// <summary>
        /// 提醒项目负责
        /// </summary>
        public bool DynamicRemindProjectTracker { get; set; }
        /// <summary>
        /// 提醒工艺师
        /// </summary>
        public bool DynamicRemindCraftsMan { get; set; }
        /// <summary>
        /// 提醒审核
        /// </summary>
        public bool DynamicRemindVerifier { get; set; }
        /// <summary>
        /// 提醒检验
        /// </summary>
        public bool DynamicRemindChecker { get; set; }
        /// <summary>
        /// 要接收提醒的模具编号，如果为空，则接收所有模具提醒
        /// </summary>
        public List<string> RemindProjectSNs { get; set; }
        /// <summary>
        /// 延期上机提醒
        /// </summary>
        public bool EnableStartDelayRemind { get; set; }
        /// <summary>
        /// 延期几天未上机
        /// </summary>
        public int StartDelayOffsetDay { get; set; } = 3;
        /// <summary>
        /// 延期下机提醒
        /// </summary>
        public bool EnableEndDelayRemind { get; set; }
        /// <summary>
        /// 延期几天未下机
        /// </summary>
        public int EndDelayOffsetDay { get; set; } = 3;
        /// <summary>
        /// 到料未上机提醒
        /// </summary>
        public bool EnableReceiveNotStartRemind { get; set; }
        /// <summary>
        /// 到料几天未上机
        /// </summary>
        public int ReceiveNotStartOffsetDay { get; set; } = 3;
        /// <summary>
        /// 工时超预计提醒
        /// </summary>
        public bool EnableExceedHourRemind { get; set; }
        /// <summary>
        /// 工时超过几小时
        /// </summary>
        public int ExceedHourOffsetHour { get; set; } = 0;
        /// <summary>
        /// 起始提醒时间
        /// </summary>
        public int RemindStartHour { get; set; }
        /// <summary>
        /// 结束提醒时间
        /// </summary>
        public int RemindEndHour { get; set; }
    }
}
