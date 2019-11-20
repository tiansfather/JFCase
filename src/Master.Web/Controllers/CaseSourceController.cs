using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Reflection;
using Master.Authentication;
using Master.Configuration;
using Master.Controllers;
using Master.EntityFrameworkCore;
using Master.Module;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;

namespace Master.Web.Controllers
{
    
    public class CaseSourceController : MasterModuleControllerBase
    {
        [AbpMvcAuthorize]
        public IActionResult Index()
        {
            return View();
        }
        [AbpMvcAuthorize]
        public IActionResult Import()
        {
            return View();
        }
        public IActionResult InitialView()
        {
            return View();
        }
    }
}