using Abp.Configuration.Startup;
using Master.WeiXin.MessageHandlers.CustomMessageHandler;
using Senparc.Weixin.MP.MessageHandlers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Configuration
{
    public class WeiXinConfiguration
    {
        /// <summary>
        /// 授权回调Url，当授权域名不够用时可使用被授权域名网站做跳转
        /// </summary>
        public string OAuthBaseUrl = "";
        public Type WeiXinMessageHandlerType = null;//公众号消息处理类
        //public Dictionary<string, string> WeiXinTemplateIdDic = new Dictionary<string, string>();//模板消息Id
    }

    public static class WeiXinConfigurationExtension
    {
        public static WeiXinConfiguration WeiXin(this IModuleConfigurations moduleConfigurations)
        {
            return moduleConfigurations.AbpConfiguration.Get<WeiXinConfiguration>();
        }
    }
}
