using Abp.Domain.Services;
using System.Threading.Tasks;
using System.Collections.Generic;
using Master.Domain;
using Microsoft.AspNetCore.Http;

namespace Master
{
    public interface IFileManager : IData<File,int>
    {
        /// <summary>
        /// 获取文件访问路径
        /// </summary>
        /// <param name="fileId"></param>
        /// <param name="w"></param>
        /// <param name="h"></param>
        /// <returns></returns>
        string GetFileUrl(int fileId,int w=0,int h=0);
        /// <summary>
        /// 上传文件
        /// todo:将上传路径也做为参数传递
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        Task<File> UploadFile(IFormFile file,bool temp);
        /// <summary>
        /// 将base64转为文件
        /// </summary>
        /// <param name="base64Content"></param>
        /// <returns></returns>
        Task<File> UploadFile(string base64Content, string oriVirtualPath = "");
        /// <summary>
        /// 从url下载文件
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        Task<File> DownLoadFile(string url);
        /// <summary>
        /// 删除文件记录
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns></returns>
        Task DeleteFile(int fileId);
        /// <summary>
        /// 物理路径转虚拟路径
        /// </summary>
        /// <param name="absPath"></param>
        /// <returns></returns>
        string AbsolutePathToVirtualPath(string absPath);
    }
}