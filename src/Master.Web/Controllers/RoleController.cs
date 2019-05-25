using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Master.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Master.Web.Controllers
{
    [AbpMvcAuthorize("Menu.Admin.Tenancy.Role")]
    public class RoleController : MasterControllerBase
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}