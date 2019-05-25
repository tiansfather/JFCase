using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.BackgroundJobs;
using Abp.Domain.Repositories;
using Master.Authentication;
using Master.Entity;
using Master.EntityFrameworkCore;
using Master.MES;
using Master.MES.Jobs;
using Master.MES.Service;
using Master.Projects;
using Master.WeiXin;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;
using Senparc.CO2NET.Extensions;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.TemplateMessage;
using Senparc.Weixin.MP.Containers;
using Senparc.Weixin.MP.Helpers;

namespace Master.Controllers
{
    [AbpMvcAuthorize]
    public class EquipmentController: MasterModuleControllerBase
    {
        

    }
}
