using CoreHtmlToImage;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Text;

namespace Common
{
    public class Fun
    {

        #region 嵌入资源读取
        public static string ReadEmbedString(Assembly asm, string path)
        {
            var fileProvider = new EmbeddedFileProvider(asm);

            var fileInfo = fileProvider.GetFileInfo(path);
            return ReadEmbedString(fileInfo);

        }
        public static Stream ReadEmbedStream(Assembly asm,string path)
        {
            var fileProvider = new EmbeddedFileProvider(asm);
            var fileInfo = fileProvider.GetFileInfo(path);
            return fileInfo.CreateReadStream();
        }
        /// <summary>
        /// 读取文件内容
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <returns></returns>
        public static string ReadEmbedString(IFileInfo fileInfo)
        {

            if (fileInfo.Exists)
            {
                byte[] buffer;
                using (Stream readStream = fileInfo.CreateReadStream())
                {
                    buffer = new byte[readStream.Length];
                    readStream.Read(buffer, 0, buffer.Length);
                }
                return Encoding.UTF8.GetString(buffer);
            }
            //如果文件不存在
            return string.Empty;
        }
        /// <summary>
        /// 获取程序集中指定路径下的嵌入文件
        /// </summary>
        /// <param name="asm"></param>
        /// <param name="baseNameSpace">命名空间:Master.Storage.Forms</param>
        /// <returns></returns>
        public static IEnumerable<IFileInfo> GetFilesInAsm(Assembly asm, string baseNameSpace)
        {
            var fileProvider = new EmbeddedFileProvider(asm, baseNameSpace);
            return fileProvider.GetDirectoryContents("");
        }
        #endregion

        /// <summary>
        /// 日期转换为时间戳（时间戳单位秒）
        /// </summary>
        /// <param name="TimeStamp"></param>
        /// <returns></returns>
        public static long ConvertToTimeStamp(DateTime time)
        {
            DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return (long)(time.AddHours(-8) - Jan1st1970).TotalMilliseconds;
        }

        /// <summary>
        /// 时间戳转换为日期（时间戳单位秒）
        /// </summary>
        /// <param name="TimeStamp"></param>
        /// <returns></returns>
        public static DateTime ConvertToDateTime(long timeStamp)
        {
            var start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return start.AddMilliseconds(timeStamp).AddHours(8);
        }

    }
}
