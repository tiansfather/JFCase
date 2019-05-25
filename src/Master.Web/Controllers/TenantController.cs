using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Master.Application.Editions;
using Master.Controllers;
using Master.MultiTenancy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Master.Web.Controllers
{
    [AbpMvcAuthorize]
    public class TenantController : MasterControllerBase
    {
        public EditionManager EditionManager { get; set; }
        public TenantManager TenantManager { get; set; }
        public async Task<IActionResult> SetEdition(int data)
        {
            var editions =await EditionManager.GetAll().ToListAsync();
            var tenant = await TenantManager.GetByIdFromCacheAsync(data);
            ViewData["Editions"] = editions;
            ViewData["EditionId"] = tenant.EditionId;
            return View();
        }
    }
}