using SharpCompress.Archives;
using SharpCompress.Common;
using System;
using System.Collections.Generic;
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
        public static void Decompression(string targetFile, string zipFile)
        {
            using (var archive = ArchiveFactory.Open(targetFile))
            {
                foreach (var entry in archive.Entries)
                {
                    if (!entry.IsDirectory)
                    {
                        entry.WriteToDirectory(zipFile);
                    }
                }
            }                
        }

        public static IEnumerable<string> GetFileNames(string targetFile)
        {
            using (var archive = ArchiveFactory.Open(targetFile))
            {
                return archive.Entries.Select(o => o.Key);
            }
        }
    }
}
