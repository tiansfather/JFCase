using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Master.Authentication.External
{
    public class WeChatAuthProviderApi : ExternalAuthProviderApiBase
    {
        public const string Name = "Wechat";
        public override async  Task<ExternalAuthUserInfo> GetUserInfo(string accessCode)
        {
            //todo:微信登录
            return null;
        }
    }
}
