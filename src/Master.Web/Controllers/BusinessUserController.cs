using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Master.Controllers;
using Master.Web.Models.Select;
using Microsoft.AspNetCore.Mvc;

namespace Master.Web.Controllers
{
    [AbpMvcAuthorize]
    public class BusinessUserController :  MasterControllerBase
    {
        public IActionResult Assistant()
        {
            return View();
        }
        public IActionResult Charger()
        {
            return View();
        }
        public IActionResult Miner()
        {
            return View();
        }
        public IActionResult NewMiner()
        {
            return View();
        }
    }
}