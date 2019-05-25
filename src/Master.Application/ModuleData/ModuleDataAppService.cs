using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Module
{
    public class ModuleDataAppService : ModuleDataAppServiceBase<ModuleData, int>
    {
        protected override string ModuleKey()
        {
            throw new NotImplementedException();
        }
    }
}
