using Master.WeiXin.MessageHandlers.CustomMessageHandler;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Senparc.Weixin;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.OAuth;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.WeiXin.Filters
{
    /// <summary>
    /// 必须关注后才能访问
    /// </summary>
    public class WeMustSubscribeFilter : Attribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var weuser = context.HttpContext.Session.Get<OAuthUserInfo>("weuser");

            var info = UserApi.Info(Config.SenparcWeixinSetting.WeixinAppId, weuser.openid);
            if (info.subscribe == 0)
            {
                //将当前用户访问地址放入消息集合中,待关注后重新访问
                CustomMessageHandler.TemplateMessageCollection[info.openid] = context.HttpContext.Request.AbsoluteUri();
                context.Result = new RedirectResult("/WeiXin/GuanZhu");
            }
        }
    }
}
