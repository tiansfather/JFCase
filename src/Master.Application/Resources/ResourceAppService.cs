using Abp.Authorization;
using Abp.AutoMapper;
using Master.Resources;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Master.Resources
{
    [AbpAuthorize]
    public class ResourceAppService : MasterAppServiceBase<Resource, int>
    {
        
    }
}
