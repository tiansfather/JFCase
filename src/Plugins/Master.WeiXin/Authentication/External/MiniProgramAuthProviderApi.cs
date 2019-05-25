using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Master.Authentication.External
{
    public class MiniProgramAuthProviderApi : ExternalAuthProviderApiBase
    {
        public const string Name = "MiniProgram";
        public override Task<ExternalAuthUserInfo> GetUserInfo(string accessCode)
        {
            return null;
        }
    }
}
