using Master.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Module
{
    public class ModuleData : BaseFullEntity
    {
        public int ModuleInfoId { get; set; }
        public virtual ModuleInfo ModuleInfo { get; set; }
    }
}
