using Abp.Domain.Entities;
using Master.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Case
{
    /// <summary>
    /// 精加工
    /// </summary>
    public class CaseFine : BaseFullEntityWithTenant, IHaveStatus, IPassivable
    {
        public int CaseInitialId { get; set; }
        public virtual CaseInitial CaseInitial { get; set; }
        public string Status { get; set; }
        public bool IsActive { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string MediaPath { get; set; }
        public DateTime UserModifyTime { get; set; } = DateTime.Now;
    }
}
