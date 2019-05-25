using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Master.Controllers;
using Master.Imports;
using Microsoft.AspNetCore.Mvc;

namespace Master.Web.Controllers
{
    [AbpMvcAuthorize]
    public class ImportController : MasterControllerBase
    {
        public ImportManager ImportManager { get; set; }
        public IActionResult Index(string type,string parameter)
        {
            ViewBag.Type = type;
            ViewBag.Parameter =string.IsNullOrEmpty( parameter)?"{}":parameter;
            ViewBag.Fields = ImportManager.GetImportFieldInfosFromType(type);
            return View();
        }
    }
}