using Abp.AspNetCore.Mvc.ViewComponents;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Web.Components
{
    public abstract class MasterViewComponent : AbpViewComponent
    {
        protected MasterViewComponent()
        {
            LocalizationSourceName = MasterConsts.LocalizationSourceName;
        }
    }
}
