using Abp.Auditing;
using Abp.Web.Models;
using Master.Controllers;
using Microsoft.AspNetCore.Mvc;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

namespace Master.Web.Controllers
{    
    public class QRCodeController: MasterControllerBase
    {
        [Route("api/qrcode")]
        [DisableAuditing]
        [DontWrapResult]
        [HttpGet]
        public void GetQRCode(string url, int pixel=5)
        {
            if (string.IsNullOrEmpty(url))
            {
                return;
            }
            Response.ContentType = "image/jpeg";

            QRCodeGenerator generator = new QRCodeGenerator();
            QRCodeData codeData = generator.CreateQrCode(url, QRCodeGenerator.ECCLevel.M, true);
            QRCoder.QRCode qrcode = new QRCoder.QRCode(codeData);

            Bitmap qrImage = qrcode.GetGraphic(pixel, Color.Black, Color.White, true);

            using(var ms = new MemoryStream())
            {
                qrImage.Save(ms, ImageFormat.Jpeg);
                Response.Body.WriteAsync(ms.GetBuffer(), 0, Convert.ToInt32(ms.Length));
                Response.Body.Close();
            }

            
        }

    }
}
