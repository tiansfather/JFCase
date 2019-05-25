using Abp.Dependency;
using Microsoft.AspNetCore.Http;
using Senparc.Weixin;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.OAuth;
using Senparc.Weixin.MP.AdvancedAPIs.TemplateMessage;
using Senparc.Weixin.MP.AdvancedAPIs.User;
using Senparc.Weixin.MP.Containers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Master.WeiXin
{
    public  class WeiXinHelper
    {
        /// <summary>
        /// 通过openId获取用户信息
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        public static UserInfoJson GetUserInfo(string openId)
        {
            return UserApi.Info(Config.SenparcWeixinSetting.WeixinAppId, openId);
        }
        /// <summary>
        /// 获取当前微信用户信息
        /// </summary>
        /// <returns></returns>
        public static OAuthUserInfo GetWeiXinUserInfo()
        {
            using (var contextAccessorWrapper = IocManager.Instance.ResolveAsDisposable(typeof(IHttpContextAccessor)))
            {
                return (contextAccessorWrapper.Object as IHttpContextAccessor).HttpContext.Session.Get<OAuthUserInfo>("weuser");
            }
        }

        public static async Task<string> DownloadMedia(string mediaId, string dir)
        {
            var appId = Config.SenparcWeixinSetting.WeixinAppId;
            //var token = await AccessTokenContainer.GetAccessTokenAsync(appId);
            //var fileUrl = $"http://file.api.weixin.qq.com/cgi-bin/media/get?access_token={token}&media_id={mediaId}";
            //var fileName = $"{dir}\\{mediaId}.jpg";
            //System.IO.Directory.CreateDirectory(dir);
            //using(var wc=new System.Net.WebClient())
            //{
            //    wc.DownloadFile(new Uri(fileUrl), fileName);
            //}
            //return fileName;
            //return MediaApi.Get(appId, mediaId, dir);
            //todo:
            //使用原方法时报httpclient 的A task was canceled.错误,当移值新服务器时出现,但旧服务器没有任何问题,猜测服务器对httpclient请求有限制
            return await MediaApi.GetAsync(appId, mediaId, dir);

        }
        /// <summary>
        /// 发送模板消息
        /// </summary>
        /// <param name="openId"></param>
        /// <param name="templateId"></param>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static SendTemplateMessageResult SendTemplateMessage(string openId, string templateId, string url, object data)
        {
            var appId = Config.SenparcWeixinSetting.WeixinAppId;
            return TemplateApi.SendTemplateMessage(appId, openId, templateId, url, data);
        }
    }
}
