using Abp.Events.Bus;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Module
{
    public class ModuleInfoChangedEventData : EventData
    {
        public string ModuleKey { get; set; }
        public int TenantId { get; set; }
    }
}
