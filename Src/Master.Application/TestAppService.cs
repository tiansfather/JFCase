using Abp.Authorization;
using Abp.UI;
using Abp.Web.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master
{
    [AbpAllowAnonymous]
    public class TestAppService : MasterAppServiceBase
    {
        public void Test()
        {
            throw new UserFriendlyException("error");
        }
    }
}