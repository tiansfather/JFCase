using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Module
{
    public class ModuleDataContext
    {
        public IDictionary<string, object> Entity { get; set; }
        public ModuleInfo ModuleInfo { get; set; }
        public IModuleInfoManager ModuleInfoManager { get; set; }
    }
}
