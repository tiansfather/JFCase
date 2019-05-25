using Abp.Domain.Entities;
using Master.Entity;
using Master.MultiTenancy;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Notices
{
    public class Notice : BaseFullEntity, IMayHaveTenant,IPassivable
    {
        public int? TenantId { get; set; }
        public virtual Tenant Tenant { get; set; }
        public string NoticeTitle { get; set; }
        public string NoticeContent { get; set; }
        public bool IsActive { get; set; }
        /// <summary>
        /// 公告类型
        /// </summary>
        public string NoticeType { get; set; }
        /// <summary>
        /// 接收方账套Id
        /// </summary>
        public int? ToTenantId { get; set; }
    }
}
