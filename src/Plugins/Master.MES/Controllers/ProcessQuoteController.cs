using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Controllers
{
    [AbpMvcAuthorize]
    public class ProcessQuoteController: MasterModuleControllerBase
    {
        public IActionResult Submit()
        {
            return View();
        }

        public IActionResult Show()
        {
            return View();
        }
    }
}
