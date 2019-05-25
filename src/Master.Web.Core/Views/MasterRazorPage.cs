using Abp.AspNetCore.Mvc.Views;
using Abp.Dependency;
using Abp.Runtime.Session;
using Master;
using Microsoft.AspNetCore.Mvc.Razor.Internal;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Web.Views
{
    public abstract class MasterRazorPage<TModel> : AbpRazorPage<TModel>
    {
        [RazorInject]
        public IAbpSession AbpSession { get; set; }

        [RazorInject]
        public IIocManager IocManager { get; set; }
        [RazorInject]
        public IFileManager FileManager { get; set; }
        protected MasterRazorPage()
        {
            LocalizationSourceName = MasterConsts.LocalizationSourceName;
        }
    }
}
