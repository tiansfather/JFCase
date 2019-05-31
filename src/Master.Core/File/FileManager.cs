using Abp.Domain.Repositories;
using Abp.Runtime.Caching;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Master.Domain;

namespace Master
{
    public class FileManager : DomainServiceBase<File, int>, IFileManager
    {
        public string AbsolutePathToVirtualPath(string absPath)
        {
            return Common.PathHelper.AbsolutePathToVirtualPath(absPath);
        }


        public string GetFileUrl(int fileId, int w = 0, int h = 0)
        {
            return $"/File/Thumb?fileid={fileId}&w={w}&h={h}";
        }
        /// <summary>
        /// todo:文件上传方法封装，将上传路径做为参数传递
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public async Task<File> UploadFile(IFormFile file)
        {
            DateTime now = DateTime.Now;
            string upload_path = $"{Directory.GetCurrentDirectory()}\\wwwroot\\files\\{now.Year}\\{now.ToString("MM")}\\{now.ToString("dd")}";

            Directory.CreateDirectory(upload_path);

            var filename = file.FileName;
            string ext = Path.GetExtension(filename);
            //var filenamenew = DateTime.Now.ToString("yyyyMMddHHmmssfff") + ext;
            var filenamenew = System.Guid.NewGuid().ToString() + ext;

            filename = upload_path + "\\" + filenamenew;
            //path = "/images/" + now.Year + "/" + now.ToString("MMdd") + "/" + filenamenew;
            //size += file.Length;
            using (FileStream fs = System.IO.File.Create(filename))
            {
                await file.CopyToAsync(fs);
                fs.Flush();

            }
            //虚拟路径
            var virtualPath = $"/files/{now.Year}/{now.ToString("MM")}/{now.ToString("dd")}/{filenamenew}";
            var fileModel = new File()
            {
                TenantId = AbpSession.TenantId,
                FileName = file.FileName,
                FilePath = virtualPath,
                FileSize = Convert.ToDecimal(file.Length) / 1024
            };

            await InsertAndGetIdAsync(fileModel);

            return fileModel;
        }

        public async Task<File> UploadFile(string base64Content, string oriVirtualPath = "")
        {
            string filenameWithOutPath, filename, virtualPath;

            if (string.IsNullOrEmpty(oriVirtualPath))
            {
                DateTime now = DateTime.Now;
                string upload_path = $"{Directory.GetCurrentDirectory()}\\wwwroot\\files\\{now.Year}\\{now.ToString("MM")}\\{now.ToString("dd")}";
                Directory.CreateDirectory(upload_path);
                string ext = ".png";
                filenameWithOutPath = Guid.NewGuid().ToString() + ext;
                filename = upload_path + "\\" + filenameWithOutPath;
                virtualPath = $"/files/{now.Year}/{now.ToString("MM")}/{now.ToString("dd")}/{filenameWithOutPath}";
            }
            else
            {
                virtualPath = oriVirtualPath;
                filename = Common.PathHelper.VirtualPathToAbsolutePath(virtualPath);
                filenameWithOutPath = System.IO.Path.GetFileName(filename);
            }


            var data = Convert.FromBase64String(base64Content);
            //path = "/images/" + now.Year + "/" + now.ToString("MMdd") + "/" + filenamenew;
            //size += file.Length;
            using (FileStream fs = System.IO.File.Create(filename))
            {

                await fs.WriteAsync(data, 0, data.Length);
                fs.Flush();

            }
            //虚拟路径
            //virtualPath = $"/files/{now.Year}/{now.ToString("MM")}/{now.ToString("dd")}/{filenameWithOutPath}";

            var fileModel = new File()
            {
                TenantId = AbpSession.TenantId,
                FileName = filenameWithOutPath,
                FilePath = virtualPath,
                FileSize = Convert.ToDecimal(data.Length) / 1024
            };

            //如果没有传入路径，则需要新增
            if (string.IsNullOrEmpty(oriVirtualPath))
            {
                await InsertAndGetIdAsync(fileModel);
            }


            return fileModel;
        }

        public async Task DeleteFile(int fileId)
        {
            var file = await GetByIdAsync(fileId);
            if (!string.IsNullOrEmpty(file.FilePath))
            {
                try
                {
                    System.IO.File.Delete(Common.PathHelper.VirtualPathToAbsolutePath(file.FilePath));
                }
                catch
                {

                }

            }
            await DeleteAsync(file);
        }

        public async Task<File> DownLoadFile(string url)
        {
            try
            {
                DateTime now = DateTime.Now;
                string upload_path = $"{Directory.GetCurrentDirectory()}\\wwwroot\\files\\{now.Year}\\{now.ToString("MM")}\\{now.ToString("dd")}\\";

                Directory.CreateDirectory(upload_path);

                var filenameWithOutPath = Guid.NewGuid().ToString() + ".png";
                var filename = upload_path + "\\" + filenameWithOutPath;

                var downloadresult = await Senparc.CO2NET.HttpUtility.Get.DownloadAsync(url, upload_path);
                System.IO.File.Move(downloadresult, filename);


                string ext = Path.GetExtension(filename);
                //虚拟路径
                var virtualPath = $"/files/{now.Year}/{now.ToString("MM")}/{now.ToString("dd")}/{filenameWithOutPath}";
                var fileModel = new File()
                {
                    TenantId = AbpSession.TenantId,
                    FileName = filenameWithOutPath,
                    FilePath = virtualPath
                };

                await InsertAndGetIdAsync(fileModel);

                return fileModel;
            }
            catch
            {
                return null;
            }

        }
    }
}
