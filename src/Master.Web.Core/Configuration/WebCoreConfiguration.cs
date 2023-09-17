using Abp.Configuration.Startup;
using Master.Web.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Configuration
{
    public class WebCoreConfiguration
    {
        /// <summary>
        /// 共享视图
        /// </summary>
        public List<string> CommonViews { get; } = new List<string>();

        /// <summary>
        /// 上传提供者
        /// </summary>
        public List<UploadProvider> UploadProviders { get; } = new List<UploadProvider>();

        /// <summary>
        /// 系统名称
        /// </summary>
        public string SoftName { get; set; } = "简法案例";

        public string BaseUrl { get; set; }
    }

    public static class WebCoreConfigurationExtension
    {
        public static WebCoreConfiguration WebCore(this IModuleConfigurations moduleConfigurations)
        {
            return moduleConfigurations.AbpConfiguration.Get<WebCoreConfiguration>();
        }
    }
}