using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Master.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Master.Web.Controllers
{
    public class BaseTypeController : MasterControllerBase
    {
        public IActionResult Add(string typeKey)
        {
            ViewBag.TypeKey = typeKey;
            return View();
        }

        public async Task<IActionResult> Edit(int id)
        {
            return View();
        }
    }
}