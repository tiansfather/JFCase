using Common;
using Master.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.DrawingCore.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Master.Web.Controllers
{
    public class VerifyCodeController: MasterControllerBase
    {
        /// <summary>
        /// 数字验证码
        /// </summary>
        /// <returns></returns>
        public FileContentResult NumberVerifyCode()
        {
            string code = VerifyCodeHelper.GetSingleObj().CreateVerifyCode(VerifyCodeHelper.VerifyCodeType.NumberVerifyCode);
            HttpContext.Session.Set("CaptchaCode", code);
            byte[] codeImage = VerifyCodeHelper.GetSingleObj().CreateByteByImgVerifyCode(code, 100, 40);
            return File(codeImage, @"image/jpeg");
        }

        /// <summary>
                /// 字母验证码
                /// </summary>
                /// <returns></returns>
        public FileContentResult AbcVerifyCode()
        {
            string code = VerifyCodeHelper.GetSingleObj().CreateVerifyCode(VerifyCodeHelper.VerifyCodeType.AbcVerifyCode);
            HttpContext.Session.Set("CaptchaCode", code);
            var bitmap = VerifyCodeHelper.GetSingleObj().CreateBitmapByImgVerifyCode(code, 100, 40);
            MemoryStream stream = new MemoryStream();
            bitmap.Save(stream, ImageFormat.Png);
            return File(stream.ToArray(), "image/png");
        }

        /// <summary>
                /// 混合验证码
                /// </summary>
                /// <returns></returns>
        public FileContentResult MixVerifyCode()
        {
            string code = VerifyCodeHelper.GetSingleObj().CreateVerifyCode(VerifyCodeHelper.VerifyCodeType.MixVerifyCode);
            HttpContext.Session.Set("CaptchaCode", code);
            var bitmap = VerifyCodeHelper.GetSingleObj().CreateBitmapByImgVerifyCode(code, 100, 40);
            MemoryStream stream = new MemoryStream();
            bitmap.Save(stream, ImageFormat.Gif);
            return File(stream.ToArray(), "image/gif");
        }

    }
}
