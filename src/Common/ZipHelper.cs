using SharpCompress.Archives;
using SharpCompress.Common;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
namespace Common
{
    public class ZipHelper
    {
        /// <summary>
        /// 解压文件
        /// </summary>
        /// <param name="targetFile">解压文件路径</param>
        /// <param name="zipFile">解压文件后路径</param>
        public static IEnumerable<string> Decompression(string targetFile, string zipFile)
        {
            if (System.IO.Path.GetExtension(targetFile).ToLower().Contains("zip"))
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                ZipFile.ExtractToDirectory(targetFile, zipFile, Encoding.GetEncoding("GB2312"), true);
                return GetFileNames(targetFile);
            }
            else
            {
                var result = new List<string>();
                using (var archive = ArchiveFactory.Open(targetFile))
                {
                    foreach (var entry in archive.Entries)
                    {
                        if (!entry.IsDirectory)
                        {
                            result.Add(entry.Key);
                            entry.WriteToDirectory(zipFile);
                        }
                    }
                }
                return result;
            }


        }

        public static IEnumerable<string> GetFileNames(string targetFile)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            using (var archive = ZipFile.Open(targetFile, ZipArchiveMode.Read, Encoding.GetEncoding("GB2312")))
            {
                return archive.Entries.Select(o => o.Name);
            }
                
            //using (var archive = ArchiveFactory.Open(targetFile))
            //{
            //    return archive.Entries.Select(o => o.Key);
            //}
        }
    }
}
