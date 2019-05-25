using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Master.MES
{
    public class TacticPerson : CreationAuditedEntity<int>
    {
        public int TacticId { get; set; }
        public virtual Tactic Tactic { get; set; }
        public int PersonId { get; set; }
        public virtual Person Person { get; set; }
    }

    /// <summary>
    /// 用于提醒的人员及对应的有效策略信息
    /// </summary>
    public class PersonTacticInfo
    {
        public Person Person { get; set; }
        public List<Tactic> Tactics { get; set; }
        /// <summary>
        /// 获取到最近可用的提醒时间的间隔
        /// 如最近可用的提醒时间段是8-18,当前是3点，则返回5
        /// </summary>
        /// <returns></returns>
        public TimeSpan GetAvailableRemindTimeSpan()
        {
            var nowHour = DateTime.Now.Hour;
            //如果当前时间有在对应的策略的提醒时间内，立即执行
            if(Tactics.Exists(o=>o.RemindTacticInfo.RemindStartHour==0 && o.RemindTacticInfo.RemindEndHour==0 ||   o.RemindTacticInfo.RemindStartHour<=nowHour && o.RemindTacticInfo.RemindEndHour > nowHour))
            {
                return TimeSpan.FromSeconds(0);
            }
            else
            {
                //寻找距离当前时间最近的提醒时间间隔
                return Tactics.Select(o => o.RemindTacticInfo.RemindStartHour > nowHour ? (DateTime.Parse($"{DateTime.Now.ToString("yyyy-MM-dd")} {o.RemindTacticInfo.RemindStartHour}:00:00") - DateTime.Now) : (DateTime.Parse($"{DateTime.Now.ToString("yyyy-MM-dd")} {o.RemindTacticInfo.RemindStartHour}:00:00").AddDays(1) - DateTime.Now)).Min();
            }
        }
    }
}
