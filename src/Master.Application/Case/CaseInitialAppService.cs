using Abp.Authorization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Case
{
    [AbpAuthorize]
    public class CaseInitialAppService : ModuleDataAppServiceBase<CaseInitial, int>
    {
        protected override string ModuleKey()
        {
            return nameof(CaseInitial);
        }
    }
}
