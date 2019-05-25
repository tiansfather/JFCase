using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Web.Configuration
{
    /// <summary>
    /// 通用上传提供者,用于在File/CommonUpload中使用
    /// </summary>
    public class UploadProvider
    {
        public string ProviderName { get; set; }
        public string ProviderUrl { get; set; }
    }
}
