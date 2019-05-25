using Abp.Domain.Entities;
using Master.Entity;
using Master.MultiTenancy;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Master.MES
{
    /// <summary>
    /// 策略
    /// </summary>
    public class Tactic : BaseFullEntity, IPassivable,IMayHaveTenant
    {
        /// <summary>
        /// 策略名称
        /// </summary>
        public string TacticName { get; set; }
        /// <summary>
        /// 策略类型
        /// </summary>
        public TacticType TacticType { get; set; }
        /// <summary>
        /// 是否有效
        /// </summary>
        public bool IsActive { get; set; } = true;
        /// <summary>
        /// 报工策略信息
        /// </summary>
        [NotMapped]
        public RemindTacticInfo RemindTacticInfo
        {
            get
            {
                return this.GetData<RemindTacticInfo>("RemindTacticInfo");
            }
            set
            {
                this.SetData("RemindTacticInfo", value);
            }
        }

        public int? TenantId { get; set; }
        public virtual Tenant Tenant { get; set; }
    }
    /// <summary>
    /// 策略类型
    /// </summary>
    public enum TacticType
    {
        /// <summary>
        /// 报工提醒
        /// </summary>
        RemindTactic=1,
        /// <summary>
        /// 管理端提醒
        /// </summary>
        Host=2
    }
    /// <summary>
    /// 报工提醒策略信息
    /// </summary>
    public class RemindTacticInfo
    {
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
        #region 延期提醒
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
        #endregion
        /// <summary>
        /// 要接收提醒的模具编号，如果为空，则接收所有模具提醒
        /// </summary>
        public List<string> RemindProjectSNs { get; set; } = new List<string>();
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
