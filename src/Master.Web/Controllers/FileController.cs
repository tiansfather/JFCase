using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Authorization;
using Abp.Configuration.Startup;
using Abp.UI;
using Abp.Web.Models;
using Master.Configuration;
using Master.Controllers;
using Master.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Master.Web.Controllers
{
    
    public class FileController : MasterControllerBase
    {
        public IAbpStartupConfiguration Configuration { get; set; }
        public IFileManager FileManager { get; set; }
        public IHostingEnvironment HostingEnvironment { get; set; }

        #region 私有
        private async Task<UploadResult> UploadFile(IFormFile file,bool temp)
        {
            //string ext = Path.GetExtension(file.FileName);

            //DateTime now = DateTime.Now;
            //string upload_path = $"{Directory.GetCurrentDirectory()}\\wwwroot\\files\\{now.Year}\\{now.ToString("MM")}\\{now.ToString("dd")}";

            //Directory.CreateDirectory(upload_path);



            //var filenameWithOutPath = Guid.NewGuid().ToString() + ext;
            //var filename = upload_path + "\\" + filenameWithOutPath;
            ////path = "/images/" + now.Year + "/" + now.ToString("MMdd") + "/" + filenamenew;
            ////size += file.Length;
            //using (FileStream fs = System.IO.File.Create(filename))
            //{
            //    await file.CopyToAsync(fs);
            //    fs.Flush();

            //}
            ////虚拟路径
            //var virtualPath = $"/files/{now.Year}/{now.ToString("MM")}/{now.ToString("dd")}/{filenameWithOutPath}";
            var uploadFile = await FileManager.UploadFile(file,temp);

            var result = new UploadResult { Success = true, FilePath = uploadFile.FilePath, FileName = file.FileName ,FileId=uploadFile.Id};
            return result;
        }

        private async Task<UploadResult> UploadFile(string base64Content,string oriVirtualPath="")
        {
            //string ext = ".png";

            //DateTime now = DateTime.Now;
            //string upload_path = $"{Directory.GetCurrentDirectory()}\\wwwroot\\files\\{now.Year}\\{now.ToString("MM")}\\{now.ToString("dd")}";

            //Directory.CreateDirectory(upload_path);



            //var filenameWithOutPath = Guid.NewGuid().ToString() + ext;
            //var filename = upload_path + "\\" + filenameWithOutPath;
            ////path = "/images/" + now.Year + "/" + now.ToString("MMdd") + "/" + filenamenew;
            ////size += file.Length;
            //using (FileStream fs = System.IO.File.Create(filename))
            //{
            //    var data = Convert.FromBase64String(base64Content);
            //    await fs.WriteAsync(data, 0, data.Length);
            //    fs.Flush();

            //}
            ////虚拟路径
            //var virtualPath = $"/files/{now.Year}/{now.ToString("MM")}/{now.ToString("dd")}/{filenameWithOutPath}";

            var uploadFile = await FileManager.UploadFile(base64Content, oriVirtualPath);

            var result = new UploadResult { Success = true, FilePath = uploadFile.FilePath, FileName = uploadFile.FileName ,FileId=uploadFile.Id};
            return result;
        }
        #endregion

        /// <summary>
        /// 通用上传
        /// </summary>
        /// <returns></returns>
        public IActionResult CommonUpload()
        {
            var uploadProviders = Configuration.Modules.WebCore().UploadProviders;
            return View(uploadProviders);
        }

        /// <summary>
        /// 截图上传方式提交
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> UploadByBase64(string data,string oriVirtualPath)
        {
            var base64Content = data.Replace("data:image/png;base64,", "");
            var result = await UploadFile(base64Content, oriVirtualPath);

            return Json(result);
        }

        /// <summary>
        /// 文件上传
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AbpAllowAnonymous]
        public async Task<JsonResult> Upload(bool temp)
        {
            //throw new UserFriendlyException("Not Available");
            //if(new Random().Next(1, 4) > 2)
            //{
            //    return Json(new UploadResult { Success = false, Msg = "请稍候重试" });
            //}
            try
            {
                var files = HttpContext.Request.Form.Files;
                if (files.Count > 0)
                {
                    //可以写遍历files
                    var file = files[0];
                    string ext = Path.GetExtension(file.FileName);

                    var result = await UploadFile(file, temp);

                    return Json(result);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message+ex.StackTrace);
                return Json(new UploadResult { Success = false, Msg = "系统繁忙,请稍候重试" });
            }


            throw new UserFriendlyException("未找到上传文件");
        }

        public ActionResult DownLoad(string fileName, string filePath)
        {
            return File(filePath, "application/octet-stream", fileName);
        }

        [AbpMvcAuthorize]
        [Route("[controller]/[action]")]
        [HttpPost]
        public async Task<JsonResult> UploadByContent([FromBody]string html, string filePath)
        {
            var absPath = Common.PathHelper.VirtualPathToAbsolutePath(filePath);
            var directory = System.IO.Path.GetDirectoryName(absPath);
            System.IO.Directory.CreateDirectory(directory);
            await System.IO.File.WriteAllTextAsync(absPath, html,Encoding.Default);
            return Json(new { });
        }

        /// <summary>
        /// 通过文件名获了文件的Content-Type
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        private string GetFileContentType(string filename)
        {
            //todo:通过文件名获了文件的Content-Type
            var ext = System.IO.Path.GetExtension(filename);
            //var mimeType = MimeMapping.GetMimeMapping(fileName);
            string ContentType = "";
            switch (ext)
            {
                case ".png":
                    ContentType = "image/png";
                    break;
                case ".jpg":
                case ".jpe":
                case ".jpeg":
                    ContentType = "image/jpeg";
                    break;
                default:
                    ContentType = "application/octet-stream";
                    break;
            }
            return ContentType;
        }

        /// <summary>
        /// 获取图片缩略
        /// </summary>
        /// <param name="fileid"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        [Route("[controller]/[action]/{fileId}")]
        [HttpGet]
        public async Task<FileResult> Thumb(int? fileid, int w = 0, int h = 0)
        {
            //没有文件的预处理方法
            if (fileid == null)
            {
                return null;
            }

            var file = await FileManager.GetByIdFromCacheAsync(Convert.ToInt32(fileid));
            //没有文件的预处理方法
            if (file == null)
            {
                return null;
            }

            //获取图片缩略 2018/5/23/15:21 lijianbo
            //路径需要物理路径
            //var fullpath = HostingEnvironment.ContentRootPath + "\\wwwroot" + file.FilePath.Replace("/", "\\");
            var fullpath = Common.PathHelper.VirtualPathToAbsolutePath(file.FilePath);


            using (var filenow = Common.ImageHelper.ThumbImageToStream(fullpath, w, h==0?w:h))
            {

                return File(filenow.ToArray(), GetFileContentType(file.FileName), file.FileName);
            }
            //return null;
        }
    }
}