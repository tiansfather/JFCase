using Microsoft.AspNetCore.Http;
using Senparc.CO2NET.Extensions;
using Senparc.Weixin;
using Senparc.Weixin.MP.AdvancedAPIs.OAuth;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Controllers
{
    public class WeiXinBaseController: MasterControllerBase
    {
        //下面换成账号对应的信息，也可以放入web.config等地方方便配置和更换
        protected readonly string appId = Config.SenparcWeixinSetting.WeixinAppId;//与微信公众账号后台的AppId设置保持一致，区分大小写。
        protected readonly string appSecret = Config.SenparcWeixinSetting.WeixinAppSecret;//与微信公众账号后台的AppId设置保持一致，区分大小写。
        protected readonly string token = Config.SenparcWeixinSetting.Token;//与微信公众账号后台的Token设置保持一致，区分大小写。
        protected readonly string encodingAESKey = Config.SenparcWeixinSetting.EncodingAESKey;//与微信公众账号后台的EncodingAESKey设置保持一致，区分大小写。

        public OAuthUserInfo WeUser
        {
            get
            {
                return HttpContext.Session.Get<OAuthUserInfo>("weuser");
            }
        }
    }
}
