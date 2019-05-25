using Abp.Dependency;
using Abp.Runtime.Session;
using Master.Authentication;
using Master.Authentication.External;
using Master.Controllers;
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
    /// 获取用户系统账号过滤器
    /// </summary>
    public class WeBindUserFilter : Attribute, IActionFilter
    {
        //是否强制绑定系统账号,若设为是，则未绑定时则跳转至错误页
        private bool _mustBind = true;

        public WeBindUserFilter(bool mustBind = true)
        {
            _mustBind = mustBind;
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {            
            var weuser= context.HttpContext.Session.Get<OAuthUserInfo>("weuser");
            using(var scope = IocManager.Instance.CreateScope())
            {
                var userManager = scope.Resolve<UserManager>();
                var tokenAuthController = scope.Resolve<TokenAuthController>();
                var abpSession = scope.Resolve<IAbpSession>();
                //var claimPrincipalCreator = scope.Resolve<IClaimsPrincipalCreator>();
                try
                {
                    //获取用户信息
                    var user = userManager.FindAsync(new Microsoft.AspNetCore.Identity.UserLoginInfo(WeChatAuthProviderApi.Name, weuser.openid, "")).Result;
                    if (user != null)
                    {
                        var authenticateResult = tokenAuthController.ExternalAuthenticate(new Models.TokenAuth.ExternalAuthenticateModel() { AuthProvider = WeChatAuthProviderApi.Name, ProviderKey = weuser.openid ,ClientInfo="Wechat"}).Result;
                        //生成token返回给客户端
                        //context.HttpContext.User = claimPrincipalCreator.CreateClaimsPrincipal(user).Result;
                        context.HttpContext.Response.Cookies.Append("token", authenticateResult.EncryptedAccessToken, new Microsoft.AspNetCore.Http.CookieOptions());
                        //todo:优化？为了使antifogerytoken起作用，此处需要重新加载页面
                        if (!abpSession.UserId.HasValue || abpSession.UserId.Value!=user.Id)
                        {
                            //通过前台重新加载此页面,使antifogory生效
                            var redirectUrl = context.HttpContext.Request.AbsoluteUri();
                            //由于微信端跳转自身会失败,所以在后面加上参数
                            redirectUrl = redirectUrl.IndexOf('?') > 0 ? redirectUrl + "&_=1" : redirectUrl + "?_=1";
                            context.HttpContext.Response.ContentType = "text/html; charset=utf-8";
                            context.HttpContext.Response.WriteAsync($"<script>location.href='{redirectUrl}';</script>");
                        }
                    }
                    else
                    {
                        context.HttpContext.Response.Cookies.Delete("token");
                        if (_mustBind)
                        {
                            context.Result = new RedirectResult("/WeiXin/Error?msg=" + "当前微信尚未绑定系统账号".UrlEncode());
                        }

                        //context.Result = new RedirectResult("/MES/Register/?msg=" + "当前微信尚未绑定系统账号".UrlEncode());
                    }
                }
                catch (Exception ex)
                {
                    if (_mustBind)
                    {
                        context.Result = new RedirectResult("/WeiXin/Error?msg=" + ex.Message.UrlEncode());
                    }

                }
            }
            //using(var userManagerWrapper = IocManager.Instance.ResolveAsDisposable<UserManager>())
            //{
            //    using(var tokenAuthControllerWrapper = IocManager.Instance.ResolveAsDisposable<TokenAuthController>())
            //    {
            //        using(var abpsessionWrapper = IocManager.Instance.ResolveAsDisposable<IAbpSession>())
            //        {
                        
            //        }
                    
                    
            //        //using (var SessionWrapper = IocManager.Instance.ResolveAsDisposable<IAbpSession>())
            //        //{
            //        //    if (SessionWrapper.Object.ToUserIdentifier() == null)
            //        //    {
            //        //    }
            //        //}
            //    }
                
            //}
            
        }
    }
}
