using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Reflection;
using Master.Authentication;
using Master.Case;
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
        public CaseSourceHistoryManager CaseSourceHistoryManager { get; set; }
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
        [AbpMvcAuthorize]
        public async Task<IActionResult> History(int id)
        {
            var historys=await CaseSourceHistoryManager.Repository.GetAll().Include(o=>o.CreatorUser).Where(o => o.CaseSourceId == id).ToListAsync();
            return View(historys);
        }
        public IActionResult InitialView()
        {
            return View();
        }
    }
}