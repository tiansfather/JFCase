using Abp.Domain.Entities;
using Master.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Case
{
    /// <summary>
    /// 案例卡
    /// </summary>
    public class CaseCard:BaseFullEntityWithTenant, IHaveStatus, IPassivable
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public int CaseInitialId { get; set; }
        public virtual CaseInitial CaseInitial { get; set; }
        public string Status { get; set; }
        public bool IsActive { get; set; }
    }
}
