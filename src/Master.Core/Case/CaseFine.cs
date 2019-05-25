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
        public string Status { get; set; }
        public bool IsActive { get; set; }
    }
}
