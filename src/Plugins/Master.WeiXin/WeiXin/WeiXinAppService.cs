using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Runtime.Security;
using Abp.UI;
using Master.Authentication;
using Master.Authentication.External;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp;
using Abp.Runtime.Caching;
using Abp.Auditing;
using System.IO;
using Microsoft.AspNetCore.Identity;
using Senparc.Weixin.MP.AdvancedAPIs.OAuth;

namespace Master.WeiXin
{
    public class WeiXinAppService: MasterAppServiceBase
    {
        public IRepository<UserLogin,int > UserLoginRepository { get; set; }
        public UserManager UserManager { get; set; }
        public FileManager FileManager { get; set; }

        #region 扫码登录

        /// <summary>
        /// 微信端确认登录
        /// </summary>
        public virtual void Login(string guid)
        {
            var weuser = WeiXinHelper.GetWeiXinUserInfo();
            Logger.Info("Login2:" + guid + Newtonsoft.Json.JsonConvert.SerializeObject(weuser));
            //存入缓存
            CacheManager.GetCache<string,OAuthUserInfo>("ExternalLoginCache").Get(guid,()=>weuser);
            //using (var contextAccessorWrapper = IocManager.Instance.ResolveAsDisposable(typeof(IHttpContextAccessor)))
            //{

            //    var session = (contextAccessorWrapper.Object as IHttpContextAccessor).HttpContext.Session;
            //    var guid = session.Get<string>("WeChatLoginId");
            //    //将openid信息存入session
            //    session.Set(guid, weuser.openid);
            //}
        }
        /// <summary>
        /// 轮询获取微信登录的openid
        /// </summary>
        /// <returns></returns>
        [DisableAuditing]
        public virtual object GetLoginInfo(string guid)
        {
            var weuser = CacheManager.GetCache<string, OAuthUserInfo>("ExternalLoginCache").GetOrDefault(guid);


            return weuser;
        }

        #endregion

        #region 绑定登录

        /// <summary>
        /// 绑定微信至账号
        /// </summary>
        /// <param name="encrpytedUserId"></param>
        /// <returns></returns>
        public virtual async Task BindLogin(string encryptedUserId)
        {

            if(!int.TryParse( SimpleStringCipher.Instance.Decrypt(encryptedUserId, AppConsts.DefaultPassPhrase),out var userid)){
                throw new UserFriendlyException("未找到对应用户");
            }
            var weuser = WeiXinHelper.GetWeiXinUserInfo();
            var user = await UserManager.GetAll().IgnoreQueryFilters().Where(o=>o.Id==userid).SingleAsync();

            await UserManager.BindExternalLogin(user,new UserLoginInfo(WeChatAuthProviderApi.Name,weuser.openid,""));

            //await UserLoginRepository.DeleteAsync(o => o.UserId == userid && o.LoginProvider == WeChatAuthProviderApi.Name);
            //var userlogin = new UserLogin()
            //{
            //    UserId = userid,
            //    LoginProvider = WeChatAuthProviderApi.Name,
            //    TenantId = user.TenantId,
            //    ProviderKey = weuser.openid
            //};
            //await UserLoginRepository.InsertAsync(userlogin);
        }
        /// <summary>
        /// 轮询绑定微信登录状态
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        [DisableAuditing]
        public virtual async Task<bool> GetBindLoginInfo(string provider,string encryptedUserId)
        {
            var userId = int.Parse(SimpleStringCipher.Instance.Decrypt(encryptedUserId, AppConsts.DefaultPassPhrase));
            return await UserLoginRepository.CountAsync(o => o.UserId == userId && o.LoginProvider == provider)>0;
        }

        #endregion

        #region 扫码上传
        /// <summary>
        /// 将扫码上传的文件保存至缓存
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="uploadFileInfo"></param>
        public virtual void SetUploadInfo(string guid, UploadResult uploadResult)
        {
            CacheManager.GetCache<string, UploadResult>("ExternalUploadCache").Get(guid, () => uploadResult);
        }
        /// <summary>
        /// 轮询获取微信上传的文件
        /// </summary>
        /// <returns></returns>
        [DisableAuditing]
        public virtual UploadResult GetUploadInfo()
        {
            using (var contextAccessorWrapper = IocManager.Instance.ResolveAsDisposable(typeof(IHttpContextAccessor)))
            {
                var session = (contextAccessorWrapper.Object as IHttpContextAccessor).HttpContext.Session;
                var guid = session.Get<string>("WeChatUploadId");

                var result = CacheManager.GetCache<string, UploadResult>("ExternalUploadCache").GetOrDefault(guid);

                Logger.Info($"Getting:{guid}:{result?.ToString()}");

                return result;
            }
        }
        #endregion

        #region 扫码获取用户信息
       
        public virtual async Task SetUserInfo(string guid)
        {
            var user =await Resolve<UserManager>().GetByIdAsync(AbpSession.UserId.Value);
            CacheManager.GetCache<string, User>("ExternalUserInfoCache").Get(guid, () => user);
        }
        [DisableAuditing]
        public virtual User GetUserInfo()
        {
            using (var contextAccessorWrapper = IocManager.Instance.ResolveAsDisposable(typeof(IHttpContextAccessor)))
            {
                var session = (contextAccessorWrapper.Object as IHttpContextAccessor).HttpContext.Session;
                var guid = session.Get<string>("WeChatUserInfoId");

                var result = CacheManager.GetCache<string, User>("ExternalUserInfoCache").GetOrDefault(guid);

                Logger.Info($"Getting:{guid}:{result?.ToString()}");

                return result;
            }
        }
        #endregion

        #region 下载微信文件
        public virtual async Task<List<UploadFileInfo>> DownLoadMedia(string[] mediaIds)
        {
            var now = DateTime.Now;
            string upload_path = $"{Directory.GetCurrentDirectory()}\\wwwroot\\files\\{now.Year}\\{now.ToString("MM")}\\{now.ToString("dd")}\\";
            //报工文件
            var files = new List<UploadFileInfo>();
            foreach (var mediaId in mediaIds)
            {
                var path = await WeiXinHelper.DownloadMedia(mediaId, upload_path);
                files.Add(new UploadFileInfo()
                {
                    FilePath = FileManager.AbsolutePathToVirtualPath(path),
                    FileName=Guid.NewGuid().ToString()+System.IO.Path.GetExtension(path)
                });
            }
            return files;
        }
        #endregion

    }
}
