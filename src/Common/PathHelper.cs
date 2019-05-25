using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Common
{
    public static class PathHelper
    {
        public static string AbsolutePathToVirtualPath(string absPath)
        {
            var rootDic = Directory.GetCurrentDirectory() + "\\wwwroot";
            return absPath.Replace("\\\\", "\\").Replace(rootDic, "").Replace("\\", "/");
        }

        public static string VirtualPathToAbsolutePath(string virtualPath)
        {
            var rootDic = Directory.GetCurrentDirectory() + "\\wwwroot";
            return rootDic+virtualPath.Replace("/", "\\");
        }
    }
}
