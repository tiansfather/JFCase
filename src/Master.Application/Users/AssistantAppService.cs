using Abp.Authorization;
using Master.Authentication;
using Master.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Users
{
    [AbpAuthorize]
    public class AssistantAppService : ModuleDataAppServiceBase<User, long>
    {
        protected override string ModuleKey()
        {
            return nameof(Assistant);
        }
    }
}
