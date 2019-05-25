using Abp.AspNetCore.Mvc.Authorization;
using Abp.Runtime.Security;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Controllers
{
    public class MiniProgramController : MasterControllerBase
    {
        /// <summary>
        /// 扫码绑定小程序
        /// </summary>
        /// <returns></returns>
        [AbpMvcAuthorize]
        public ActionResult BindLogin(long? userId)
        {

            var encryptedUserId = SimpleStringCipher.Instance.Encrypt((userId.HasValue ? userId.Value : AbpSession.UserId.Value).ToString(), AppConsts.DefaultPassPhrase);
            ViewBag.UserId = encryptedUserId;
            return View();
        }

        /// <summary>
        /// 绑定显示页
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public ActionResult BindLoginCallback(string userid)
        {            
            ViewBag.UserId = userid;
            return View();
        }
    }
}
