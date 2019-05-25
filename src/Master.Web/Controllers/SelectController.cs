using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Master.Controllers;
using Master.Web.Models.Select;
using Microsoft.AspNetCore.Mvc;

namespace Master.Web.Controllers
{
    public class SelectController :  MasterControllerBase
    {
        public IActionResult SelUser(SelectFormViewModel model)
        {
            return View(model);
        }
    }
}