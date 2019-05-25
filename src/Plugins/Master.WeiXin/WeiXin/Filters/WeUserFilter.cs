using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Senparc.CO2NET.Extensions;
using Senparc.Weixin.MP.AdvancedAPIs.OAuth;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.WeiXin.Filters
{
    /// <summary>
    /// 用于使用微信oauth获取用户openid
    /// </summary>
    public class WeUserFilterAttribute : Attribute,IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var url = context.HttpContext.Request.Path+context.HttpContext.Request.QueryString;
            var weuser = context.HttpContext.Session.Get<OAuthUserInfo>("weuser");
            if (weuser == null)
            {
                var oauthUrl = "/weixin/oauth?returnUrl=" + url.UrlEncode();
                context.Result = new RedirectResult(oauthUrl);
            }
        }
    }
}
