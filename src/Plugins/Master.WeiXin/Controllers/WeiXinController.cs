using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Senparc.CO2NET.Extensions;
using Senparc.Weixin;
using Senparc.Weixin.HttpUtility;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.OAuth;
using Senparc.Weixin.MP.CommonAPIs;
using System.Net;
using Microsoft.AspNetCore.Http;
using Senparc.Weixin.Exceptions;
using Master.Configuration;
using Master.WeiXin;
using Master.Authentication;
using Abp.Domain.Repositories;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Master.Authentication.External;
using Abp.Runtime.Security;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Runtime.Caching;
using Abp.Runtime.Session;
using Master.WeiXin.Filters;
using Senparc.Weixin.MP.Entities;
using Senparc.Weixin.MP.Entities.Request;
using Senparc.CO2NET.HttpUtility;
using Senparc.CO2NET.Utilities;
using Master.WeiXin.MessageHandlers.CustomMessageHandler;
using Senparc.Weixin.MP.MessageHandlers;
using Master.MultiTenancy;

namespace Master.Controllers
{
    public class WeiXinController: WeiXinBaseController
    {
        
        public WeiXinConfiguration WeiXinConfiguration { get; set; }
        public UserManager UserManager { get; set; }
        public TokenAuthController TokenAuthController { get; set; }
        public IRepository<UserLogin,int> UserLoginRepository { get; set; }
        public TenantManager TenantManager { get; set; }
        public WeiXinAppService WeiXinAppService { get; set; }

        

        #region 消息接口
        /// <summary>
        /// 微信后台验证地址（使用Get），微信后台的“接口配置信息”的Url填写如：http://sdk.weixin.senparc.com/weixin
        /// </summary>
        [HttpGet]
        [ActionName("Index")]
        public ActionResult Get(PostModel postModel, string echostr)
        {
            if (CheckSignature.Check(postModel.Signature, postModel.Timestamp, postModel.Nonce, token))
            {
                return Content(echostr); //返回随机字符串则表示验证通过
            }
            else
            {
                return Content("消息接口回调页:" + postModel.Signature + "," + Senparc.Weixin.MP.CheckSignature.GetSignature(postModel.Timestamp, postModel.Nonce, token) + "。" );
            }
        }

        /// <summary>
        /// 用户发送消息后，微信平台自动Post一个请求到这里，并等待响应XML。
        /// PS：此方法为简化方法，效果与OldPost一致。
        /// v0.8之后的版本可以结合Senparc.Weixin.MP.MvcExtension扩展包，使用WeixinResult，见MiniPost方法。
        /// </summary>
        [HttpPost]
        [ActionName("Index")]
        public ActionResult Post(PostModel postModel)
        {
            if (!CheckSignature.Check(postModel.Signature, postModel.Timestamp, postModel.Nonce, token))
            {
                return Content("参数错误！");
            }

            #region 打包 PostModel 信息

            postModel.Token = token;//根据自己后台的设置保持一致
            postModel.EncodingAESKey = encodingAESKey;//根据自己后台的设置保持一致
            postModel.AppId = appId;//根据自己后台的设置保持一致

            #endregion

            //v4.2.2之后的版本，可以设置每个人上下文消息储存的最大数量，防止内存占用过多，如果该参数小于等于0，则不限制
            var maxRecordCount = 10;

            //自定义MessageHandler，对微信请求的详细判断操作都在这里面。
            MessageHandler<CustomMessageContext> messageHandler;
            var messageHandlerType = WeiXinConfiguration.WeiXinMessageHandlerType;
            if (messageHandlerType != null)
            {
                messageHandler = Activator.CreateInstance(messageHandlerType, Request.GetRequestMemoryStream(), postModel, maxRecordCount) as MessageHandler<CustomMessageContext>;
            }
            else{
                messageHandler = new CustomMessageHandler(Request.GetRequestMemoryStream(), postModel, maxRecordCount);
            }


            #region 设置消息去重

            /* 如果需要添加消息去重功能，只需打开OmitRepeatedMessage功能，SDK会自动处理。
             * 收到重复消息通常是因为微信服务器没有及时收到响应，会持续发送2-5条不等的相同内容的RequestMessage*/
            messageHandler.OmitRepeatedMessage = true;//默认已经开启，此处仅作为演示，也可以设置为false在本次请求中停用此功能

            #endregion

            try
            {
                #region 记录 Request 日志
                //var logPath = Server.GetMapPath(string.Format("~/App_Data/MP/{0}/", DateTime.Now.ToString("yyyy-MM-dd")));
                //if (!Directory.Exists(logPath))
                //{
                //    Directory.CreateDirectory(logPath);
                //}

                ////测试时可开启此记录，帮助跟踪数据，使用前请确保App_Data文件夹存在，且有读写权限。
                //messageHandler.RequestDocument.Save(Path.Combine(logPath, string.Format("{0}_Request_{1}_{2}.txt", _getRandomFileName(),
                //    messageHandler.RequestMessage.FromUserName,
                //    messageHandler.RequestMessage.MsgType)));
                //if (messageHandler.UsingEcryptMessage)
                //{
                //    messageHandler.EcryptRequestDocument.Save(Path.Combine(logPath, string.Format("{0}_Request_Ecrypt_{1}_{2}.txt", _getRandomFileName(),
                //        messageHandler.RequestMessage.FromUserName,
                //        messageHandler.RequestMessage.MsgType)));
                //}
                Logger.Info($"WeChat Request Message:{messageHandler.RequestMessage.FromUserName},{messageHandler.RequestMessage.MsgType}");

                #endregion

                //执行微信处理过程
                messageHandler.Execute();

                #region 记录 Response 日志

                //测试时可开启，帮助跟踪数据

                //if (messageHandler.ResponseDocument == null)
                //{
                //    throw new Exception(messageHandler.RequestDocument.ToString());
                //}
                //if (messageHandler.ResponseDocument != null)
                //{
                //    messageHandler.ResponseDocument.Save(Path.Combine(logPath, string.Format("{0}_Response_{1}_{2}.txt", _getRandomFileName(),
                //        messageHandler.ResponseMessage.ToUserName,
                //        messageHandler.ResponseMessage.MsgType)));
                //}

                //if (messageHandler.UsingEcryptMessage && messageHandler.FinalResponseDocument != null)
                //{
                //    记录加密后的响应信息
                //    messageHandler.FinalResponseDocument.Save(Path.Combine(logPath, string.Format("{0}_Response_Final_{1}_{2}.txt", _getRandomFileName(),
                //        messageHandler.ResponseMessage.ToUserName,
                //        messageHandler.ResponseMessage.MsgType)));
                //}
                Logger.Info($"WeChat Response Message:{messageHandler.ResponseMessage.ToUserName},{messageHandler.ResponseMessage.MsgType}");
                #endregion

                return Content(messageHandler.ResponseDocument.ToString());//v0.7-
                //return new WeixinResult(messageHandler);//v0.8+
                //return new FixWeixinBugWeixinResult(messageHandler);//为了解决官方微信5.0软件换行bug暂时添加的方法，平时用下面一个方法即可
            }
            catch (Exception ex)
            {
                #region 异常处理
                WeixinTrace.Log("MessageHandler错误：{0}", ex.Message);
                Logger.Error("MessageHandler错误:" + ex.Message, ex);

                return Content("");
                #endregion
            }
        }

        ///// <summary>
        ///// 最简化的处理流程（不加密）
        ///// </summary>
        //[HttpPost]
        //[ActionName("MiniPost")]
        //public ActionResult MiniPost(PostModel postModel)
        //{
        //    if (!CheckSignature.Check(postModel.Signature, postModel.Timestamp, postModel.Nonce, token))
        //    {
        //        //return Content("参数错误！");//v0.7-
        //        return new WeixinResult("参数错误！");//v0.8+
        //    }

        //    postModel.Token = token;
        //    postModel.EncodingAESKey = encodingAESKey;//根据自己后台的设置保持一致
        //    postModel.AppId = AppId;//根据自己后台的设置保持一致

        //    var messageHandler = new CustomMessageHandler(Request.GetRequestMemoryStream(), postModel, 10);

        //    messageHandler.Execute();//执行微信处理过程

        //    //return Content(messageHandler.ResponseDocument.ToString());//v0.7-
        //    //return new WeixinResult(messageHandler);//v0.8+
        //    return new FixWeixinBugWeixinResult(messageHandler);//v0.8+
        //}

       
        #endregion

        #region OAuth
        public ActionResult OAuth(string returnUrl)
        {

            var state = "TiansFather-" + DateTime.Now.Millisecond;//随机数，用于识别请求可靠性
            HttpContext.Session.SetString("State", state);//储存随机数到Session

            var redirectUrl = $"{(Request.Scheme)}://{Request.Host}/Weixin/UserInfoCallback?returnUrl={returnUrl.UrlEncode()}";
            var oauthUrl = OAuthApi.GetAuthorizeUrl(appId,
                redirectUrl,
                state, OAuthScope.snsapi_userinfo);
            if (!string.IsNullOrEmpty(WeiXinConfiguration.OAuthBaseUrl))
            {
                //通过使用能被授权的域名进行跳转来实现多个授权域名
                oauthUrl = $"{WeiXinConfiguration.OAuthBaseUrl}?appid={appId}&scope=snsapi_userinfo&state={state}&redirect_uri={redirectUrl}";
            }


            return Redirect(oauthUrl);
        }

        /// <summary>
        /// OAuthScope.snsapi_userinfo方式回调
        /// </summary>
        /// <param name="code"></param>
        /// <param name="state"></param>
        /// <param name="returnUrl">用户最初尝试进入的页面</param>
        /// <returns></returns>
        public ActionResult UserInfoCallback(string code, string state, string returnUrl)
        {
            if (string.IsNullOrEmpty(code))
            {
                return Content("您拒绝了授权！");
            }

            if (state != HttpContext.Session.GetString("State"))
            {
                //这里的state其实是会暴露给客户端的，验证能力很弱，这里只是演示一下，
                //建议用完之后就清空，将其一次性使用
                //实际上可以存任何想传递的数据，比如用户ID，并且需要结合例如下面的Session["OAuthAccessToken"]进行验证
                return Content("验证失败！请从正规途径进入！");
            }

            OAuthAccessTokenResult result = null;

            //通过，用code换取access_token
            try
            {
                result = OAuthApi.GetAccessToken(appId, appSecret, code);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
            if (result.errcode != ReturnCode.请求成功)
            {
                return Content("错误：" + result.errmsg);
            }
            //下面2个数据也可以自己封装成一个类，储存在数据库中（建议结合缓存）
            //如果可以确保安全，可以将access_token存入用户的cookie中，每一个人的access_token是不一样的
            HttpContext.Session.SetString("OAuthAccessTokenStartTime", DateTime.Now.ToString());
            HttpContext.Session.SetString("OAuthAccessToken", result.ToJson());

            //因为第一步选择的是OAuthScope.snsapi_userinfo，这里可以进一步获取用户详细信息
            try
            {

                OAuthUserInfo userInfo = OAuthApi.GetUserInfo(result.access_token, result.openid);
                HttpContext.Session.Set("weuser", userInfo);

                //获取用户信息
                var user = UserManager.FindAsync(new Microsoft.AspNetCore.Identity.UserLoginInfo(WeChatAuthProviderApi.Name, userInfo.openid, "")).Result;
                if (user != null)
                {
                    //生成token返回给客户端
                    var authenticateResult = TokenAuthController.ExternalAuthenticate(new Models.TokenAuth.ExternalAuthenticateModel() { AuthProvider = WeChatAuthProviderApi.Name, ProviderKey = userInfo.openid }).Result;
                    HttpContext.Response.Cookies.Append("token", authenticateResult.EncryptedAccessToken, new Microsoft.AspNetCore.Http.CookieOptions());
                }
                else
                {
                    HttpContext.Response.Cookies.Delete("token");
                }

                if (!string.IsNullOrEmpty(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                return Content(userInfo.nickname);
            }
            catch (ErrorJsonResultException ex)
            {
                return Content(ex.Message);
            }
        }

        /// <summary>
        /// OAuthScope.snsapi_base方式回调
        /// </summary>
        /// <param name="code"></param>
        /// <param name="state"></param>
        /// <param name="returnUrl">用户最初尝试进入的页面</param>
        /// <returns></returns>
        public ActionResult BaseCallback(string code, string state, string returnUrl)
        {
            if (string.IsNullOrEmpty(code))
            {
                return Content("您拒绝了授权！");
            }

            if (state != HttpContext.Session.GetString("State"))
            {
                //这里的state其实是会暴露给客户端的，验证能力很弱，这里只是演示一下，
                //建议用完之后就清空，将其一次性使用
                //实际上可以存任何想传递的数据，比如用户ID，并且需要结合例如下面的Session["OAuthAccessToken"]进行验证
                return Content("验证失败！请从正规途径进入！");
            }

            //通过，用code换取access_token
            var result = OAuthApi.GetAccessToken(appId, appSecret, code);
            if (result.errcode != ReturnCode.请求成功)
            {
                return Content("错误：" + result.errmsg);
            }

            //下面2个数据也可以自己封装成一个类，储存在数据库中（建议结合缓存）
            //如果可以确保安全，可以将access_token存入用户的cookie中，每一个人的access_token是不一样的
            HttpContext.Session.SetString("OAuthAccessTokenStartTime", DateTime.Now.ToString());
            HttpContext.Session.SetString("OAuthAccessToken", result.ToJson());

            //因为这里还不确定用户是否关注本微信，所以只能试探性地获取一下
            OAuthUserInfo userInfo = null;
            try
            {
                //已关注，可以得到详细信息
                userInfo = OAuthApi.GetUserInfo(result.access_token, result.openid);
                //var info = UserApi.Info(appId, result.openid);
                //if (info.subscribe == 0)
                //{
                //    return Redirect("/WeiXin/GuanZhu");
                //}
                HttpContext.Session.Set("weuser", userInfo);
                //获取用户信息
                var user = UserManager.FindAsync(new Microsoft.AspNetCore.Identity.UserLoginInfo(WeChatAuthProviderApi.Name, userInfo.openid, "")).Result;
                if (user != null)
                {
                    //生成token返回给客户端
                    var authenticateResult = TokenAuthController.ExternalAuthenticate(new Models.TokenAuth.ExternalAuthenticateModel() { AuthProvider = WeChatAuthProviderApi.Name, ProviderKey = userInfo.openid }).Result;
                    HttpContext.Response.Cookies.Append("token", authenticateResult.EncryptedAccessToken, new Microsoft.AspNetCore.Http.CookieOptions());
                }
                else
                {
                    HttpContext.Response.Cookies.Delete("token");
                }
                if (!string.IsNullOrEmpty(returnUrl))
                {
                    return Redirect(returnUrl);
                }


                ViewData["ByBase"] = true;
                return View("UserInfoCallback", userInfo);
            }
            catch (ErrorJsonResultException ex)
            {
                //未关注，只能授权，无法得到详细信息
                //跳转至关注页
                Logger.Error(ex.Message);
                return Redirect("/WeiXin/GuanZhu");
                //这里的 ex.JsonResult 可能为："{\"errcode\":40003,\"errmsg\":\"invalid openid\"}"
                return Content("用户已授权，授权Token：" + result);
            }
        }
        #endregion

        #region 账套内员工扫码注册
        [WeUserFilter]
        [WeMustSubscribeFilter]
        public async Task<IActionResult> Register(int tenantId)
        {
            //先判断当前微信用户是否已经绑定了系统用户
            var user = await UserManager.FindAsync(new Microsoft.AspNetCore.Identity.UserLoginInfo(WeChatAuthProviderApi.Name, WeUser.openid, ""));
            if (user != null)
            {
                if (user.ToBeVerified)
                {
                    return Redirect("/WeiXin/Error?msg=" + "当前微信已经提交用户信息,请耐心等待审核".UrlEncode());
                }
                else
                {
                    return Redirect("/WeiXin/Error?msg=" + "当前微信用户已经绑定用户信息".UrlEncode());
                }

            }
            var tenant = await TenantManager.GetByIdFromCacheAsync(tenantId);
            ViewBag.TenantId = tenantId;
            ViewBag.Name = tenant.Name;
            ViewBag.Logo = tenant.Logo;
            ViewBag.OpenId = WeUser.openid;
            return View();
            
        }
        #endregion

        #region 扫码登录
        /// <summary>
        /// 扫码绑定微信至账号二维码页
        /// </summary>
        /// <returns></returns>
        [AbpMvcAuthorize]
        public ActionResult BindLogin(long? userId)
        {

            var encryptedUserId = SimpleStringCipher.Instance.Encrypt((userId.HasValue ? userId.Value : AbpSession.UserId.Value).ToString(), AppConsts.DefaultPassPhrase);
            ViewBag.UserId = encryptedUserId;
            return View();
        }
        [WeUserFilter]
        [WeMustSubscribeFilter]
        public async Task<ActionResult> BindLoginCallback(string userid)
        {
            //判断当前微信账号是否已经绑定了用户
            var count = await UserLoginRepository.GetAll().IgnoreQueryFilters()
                .CountAsync(o => o.LoginProvider == WeChatAuthProviderApi.Name && o.ProviderKey == WeUser.openid);

            if (count > 0)
            {
                return Redirect("/WeiXin/Error?msg=" + "当前微信已经绑定了账号".UrlEncode());
            }
            ViewBag.UserId = userid;
            return View();
        }
        /// <summary>
        /// 直接绑定，不需要用户确认，为了解决iphone低级别手机无法点击按钮问题
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        [WeUserFilter]
        [WeMustSubscribeFilter]
        public async Task<ActionResult> BindLoginCallbackDirect(string userid)
        {
            //判断当前微信账号是否已经绑定了用户
            var count = await UserLoginRepository.GetAll().IgnoreQueryFilters()
                .CountAsync(o => o.LoginProvider == WeChatAuthProviderApi.Name && o.ProviderKey == WeUser.openid);
            if (count > 0)
            {
                return Redirect("/WeiXin/Error?msg=" + "当前微信已经绑定了账号".UrlEncode());
            }

            await WeiXinAppService.BindLogin(userid);
            return Redirect("/WeiXin/Success?msg=" + "绑定成功".UrlEncode());
        }
        /// <summary>
        /// 扫码登录页
        /// </summary>
        /// <returns></returns>
        public ActionResult Login()
        {
            //生成一个标识存入Ｓｅｓｓｉｏｎ
            var guid = Guid.NewGuid();
            HttpContext.Session.Set("WeChatLoginId", guid);

            ViewBag.Guid = guid.ToString();
            return View();
        }
        [WeUserFilter]
        public async Task<ActionResult> LoginCallback(string guid)
        {
            //判断当前微信账号是否已经绑定用户
            var userLogin = await UserLoginRepository.GetAll().IgnoreQueryFilters()
                .Where(o => o.LoginProvider == WeChatAuthProviderApi.Name && o.ProviderKey == WeUser.openid)
                .FirstOrDefaultAsync();

            //if (userLogin == null)
            //{
            //    return Redirect("/MES/BindError");
            //    return Redirect("/WeiXin/Error?msg=" + "当前微信尚未绑定账号".UrlEncode());
            //}
            ViewBag.Guid = guid;
            return View();
        }

        #endregion

        #region 扫码上传
        /// <summary>
        /// 扫码上传
        /// </summary>
        /// <returns></returns>
        public ActionResult ScanUpload()
        {
            //生成一个标识存入Ｓｅｓｓｉｏｎ
            var guid = Guid.NewGuid();
            HttpContext.Session.Set("WeChatUploadId", guid);

            ViewBag.Guid = guid.ToString();
            return View();
        }

        [WeUserFilter]
        public async Task<ActionResult> ScanUploadCallback(string guid)
        {

            ViewBag.Guid = guid;
            return View();
        }
        #endregion

        #region 扫码获取用户信息
        /// <summary>
        /// 扫码获取用户信息
        /// </summary>
        /// <returns></returns>
        public ActionResult ScanUserInfo()
        {
            //生成一个标识存入Ｓｅｓｓｉｏｎ
            var guid = Guid.NewGuid();
            HttpContext.Session.Set("WeChatUserInfoId", guid);

            ViewBag.Guid = guid.ToString();
            return View();
        }

        [WeUserFilter]
        [WeBindUserFilter]
        public async Task<ActionResult> ScanUserInfoCallback(string guid)
        {

            ViewBag.Guid = guid;
            return View();
        }
        #endregion

        /// <summary>
        /// 关注页
        /// </summary>
        /// <returns></returns>
        public ActionResult GuanZhu()
        {
            return View();
        }
        public ActionResult UserInfo(string openId)
        {
            var info = UserApi.Info(appId, openId);
            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(info));
        }

        [WeUserFilter]
        public ActionResult LoginInfo()
        {
            return Json(AbpSession.ToUserIdentifier());
        }


    }
}
